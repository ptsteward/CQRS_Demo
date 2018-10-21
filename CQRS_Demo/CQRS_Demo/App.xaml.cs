using CQRS_Demo.DataModel;
using CQRS_Demo.Framework;
using CQRS_Demo.UI;
using SimpleInjector;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CQRS_Demo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var container = new Container();
            container.RegisterInstance(new TodoAppState());
            container.Register(typeof(IStateStore<>), typeof(StateStore<>));
            container.Register<IMessageBus, MessageBus>();
            container.Register(typeof(IAppCommandHandler<>), typeof(App).Assembly);
            container.Register(typeof(IAppQueryHandler<,>), typeof(App).Assembly);
            var todoView = container.GetInstance<TodoItemListView>();
            MainPage = todoView;
        }        
    }
}
