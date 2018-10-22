using CQRS_Demo.Framework;
using NUnit.Framework;
using System;
using System.Linq;

namespace Tests
{
    public abstract class AppCommandTests<TCommand> : MessageTests<TCommand> where TCommand : class, IAppCommand { }
}
