using BakedEnv.Environment;
using BakedEnv.Interpreter.Expressions;
using BakedEnv.Interpreter.Instructions;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Interpreter.Scopes;

using NUnit.Framework;

namespace BakedEnv.GeneralTests.InterpreterTests;

[TestFixture]
public class ProcessorInstructions
{
    [Test]
    public void TestProcessorStatementParsing()
    {
        ProcessorStatementInstruction? processorStatement = null;
        
        var session = InterpreterTestHelper.CreateSession("[BakeType: \"Cake\"]");
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
            .CreateSession("[\"Pizza\": \"Time\"]");
        session.ExecuteUntilError();
        
        session.AssertInterpreterHasVariable("Pizza", "Time");
    }

    [Test]
    public void TestWhitespace()
    {
        var session = new BakedEnvironmentBuilder()
            .WithStatementHandlers(new MockStatementHandler())
            .Build()
            .CreateSession("[    NaN: \t 0 \n ]");
        session.ExecuteUntilError();

        session.AssertInterpreterHasVariable("NaN", 0);
    }

    private class MockStatementHandler : IProcessorStatementHandler
    {
        public bool TryHandle(ProcessorStatementInstruction instruction, InvocationContext context)
        {
            string key;

            if (instruction.Key is VariableExpression variableExpression)
            {
                key = string.Concat(variableExpression.Reference.FullPath);
            }
            else
            {
                key = instruction.Key.Evaluate(context).ToString();
            }
            
            context.Interpreter.Context.Variables.Add(key, instruction.Expression.Evaluate(context));
            
            return true;
        }
    }
}