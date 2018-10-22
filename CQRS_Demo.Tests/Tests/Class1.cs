using CQRS_Demo.Core;
using CQRS_Demo.DataModel;
using CQRS_Demo.DataModel.Messages;
using CQRS_Demo.Framework;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Tests
{
    [TestFixture]
    public class SampleQueryMessageTests : AppQueryTests<SampleQueryMessage, int>
    {
        protected override IEnumerable<DefaultArgument> DefaultArguments => new[]
        {
            new DefaultArgument(nameof(SampleQueryMessage.Arg1), "abc"),
            new DefaultArgument(nameof(SampleQueryMessage.Arg2), "def"),
            new DefaultArgument(nameof(SampleQueryMessage.Arg3), 23)
        };        
    }

    [TestFixture]
    public class AddItemHandlerTests : DependencyInjectedTests<AddItemHandler>
    {
        private IAppCommandHandler<AddItemMessage> addItemHandler;
        private Mock<IStateStore<TodoAppState>> mockStateStore;

        [SetUp]
        public void SetUp()
        {
            mockStateStore = new Mock<IStateStore<TodoAppState>>();
            addItemHandler = new AddItemHandler(mockStateStore.Object);
        }

        [Test]
        public async Task NewItemIsAddedToTodoList()
        {
            var state = new TodoAppState();
            mockStateStore.Setup(store => store.Reduce(It.IsAny<Func<TodoAppState, TodoAppState>>()))
                .Callback<Func<TodoAppState, TodoAppState>>(func => func(state))
                .Returns(Task.CompletedTask);

            await addItemHandler.HandleAsync(AddItemMessage.Default);

            Assert.IsTrue(state.TodoItems.Any());
        } 
    }
}
