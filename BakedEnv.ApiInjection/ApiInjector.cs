using BakedEnv.Environment;
using BakedEnv.Objects;
using BakedEnv.Variables;

namespace BakedEnv.ApiInjection;

public class ApiInjector
{
    private Dictionary<string, ApiObject> ApiObjects { get; }

    public const string ApiMethodName = "api";

    public void AddApiMethod(BakedEnvironment environment)
    {
        BakedObject RetrieveApi(string name)
        {
            if (!ApiObjects.TryGetValue(name, out var value)) 
                return new BakedNull();

            return value;
        }

        var method = new DelegateObject((Func<string, BakedObject>)RetrieveApi);
        var variable = new BakedVariable(ApiMethodName, method) { IsReadOnly = true };

        environment.GlobalVariables.Add(variable);
    }
    
    public void Inject(string name, ApiObject o)
    {
        ApiObjects[name] = o;
    }
}