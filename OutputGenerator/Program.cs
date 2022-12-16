// err 100 40   
// Output 100 lines, 40 chars long, to StdErr
//
// out 50 10 80
// Output 50 lines, 10 to 80 chars long, to StdOut

using NSKurtsTools;

const string argNameOutChannel = "outChannel";
const string argNameLineCount = "lineCount";
const string argNameLineLength = "lineLength";
const string argNameMinLineLength = "minLineLength";
const string argNameMaxLineLength = "maxLineLength";

ArgumentModel argumentModel = new();
argumentModel.AddSimpleKeySet(argNameOutChannel, argNameLineCount, argNameLineLength);
argumentModel.AddSimpleKeySet(argNameOutChannel, argNameLineCount, argNameMinLineLength, argNameMaxLineLength);
argumentModel.AddSwitchKey(new[]{ "/?", "/h" });

ArgumentAnalyzer argumentAnalyzer = new(argumentModel, args);

string outChannel = argumentAnalyzer.GetSimpleArgument(argNameOutChannel);

TextWriter channel = null!;
switch (outChannel){
    case "err":
        channel = Console.Error;
        break;
    case "out":
        channel = Console.Out;
        break;
    default:
        Console.Error.WriteLine($"\"{outChannel}\" is not a valid out channel. Must be \"out\" or \"err\"");
        Environment.Exit(1);
        break;
}

int lineCount=int.Parse(argumentAnalyzer.GetSimpleArgument(argNameLineCount));

bool fixedLineLength;
int lineLength = 0, minLineLength = 0, maxLineLength = 0;

if (argumentAnalyzer.SimpleKeyExists(argNameLineLength)){
    fixedLineLength = true;
    lineLength =int.Parse( argumentAnalyzer.GetSimpleArgument(argNameLineLength));
}
else{
    fixedLineLength = false;
    minLineLength =int.Parse( argumentAnalyzer.GetSimpleArgument(argNameMinLineLength));
    maxLineLength =int.Parse( argumentAnalyzer.GetSimpleArgument(argNameMaxLineLength));
}

Random random=new();

string GetLine(){
    int length = fixedLineLength ? lineLength : random.Next(minLineLength, maxLineLength);

    string s="";
    for (int n = 0; n < length;n++) s += (char)random.Next(32, 127);

    return s;
}

for (int n = 0; n < lineCount; n++){
    string s = GetLine();
    channel.WriteLine(s);
}

channel.Flush();

channel.Close();
