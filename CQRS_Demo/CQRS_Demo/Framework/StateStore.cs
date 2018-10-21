using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS_Demo.Framework
{
    public interface IStateStore<TState> : IObservable<TState> where TState : class
    {
        Task Reduce(Func<TState, TState> reducer);
    }
}
