using System.Text.Json;
using System.Text.Json.Serialization;

namespace ErrorSourceGen;

#nullable enable
public class ErrorsContract : ExtensionDictionary<ErrorGroupContract>
{
    
}

public class ErrorGroupContract : ExtensionDictionary<ErrorContract>
{
    
}

public class ErrorContract
{
    [JsonPropertyName("name")] public string Name { get; set; }
    [JsonPropertyName("short")] public string ShortDescription { get; set; }
    [JsonPropertyName("long")] public string LongDescription { get; set; }
}

public abstract class ExtensionDictionary<TValue>
{
    [JsonExtensionData] public Dictionary<string, JsonElement> Elements { get; set; } = new();

    public Dictionary<string, TValue> Properties
    {
        get
        {
            var dict = new Dictionary<string, TValue>();

            foreach (var pair in Elements)
            {
                try
                {
                    var value = JsonSerializer.Deserialize<TValue>(pair.Value.GetRawText());

                    if (value == null)
                        continue;
                    
                    dict[pair.Key] = value;
                }
                catch (JsonException) {}
            }

            return dict;
        }
    }
}