using System;
using System.Threading.Tasks;

namespace CQRS_Demo.Framework
{
    public interface IMessageBus
    {
        Task QueueCommandAsync(IAppCommand command, Func<Task> onCompleted, Func<Exception, Task> onError);
    }
}
