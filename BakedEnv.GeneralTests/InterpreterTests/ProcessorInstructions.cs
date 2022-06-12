using System;
using System.Collections.Generic;
using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Interpreter.Sources;
using BakedEnv.Objects;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class ProcessorInstructions
{
    [Test]
    public void TestProcessorStatementParsing()
    {
        ProcessorStatementInstruction? processorStatement = null;
        
        var session = new BakedEnvironment().CreateSession(new RawStringSource("[BakeType: \"Cake\"]")).Init();
        session.ExecuteUntil(i => TestHelper.ObjectIs(i, out processorStatement));
        
        if (processorStatement == null)
            Assert.Fail();
        
        processorStatement!.Execute(session.Interpreter);

        Assert.True(processorStatement!.Value.Equals("Cake"));
    }
    
    [Test]
    public void TestProcessorStatementExecution()
    {
        ProcessorStatementInstruction? processorStatement = null;
        
        var session = new BakedEnvironment()
            .WithStatementHandlers(new MockStatementHandler())
            .CreateSession(new RawStringSource($"[Pizza: \"Time\"]"))
            .Init();
        session.ExecuteUntil(i => TestHelper.ObjectIs(i, out processorStatement));

        if (processorStatement == null)
            Assert.Fail();
        
        processorStatement!.Execute(session.Interpreter);
        
        Assert.True(session.Interpreter.Context.Variables["Pizza"].Equals("Time"));
    }

    [Test]
    public void TestWhitespace()
    {
        ProcessorStatementInstruction? processorStatement = null;
        
        var session = new BakedEnvironment()
            .WithStatementHandlers(new MockStatementHandler())
            .CreateSession(new RawStringSource($"[    NaN: \t 0 \n ]"))
            .Init();
        session.ExecuteUntil(i => TestHelper.ObjectIs(i, out processorStatement));

        if (processorStatement == null)
            Assert.Fail();
        
        processorStatement!.Execute(session.Interpreter);

        Assert.True(session.Interpreter.Context.Variables["NaN"].Equals(0));
    }

    private class MockStatementHandler : IProcessorStatementHandler
    {
        public bool TryHandle(ProcessorStatementInstruction instruction, BakedInterpreter interpreter)
        {
            interpreter.Context.Variables[instruction.Name] = instruction.Value;
            
            return true;
        }
    }
}