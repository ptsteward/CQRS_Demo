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
    }

    public class MessageBus : IMessageBus
    {
        public MessageBus(Container container)
        {
            this.container = container ?? throw new ArgumentNullException();
            if (Processor == null)
                Processor = new ActionBlock<IAppCommand>(ProcessCommandAsync, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount });
        }

        public Task QueueCommandAsync(IAppCommand command) => Processor.SendAsync(command);

        public async Task ProcessCommandAsync(IAppCommand command)
        {
            try
            {
                var handlerType = typeof(IAppCommandHandler<>).MakeGenericType(command.GetType());
                var handler = container.GetInstance(handlerType);
                var handleMethod = handler.GetType().GetMethod(nameof(IAppCommandHandler<IAppCommand>.HandleAsync));
                await (Task)handleMethod.Invoke(handler, new[] { command });
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private readonly Container container;

        private static ITargetBlock<IAppCommand> Processor { get; set; }
    }
}
