using System.Collections.Generic;
using BakedEnv.Extensions;
using NUnit.Framework;

namespace BakedEnv.GeneralTests.Extensions;

[TestFixture]
public class ObjectExt
{
    [Test]
    public void TestGetOrAddType()
    {
        var collection = new List<ClassA>();

        collection.GetOrAddByType(() => new ClassA());
        collection.GetOrAddByType(() => new ClassB());
        collection.GetOrAddByType(() => new ClassB());
        
        Assert.True(collection.Count == 2);
    }
    
    private class ClassA { }
    private class ClassB : ClassA { }
}