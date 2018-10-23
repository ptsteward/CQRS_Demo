using System;
using System.Threading.Tasks;

namespace CQRS_Demo.Framework
{
    public static class MessageBusExtensions
    {
        public static Task QueueCommandAsync(this IMessageBus bus, IAppCommand command)
            => bus.QueueCommandAsync(command, () => Task.CompletedTask, e => Task.CompletedTask);

        public static Task QueueCommandAsync(this IMessageBus bus, IAppCommand command, Func<Task> onCompleted)
            => bus.QueueCommandAsync(command, onCompleted, e => Task.CompletedTask);

        public static Task QueueCommandAsync(this IMessageBus bus, IAppCommand command, Func<Exception, Task> onError)
            => bus.QueueCommandAsync(command, () => Task.CompletedTask, onError);
    }
}
