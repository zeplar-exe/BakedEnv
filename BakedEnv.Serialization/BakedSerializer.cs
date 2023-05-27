using System.Reflection;

using BakedEnv.Environment.Library;
using BakedEnv.Objects;
using BakedEnv.Objects.Conversion;
using BakedEnv.Serialization.Attributes;

namespace BakedEnv.Serialization;

public class BakedSerializer
{
    public BakedObject Serialize(object? o, IConversionTable conversion)
    {
        if (o == null)
            return new BakedNull();
        
        var bakedObject = new NameAccessBakedTable();
        var objectType = o.GetType();

        foreach (var property in objectType.GetProperties())
        {
            if (property.GetCustomAttribute<BakedSerializerIgnoreAttribute>() != null)
                continue;
            
            var propertyAttribute = property.GetCustomAttribute<BakedPropertyAttribute>();

            var keyString = new BakedString(propertyAttribute?.Name ?? property.Name);
            var propertyValue = property.GetValue(o);

            if (property.PropertyType.IsPrimitive)
            {
                if (conversion.TryToBakedObject(propertyValue, out var value))
                    bakedObject[keyString] = value;
                else
                    bakedObject[keyString] = new BakedNull();
            }
            else
            {
                bakedObject[keyString] = Serialize(propertyValue, conversion);
            }
        }
        
        return bakedObject;
    }

    public T Deserialize<T>(BakedObject bakedObject, IConversionTable conversion)
    {
        return (T)Deserialize(typeof(T), bakedObject, conversion);
    }

    public object Deserialize(Type type, BakedObject bakedObject, IConversionTable conversion)
    {
        var result = Activator.CreateInstance(type)!;

        Populate(result, bakedObject, conversion);

        return result;
    }
    
    public void Populate(object o, BakedObject bakedObject, IConversionTable conversion)
    {
        var objectType = o.GetType();
        
        foreach (var property in objectType.GetProperties())
        {
            if (property.GetCustomAttribute<BakedSerializerIgnoreAttribute>() != null)
                continue;
            
            var propertyAttribute = property.GetCustomAttribute<BakedPropertyAttribute>();
            var propertyName = propertyAttribute?.Name ?? property.Name;
            
            if (bakedObject.TryGetContainedObject(propertyName, out var contained))
            {
                if (conversion.TryToObject(contained, property.PropertyType, out var propertyValue))
                    property.SetValue(o, propertyValue);
            }
        }
    }
}