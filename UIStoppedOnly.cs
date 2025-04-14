using System;
using System.Drawing;
using System.Windows.Forms;
using TrainCrew;

class UIStoppedOnly: Form
{
	public static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new UIStoppedOnly());
	}

	private UIStoppedOnly()
	{
		this.Text = "UIStoppedOnly";
		this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
		this.MaximizeBox = false;
		this.Load += LoadHandler;
	}

	private Timer timer;
	private bool inited = false;
	private bool inGame = false;
	private bool stopped = true;
	private bool pressed = false;
	private bool checkPhase = false;

	private void LoadHandler(object sender, EventArgs e)
	{
		TrainCrewInput.Init();
		inited = true;
		timer = new Timer();
		timer.Interval = 25;
		timer.Tick += TickHandler;
		timer.Start();
		this.Closed += ClosedHandler;
	}

	private void ClosedHandler(object sender, EventArgs e)
	{
		timer.Stop();
		inited = false;
		if (pressed)
		{
			TrainCrewInput.SetButton(InputAction.ViewUserInterface, false);
		}
		TrainCrewInput.Dispose();
	}

	private void TickHandler(object sender, EventArgs e)
	{
		if (!inited) return;
		checkPhase = !checkPhase;
		if (!checkPhase)
		{
			if (pressed)
			{
				TrainCrewInput.SetButton(InputAction.ViewUserInterface, false);
				pressed = false;
			}
			return;
		}
		GameScreen gs = TrainCrewInput.gameState.gameScreen;
		bool newInGame = gs == GameScreen.MainGame || gs == GameScreen.MainGame_Pause;
		bool newStopped = TrainCrewInput.GetTrainState().Speed == 0;
		if (!inGame && newInGame)
		{
			// ゲームを開始した → 停車状態をリセット
			stopped = newStopped;
		}
		if (newInGame && stopped != newStopped)
		{
			// 停車状態が変化した → UIの表示切り替えボタンを押す
			TrainCrewInput.SetButton(InputAction.ViewUserInterface, true);
			pressed = true;
		}
		inGame = newInGame;
		stopped = newStopped;
	}
}
