using System.Reflection;

using BakedEnv;

namespace SourceGeneration.Tests.ErrorSourceGen;

public class BaseTests
{
    [TestCaseSource(nameof(GetErrors))]
    public void TestError(BakedError testError)
    {
        Assert.Multiple(() =>
        {
            Assert.That(string.IsNullOrEmpty(testError.Id), Is.False, "An error ID cannot be null or empty.");
            Assert.That(string.IsNullOrEmpty(testError.Name), Is.False, "An error name cannot be null or empty.");
        });
    }

    private static BakedError[] GetErrors()
    {
        var errors = new List<BakedError>();
        var generatedMethods = typeof(BakedError)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.ReturnType == typeof(BakedError));
        
        foreach (var method in generatedMethods)
        {
            var errorParams = method.GetParameters();
            var testParams = new List<object?>();

            foreach (var p in errorParams)
            {
                if (p.ParameterType.IsValueType)
                {
                    testParams.Add(Activator.CreateInstance(p.ParameterType));
                }
                else
                {
                    testParams.Add(null);
                }
            }

            var result = method.Invoke(null, testParams.ToArray());
            errors.Add((BakedError)result!);
        }

        return errors.ToArray();
    }
}