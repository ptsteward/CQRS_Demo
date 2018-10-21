using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS_Demo.Framework
{
    public class StateStore<TState> : IStateStore<TState> where TState : class
    {
        private static readonly ReplaySubject<TState> stateSubject = new ReplaySubject<TState>(1);
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private TState currentState;

        public StateStore(TState initialState)
        {
            currentState = initialState ?? throw new ArgumentNullException(nameof(initialState));
            stateSubject.OnNext(currentState);
        }

        public IDisposable Subscribe(IObserver<TState> observer) => stateSubject.Subscribe(observer);

        public async Task Reduce(Func<TState, TState> reducer)
        {
            try
            {
                await semaphore.WaitAsync();
                currentState = reducer(currentState);
                stateSubject.OnNext(currentState);
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
