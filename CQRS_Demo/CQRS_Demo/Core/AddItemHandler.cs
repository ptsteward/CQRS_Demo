using CQRS_Demo.DataModel;
using CQRS_Demo.DataModel.Messages;
using CQRS_Demo.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Demo.Core
{
    public class AddItemHandler : IAppCommandHandler<AddItemMessage>
    {
        private readonly IStateStore<TodoAppState> stateStore;

        public AddItemHandler(IStateStore<TodoAppState> stateStore) => this.stateStore = stateStore ?? throw new ArgumentNullException(nameof(stateStore));

        public async Task HandleAsync(AddItemMessage command)
        {
            await stateStore.Reduce(state =>
            {
                var newItems = state.TodoItems.ToList();
                newItems.Add(new TodoItem()
                {
                    Description = "New Item"
                });
                state.TodoItems = newItems;
                return state;
            });
        }
    }
}
