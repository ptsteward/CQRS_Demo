using Microsoft.AppCenter.Crashes;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace CQRS_Demo.Framework
{
    public interface IAppCommand { }

    public interface IAppQuery<TResult> { }

    public interface IAppCommandHandler<TCommand> where TCommand : IAppCommand
    {
        Task HandleAsync(TCommand command);
    }

    public interface IAppQueryHandler<TQuery, TResult> where TQuery : IAppQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }

    public interface IMessageBus
    {
        Task QueueCommandAsync(IAppCommand command);
        Task QueueCommandAsync(IAppCommand command, Func<Task> onCompleted);
        Task QueueCommandAsync(IAppCommand command, Func<Exception, Task> onError);
        Task QueueCommandAsync(IAppCommand command, Func<Task> onCompleted, Func<Exception, Task> onError);
    }

    public class MessageBus : IMessageBus
    {
        private readonly Container container;
        private static ITargetBlock<Message> Processor { get; set; }

        public MessageBus(Container container)
        {
            this.container = container ?? throw new ArgumentNullException();
            if (Processor == null)
                Processor = new ActionBlock<Message>(ProcessCommandAsync, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount });
        }

        public Task QueueCommandAsync(IAppCommand command) => QueueCommandAsync(command, () => Task.CompletedTask, ex => Task.CompletedTask);
        public Task QueueCommandAsync(IAppCommand command, Func<Task> onCompleted) => QueueCommandAsync(command, onCompleted, ex => Task.CompletedTask);
        public Task QueueCommandAsync(IAppCommand command, Func<Exception, Task> onError) => QueueCommandAsync(command, () => Task.CompletedTask, onError);
        public Task QueueCommandAsync(IAppCommand command, Func<Task> onCompleted, Func<Exception, Task> onError) => Processor.SendAsync(new Message(command, onCompleted, onError));

        private async Task ProcessCommandAsync(Message message)
        {
            try
            {
                var handlerType = typeof(IAppCommandHandler<>).MakeGenericType(message.GetType());
                var handler = container.GetInstance(handlerType);
                var handleMethod = handler.GetType().GetMethod(nameof(IAppCommandHandler<IAppCommand>.HandleAsync));
                await (Task)handleMethod.Invoke(handler, new[] { message.Command });
                await message.OnCompleted();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                await message.OnError(ex);
            }
        }                

        private class Message
        {
            public Message(IAppCommand command, Func<Task> onCompleted, Func<Exception, Task> onError)
            {
                Command = command ?? throw new ArgumentNullException(nameof(command));
                OnCompleted = onCompleted ?? throw new ArgumentNullException(nameof(onCompleted));
                OnError = onError ?? throw new ArgumentNullException(nameof(onError));
            }

            public IAppCommand Command { get; }
            public Func<Task> OnCompleted { get; }
            public Func<Exception, Task> OnError { get; }
        }
    }
}
