using BakedEnv.ExternalApi;

namespace BakedEnv;

public class BakedEnvironment
{
    public List<ExternalMethod> GlobalMethods { get; }
    public List<BakedVariable> GlobalVariables { get; }
    
    public BakeType DefaultBakeType { get; set; }
    
    public List<ApiStructure> ApiStructures { get; }

    public BakedEnvironment()
    {
        GlobalMethods = new List<ExternalMethod>();
        GlobalVariables = new List<BakedVariable>();
        ApiStructures = new List<ApiStructure>();
        DefaultBakeType = BakeType.Script;
    }
    
    public IEnumerable<InterpreterInstruction> Invoke(IBakedSource source)
    {
        throw new NotImplementedException();
    }
}