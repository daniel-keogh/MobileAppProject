using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaskList
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Reset the 'resume' id, since we just want to re-start here
            //((App)Application.Current).ResumeAtTodoId = -1;
            ListView.ItemsSource = await App.Database.GetItemsAsync();
        }

        async private void CreateNewTask_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewTaskPage());
        }

        private void Checkbox_Tapped(object sender, EventArgs e)
        {
            ImageButton checkbox = (ImageButton)sender;

            var todoItem = (sender as ImageButton).CommandParameter as AddNewItem;

            if (todoItem.CheckboxSource == "checked.png")
            {
                checkbox.Source = "unchecked.png";
                App.Database.OnChecked(todoItem);
            }
            else
            {
                checkbox.Source = "checked.png";
                App.Database.OnChecked(todoItem);
            }
        }

        async private void ContextDelete_Clicked(object sender, EventArgs e)
        {
            var todoItem = (sender as MenuItem).CommandParameter as AddNewItem;
            await App.Database.DeleteItemAsync(todoItem);

            ListView.ItemsSource = await App.Database.GetItemsAsync(); // this refreshes the page
        }

        async private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as AddNewItem;
            if (item == null)
                return;
            await Navigation.PushAsync(new ItemDetailPage(item));

            // Manually deselect item.
            ListView.SelectedItem = null;
        }
    }
}
