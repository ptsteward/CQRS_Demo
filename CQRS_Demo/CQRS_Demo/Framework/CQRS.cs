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
}
