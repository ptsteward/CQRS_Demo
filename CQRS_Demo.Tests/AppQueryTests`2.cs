using CQRS_Demo.Framework;
using NUnit.Framework;
using System;
using System.Linq;

namespace Tests
{
    public abstract class AppQueryTests<TQuery, TReply> : MessageTests<TQuery> where TQuery : class, IAppQuery<TReply> { }
}
