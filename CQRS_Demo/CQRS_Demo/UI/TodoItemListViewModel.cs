using CQRS_Demo.DataModel;
using CQRS_Demo.DataModel.Messages;
using CQRS_Demo.Framework;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace CQRS_Demo.UI
{
    public class TodoItemListViewModel : ViewModelBase
    {
        private readonly IMessageBus messageBus;        

        private IEnumerable<TodoItem> todoItems;
        public IEnumerable<TodoItem> TodoItems
        {
            get => todoItems;
            private set { todoItems = value; OnPropertyChanged(); }
        }

        public ICommand AddItemCommand { get; }
        public ICommand DeleteDoneCommand { get; }

        public TodoItemListViewModel(IStateStore<TodoAppState> appState, IMessageBus messageBus)
        {
            appState.Subscribe(state => TodoItems = state.TodoItems);
            this.messageBus = messageBus ?? throw new ArgumentNullException(nameof(messageBus));
            AddItemCommand = new Command(OnAddItem);
            DeleteDoneCommand = new Command(OnDeleteDone);
        }

        public async void OnAddItem() => await messageBus.QueueCommandAsync(new AddItemMessage());

        public async void OnDeleteDone() => await messageBus.QueueCommandAsync(new DeleteDoneMessage());

    }
}
