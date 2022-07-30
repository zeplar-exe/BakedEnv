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
        var session = new BakedEnvironmentBuilder()
            .WithStatementHandlers(new MockStatementHandler()).Build()
            .CreateSession(new RawStringSource($"[Pizza: \"Time\"]"))
            .Init();
        session.ExecuteUntilEnd();
        
        session.AssertInterpreterHasVariable("Pizza", "Time");
    }

    [Test]
    public void TestWhitespace()
    {
        var session = new BakedEnvironmentBuilder()
            .WithStatementHandlers(new MockStatementHandler()).Build()
            .CreateSession(new RawStringSource($"[    NaN: \t 0 \n ]"))
            .Init();
        session.ExecuteUntilEnd();

        session.AssertInterpreterHasVariable("NaN", 0);
    }

    private class MockStatementHandler : IProcessorStatementHandler
    {
        public bool TryHandle(ProcessorStatementInstruction instruction, InvocationContext context)
        {
            context.Interpreter.AssertReady();

            var key = instruction.Key.Evaluate(context);
            
            context.Interpreter.Context.Variables.Add(key.ToString(), instruction.Expression.Evaluate(context));
            
            return true;
        }
    }
}