﻿using System.Text;
using BakedEnv;
using BakedEnv.CLI;
using BakedEnv.Interpreter.Sources;
using CommandLine;

var parserResult = Parser.Default.ParseArguments<CommandArgs, CommandArgs.ExecuteArgs>(args);

parserResult.MapResult(
    (CommandArgs options) => ParseMainArgs(options),
    (CommandArgs.ExecuteArgs options) => ParseExecuteArgs(options),
    errors => 1);

int ParseMainArgs(CommandArgs mainArgs)
{
    return 0;
}

int ParseExecuteArgs(CommandArgs.ExecuteArgs executeArgs)
{
    IBakedSource source;
    
    if (executeArgs.FilePath != null)
    {
        var fullFilePath = executeArgs.FilePath!;

        if (!Path.IsPathRooted(executeArgs.FilePath))
            fullFilePath = Path.Join(Directory.GetCurrentDirectory(), executeArgs.FilePath);

        source = new FileSource(fullFilePath, Encoding.Default);
    }
    else
    {
        source = new RawStringSource(executeArgs.RawString!);
    }

    var environment = new BakedEnvironment();
    environment.Invoke(source);

    return 0;
}