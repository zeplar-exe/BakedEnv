using BakedEnv.Interpreter;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Interpreter.Scopes;
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
        
        var session = new BakedEnvironment().CreateSession(new RawStringSource("[BakeType: \"Cake\"]")).Init();
        session.ExecuteUntil(i => TestHelper.ObjectIs(i, out processorStatement));
        
        if (processorStatement == null)
            Assert.Fail();
        
        processorStatement!.Execute(session.Interpreter);

        var expression = processorStatement!.Expression;
        
        Assert.True(expression is ValueExpression value && value.Value.Equals("Cake"));
    }
    
    [Test]
    public void TestProcessorStatementExecution()
    {
        var session = new BakedEnvironment()
            .WithStatementHandlers(new MockStatementHandler())
            .CreateSession(new RawStringSource($"[Pizza: \"Time\"]"))
            .Init();
        session.ExecuteUntilEnd();
        
        Assert.True(session.TopVariables["Pizza"].Value.Equals("Time"));
    }

    [Test]
    public void TestWhitespace()
    {
        var session = new BakedEnvironment()
            .WithStatementHandlers(new MockStatementHandler())
            .CreateSession(new RawStringSource($"[    NaN: \t 0 \n ]"))
            .Init();
        session.ExecuteUntilEnd();

        Assert.True(session.TopVariables["NaN"].Value.Equals(0));
    }

    private class MockStatementHandler : IProcessorStatementHandler
    {
        public bool TryHandle(ProcessorStatementInstruction instruction, BakedInterpreter interpreter)
        {
            var context = new InvocationContext(interpreter.Context);
            
            interpreter.Context.Variables.Add(instruction.Name, instruction.Expression.Evaluate(interpreter, context));
            
            return true;
        }
    }
}