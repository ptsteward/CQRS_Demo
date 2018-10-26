using CQRS_Demo.Framework;

namespace Tests
{
    public abstract class AppQueryTests<TQuery, TReply> : MessageTests<TQuery> where TQuery : class, IAppQuery<TReply> { }
}
