using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.Sources;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class ProcessorInstructions
{
    [Test]
    public void TestProcessorStatementParsing()
    {
        var source = new RawStringSource("[BakeType: \"SomeBakeType\"]");
        var interpreter = new BakedInterpreter()
            .WithDefaultStatementHandler()
            .WithSource(source);
        
        interpreter.Init();

        if (!interpreter.TryGetNextInstruction(out var instruction))
            Assert.Fail();

        var processorInstruction = TestHelper.AssertIsType<ProcessorStatementInstruction>(instruction);
        processorInstruction.Execute(interpreter);

        Assert.True(processorInstruction.Value.Equals("SomeBakeType"));
    }
    
    [Test]
    public void TestProcessorStatementExecution()
    {
        var source = new RawStringSource($"[BakeType: \"{nameof(BakeType.Module)}\"]");
        var interpreter = new BakedInterpreter()
            .WithDefaultStatementHandler()
            .WithSource(source);
        
        interpreter.Init();

        if (!interpreter.TryGetNextInstruction(out var instruction))
            Assert.Fail();

        var processorInstruction = TestHelper.AssertIsType<ProcessorStatementInstruction>(instruction);
        processorInstruction.Execute(interpreter);
        
        Assert.True(interpreter.Context.BakeType == BakeType.Module);
    }

    [Test]
    public void TestWhitespace()
    {
        var source = new RawStringSource($"[    BakeType: \t \"{nameof(BakeType.Module)}\" \n ]");
        var interpreter = new BakedInterpreter()
            .WithDefaultStatementHandler()
            .WithSource(source);
        
        interpreter.Init();

        if (!interpreter.TryGetNextInstruction(out var instruction))
            Assert.Fail();

        var processorInstruction = TestHelper.AssertIsType<ProcessorStatementInstruction>(instruction);
        processorInstruction.Execute(interpreter);
        
        Assert.True(interpreter.Context.BakeType == BakeType.Module);
    }
    
}