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
        ProcessorStatementInstruction? processorStatement = null;
        
        var session = new BakedEnvironment().CreateSession(new RawStringSource("[BakeType: \"SomeBakeType\"]")).Init();
        session.ExecuteUntil(i => TestHelper.ObjectIs(i, out processorStatement));
        
        if (processorStatement == null)
            Assert.Fail();
        
        processorStatement!.Execute(session.Interpreter);

        Assert.True(processorStatement!.Value.Equals("SomeBakeType"));
    }
    
    [Test]
    public void TestProcessorStatementExecution()
    {
        ProcessorStatementInstruction? processorStatement = null;
        
        var session = new BakedEnvironment().CreateSession(new RawStringSource($"[BakeType: \"{nameof(BakeType.Module)}\"]")).Init();
        session.ExecuteUntil(i => TestHelper.ObjectIs(i, out processorStatement));

        if (processorStatement == null)
            Assert.Fail();
        
        processorStatement!.Execute(session.Interpreter);
        
        Assert.True(session.Interpreter.Context!.BakeType == BakeType.Module);
    }

    [Test]
    public void TestWhitespace()
    {
        ProcessorStatementInstruction? processorStatement = null;
        
        var session = new BakedEnvironment()
            .CreateSession(new RawStringSource($"[    BakeType: \t \"{nameof(BakeType.Module)}\" \n ]"))
            .Init();
        session.ExecuteUntil(i => TestHelper.ObjectIs(i, out processorStatement));

        if (processorStatement == null)
            Assert.Fail();
        
        processorStatement!.Execute(session.Interpreter);

        Assert.True(session.Interpreter.Context!.BakeType == BakeType.Module);
    }
    
}