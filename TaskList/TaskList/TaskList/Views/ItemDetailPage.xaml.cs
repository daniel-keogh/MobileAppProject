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
            ShowDateYesOrNo(item);
            ShowStatus(item);
        }

        private void ShowStatus(AddNewItem item)
        {
            if (item.IsComplete)
                StatusLabel.Text = "Complete";
            else
                StatusLabel.Text = "Uncomplete";
        }

        private void ShowDateYesOrNo(AddNewItem item)
        {
            if (item.Reminder != null)
                DueDate.IsVisible = true;
        }

        async private void ItemDelete_Clicked(object sender, EventArgs e)
        {
            var todoItem = (sender as MenuItem).CommandParameter as AddNewItem;

            await App.Database.DeleteItemAsync(todoItem);
            await Navigation.PopAsync();
        }

        async private void ItemComplete_Clicked(object sender, EventArgs e)
        {
            var todoItem = (sender as MenuItem).CommandParameter as AddNewItem;

            App.Database.OnChecked(todoItem);
            await Navigation.PopAsync();
        }
    }
}