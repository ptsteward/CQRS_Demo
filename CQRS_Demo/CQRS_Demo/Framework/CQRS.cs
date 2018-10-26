using System.Threading.Tasks;

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
}
