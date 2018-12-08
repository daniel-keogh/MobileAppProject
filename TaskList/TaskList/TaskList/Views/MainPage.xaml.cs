using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaskList
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            ListViewAll.ItemsSource = await App.Database.GetItemsAsync();
            ListViewComp.ItemsSource = await App.Database.GetItemsDoneAsync();
            ListViewUnComp.ItemsSource = await App.Database.GetItemsNotDoneAsync();
        }


        private async void AllTab_Appearing(object sender, EventArgs e)
        {
            ListViewAll.ItemsSource = await App.Database.GetItemsAsync();
        }

        private async void CompTab_Appearing(object sender, EventArgs e)
        {
            ListViewComp.ItemsSource = await App.Database.GetItemsDoneAsync();
        }

        private async void UnCompTab_Appearing(object sender, EventArgs e)
        {
            ListViewUnComp.ItemsSource = await App.Database.GetItemsNotDoneAsync();
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
            }
            else
            {
                checkbox.Source = "checked.png";
            }

            App.Database.OnChecked(todoItem);
        }

        async private void ContextDelete_Clicked(object sender, EventArgs e)
        {
            var todoItem = (sender as MenuItem).CommandParameter as AddNewItem;
            await App.Database.DeleteItemAsync(todoItem);

            OnAppearing(); // refresh the page
        }

        async private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as AddNewItem;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(item));

            // Manually deselect item.
            if (sender.Equals(ListViewComp))
            {
                ListViewComp.SelectedItem = null;
            }
            else if (sender.Equals(ListViewUnComp))
            {
                ListViewUnComp.SelectedItem = null;
            }
            else
            {
                ListViewAll.SelectedItem = null;
            }
        }

        private async void ListView_Refreshing(object sender, EventArgs e)
        {
            if (sender.Equals(ListViewComp))
            {
                ListViewComp.ItemsSource = await App.Database.GetItemsDoneAsync();
                ListViewComp.EndRefresh();
            }
            else if (sender.Equals(ListViewUnComp))
            {
                ListViewUnComp.ItemsSource = await App.Database.GetItemsNotDoneAsync();
                ListViewUnComp.EndRefresh();
            }
            else if (sender.Equals(ListViewAll))
            {
                ListViewAll.ItemsSource = await App.Database.GetItemsAsync();
                ListViewAll.EndRefresh();
            }
            else // if the toolbar button is clicked
            {
                OnAppearing();
                ListViewAll.EndRefresh();
                ListViewComp.EndRefresh();
                ListViewUnComp.EndRefresh();
            }
        }
    }
}
