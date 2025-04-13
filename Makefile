TARGET=UIStoppedOnly.exe
OPTIONS= \
	/target:winexe \
	/optimize+ \
	/warn:4 \
	/codepage:65001 \
	/reference:TrainCrewInput.dll

SOURCES= \
	AssemblyInfo.cs \
	UIStoppedOnly.cs

$(TARGET): $(SOURCES)
	csc /out:$@ $(OPTIONS) $^
