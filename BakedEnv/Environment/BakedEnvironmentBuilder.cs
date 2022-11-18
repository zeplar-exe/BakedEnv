using BakedEnv.ControlStatements;
using BakedEnv.Interpreter.ProcessorStatementHandling;
using BakedEnv.Objects;
using BakedEnv.Variables;

namespace BakedEnv.Environment;

public class BakedEnvironmentBuilder
{
    private BakedEnvironment Environment { get; }
    
    public BakedEnvironmentBuilder()
    {
        Environment = new BakedEnvironment();
    }
    
    public BakedEnvironmentBuilder(BakedEnvironment initial)
    {
        Environment = initial;
    }
    
    public EnvironmentVariableBuilder CreateVariable()
    {
        return new EnvironmentVariableBuilder(this);
    }
    
    /// <summary>
    /// Add a variable by key and value to the global variables.
    /// </summary>
    /// <param name="key">The variable name.</param>
    /// <param name="variable">The variable's value.</param>
    /// <returns></returns>
    public BakedEnvironmentBuilder WithVariable(IBakedVariable variable)
    {
        Environment.GlobalVariables.Add(variable);

        return this;
    }

    public BakedEnvironmentBuilder WithVariable(string name, BakedObject value, VariableFlags flags = 0)
    {
        Environment.GlobalVariables.Add(new BakedVariable(name, value, flags));

        return this;
    }

    public BakedEnvironmentBuilder WithReferenceOrder(VariableReferenceOrder order)
    {
        Environment.VariableReferenceOrder = order;

        return this;
    }
    
    /// <summary>
    /// Add an array of <see cref="IProcessorStatementHandler"/> to the interpreter.
    /// </summary>
    /// <param name="handlers">Handlers to add.</param>
    public BakedEnvironmentBuilder WithStatementHandlers(params IProcessorStatementHandler[] handlers)
    {
        Environment.ProcessorStatementHandlers.AddRange(handlers);

        return this;
    }
    
    public BakedEnvironmentBuilder WithStatementHandlers(IProcessorStatementHandler handler)
    {
        Environment.ProcessorStatementHandlers.Add(handler);

        return this;
    }

    public BakedEnvironmentBuilder WithControlStatements(params ControlStatementDefinition[] definitions)
    {
        Environment.ControlStatements.AddRange(definitions);

        return this;
    }
    
    public BakedEnvironmentBuilder WithControlStatement(ControlStatementDefinition definition)
    {
        Environment.ControlStatements.Add(definition);

        return this;
    }

    public BakedEnvironmentBuilder WithOutputWriter(TextWriter? writer)
    {
        Environment.OutputWriter = writer;

        return this;
    }

    public BakedEnvironment Build()
    {
        return Environment;
    }
}