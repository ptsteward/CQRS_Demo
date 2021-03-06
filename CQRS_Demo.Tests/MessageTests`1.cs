﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tests
{
    public abstract class MessageTests<T> where T : class
    {
        protected virtual IEnumerable<DefaultArgument> DefaultArguments { get; } = Enumerable.Empty<DefaultArgument>();

        [Test]
        public void ArgumentsAreNullChecked()
        {
            var types = DefaultArguments.Select(arg => arg.Argument.GetType()).ToArray();
            var constructor = typeof(T).GetConstructor(types);
            var arguments = DefaultArguments.ToList();
            arguments.ForEach(arg =>
            {
                var args = arguments.Select(defaultArg =>
                {
                    if (defaultArg == arg && !defaultArg.SkipNullCheck)
                        return null;
                    return defaultArg.Argument;
                }).ToArray();
                try
                {
                    if (constructor == null) throw new InvalidOperationException("No constructor found for given arguments.");
                    constructor.Invoke(args);
                    if (!arg.SkipNullCheck)
                        Assert.Fail($"No exception was thrown. ArgumentNullException expected for {arg.PropertyName} of constructor.");
                }
                catch (TargetInvocationException ex)
                {
                    Assert.AreEqual(typeof(ArgumentNullException), ex.InnerException.GetType(),
                        $"Exception of type {ex.InnerException.GetType()} was thrown. ArgumentNullException expected for {arg.PropertyName} of constructor");
                }
            });
        }

        [Test]
        public void PropertiesAreInitialized()
        {
            var types = DefaultArguments.Select(arg => arg.Argument.GetType()).ToArray();
            var constructor = typeof(T).GetConstructor(types);
            if (constructor == null) throw new InvalidOperationException("No constructor found for given arguments.");
            var instance = constructor.Invoke(DefaultArguments.Select(arg => arg.Argument).ToArray());
            DefaultArguments.ToList().ForEach(arg =>
            {
                var property = typeof(T).GetProperty(arg.PropertyName);
                Assert.AreEqual(arg.Argument, property.GetValue(instance));
            });
        }

        [Test]
        public void MessageHasDefaultProperty()
        {
            var property = typeof(T).GetProperty("Default", BindingFlags.Public | BindingFlags.Static);
            Assert.IsNotNull(property, "Static Default property not found.");
            Assert.IsFalse(property.CanWrite, "Default property cannot be writeable.");
            var value1 = property.GetValue(null);
            var value2 = property.GetValue(null);
            Assert.IsNotNull(value1, "Default property returns null.");
            Assert.IsNotNull(value2, "Default property returns null.");
            Assert.AreEqual(value1, value2, "Default property returns different values per call.");
        }
    }
}
