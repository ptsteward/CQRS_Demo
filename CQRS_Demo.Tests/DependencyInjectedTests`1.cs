using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Reflection;

namespace Tests
{
    [TestFixture]
    public abstract class DependencyInjectedTests<T> where T : class
    {
        [Test]
        private void DependenciesAreNullChecked()
        {
            typeof(T).GetConstructors().ToList().ForEach(constructor =>
            {
                var parameters = constructor.GetParameters();
                var mocks = parameters.Select(parameter =>
                {
                    var mockType = typeof(Mock<>).MakeGenericType(new[] { parameter.ParameterType });
                    return (Mock)Activator.CreateInstance(mockType);
                }).ToArray();

                parameters.ToList().ForEach(parameter =>
                {
                    var mocksWithNull = mocks.Select(mock =>
                    {
                        if (mock.GetType().GetGenericArguments().First() == parameter.ParameterType)
                            return null;
                        return mock.Object;
                    }).ToArray();
                    try
                    {
                        constructor.Invoke(mocksWithNull);
                        Assert.Fail($"No exception was thrown. ArgumentNullException expected for {parameter.Name}.");
                    }
                    catch (TargetInvocationException ex)
                    {
                        Assert.AreEqual(typeof(ArgumentNullException), ex.InnerException.GetType(),
                            $"Exception of type {ex.InnerException.GetType()} was thrown. ArgumentNullException expected for {parameter.Name}.");
                    }
                });
            });
        }
    }
}
