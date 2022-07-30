using System.Runtime.Serialization;

namespace ErrorSourceGen;

public class ErrorsContract : Dictionary<string, ErrorGroupContract>
{
    
}

public class ErrorGroupContract : Dictionary<string, ErrorContract>
{
    
}

[DataContract]
public class ErrorContract
{
    [DataMember(Name = "name")] public string Name { get; set; }
    [DataMember(Name = "short")] public string ShortDescription { get; set; }
    [DataMember(Name = "long")] public string LongDescription { get; set; }
}