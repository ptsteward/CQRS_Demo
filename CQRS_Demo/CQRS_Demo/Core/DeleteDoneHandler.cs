using CQRS_Demo.DataModel;
using CQRS_Demo.DataModel.Messages;
using CQRS_Demo.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CQRS_Demo.Core
{
    public class DeleteDoneHandler : IAppCommandHandler<DeleteDoneMessage>
    {
        private readonly IStateStore<TodoAppState> stateStore;

        public DeleteDoneHandler(IStateStore<TodoAppState> stateStore) => this.stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));

        public async Task HandleAsync(DeleteDoneMessage command)
        {
            await stateStore.Reduce(state =>
            {
                var newItems = state.TodoItems.Where(item => !item.Done).ToList();
                state.TodoItems = newItems;
                return state;
            });
        }
    }
}
