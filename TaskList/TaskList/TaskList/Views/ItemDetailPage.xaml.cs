using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage(AddNewItem item)
        {
            InitializeComponent();
            BindingContext = item;
            ShowStatus(item);
            ShowDate(item);
        }

        private void ShowStatus(AddNewItem item)
        {
            if (item.IsComplete)
                StatusLabel.Text = "Complete";
            else
                StatusLabel.Text = "Uncomplete";
        }

        private void ShowDate(AddNewItem item)
        {
            // only show the DueDate if a Reminder was set
            if (item.Reminder != null)
                DueDate.IsVisible = true;
        }

        private async void ItemDelete_Clicked(object sender, EventArgs e)
        {
            var todoItem = (sender as MenuItem).CommandParameter as AddNewItem;

            await App.Database.DeleteItemAsync(todoItem);
            await Navigation.PopAsync();
        }

        private async void ItemComplete_Clicked(object sender, EventArgs e)
        {
            var todoItem = (sender as MenuItem).CommandParameter as AddNewItem;

            App.Database.OnChecked(todoItem);
            await Navigation.PopAsync();
        }
    }
}