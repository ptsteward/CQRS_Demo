using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CQRS_Demo.UI
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoItemListView : ContentPage
    {
        public TodoItemListView(TodoItemListViewModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}