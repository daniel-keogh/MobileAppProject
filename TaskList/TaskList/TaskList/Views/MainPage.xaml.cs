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

            ListAll.ItemsSource = await App.Database.GetItemsAsync();
            ListToday.ItemsSource = await App.Database.GetItemsTodayAsync();
            ListThisWeek.ItemsSource = await App.Database.GetItemsThisWeekAsync();
        }


        private async void AllTab_Appearing(object sender, EventArgs e)
        {
            ListAll.ItemsSource = await App.Database.GetItemsAsync();
        }

        private async void TodayTab_Appearing(object sender, EventArgs e)
        {
            ListToday.ItemsSource = await App.Database.GetItemsTodayAsync();
        }

        private async void ThisWeekTab_Appearing(object sender, EventArgs e)
        {
            ListThisWeek.ItemsSource = await App.Database.GetItemsThisWeekAsync();
        }


        private async void CreateNewTask_Clicked(object sender, EventArgs e)
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

        private async void ContextDelete_Clicked(object sender, EventArgs e)
        {
            var todoItem = (sender as MenuItem).CommandParameter as AddNewItem;
            await App.Database.DeleteItemAsync(todoItem);

            ListView_Refreshing(sender, e); // refresh the page
        }

        private async void ListView_DeleteAll(object sender, EventArgs e)
        {
            bool choice = await DisplayAlert("Warning", "Are you sure you want to delete everything?\n\nThis action is irreversible.", "Yes", "No");

            if (choice)
            {
                await App.Database.DeleteAllItemsAsync();
                ListView_Refreshing(sender, e);
            }
        }

        private async void ListView_DeleteDone(object sender, EventArgs e)
        {
            bool choice = await DisplayAlert("Are you sure?", "Delete all completed items?", "Yes", "No");

            if (choice)
            {
                await App.Database.DeleteDoneItemsAsync();
                ListView_Refreshing(sender, e);
            }
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as AddNewItem;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(item));

            // Manually deselect item.
            if (sender.Equals(ListToday))
            {
                ListToday.SelectedItem = null;
            }
            else if (sender.Equals(ListThisWeek))
            {
                ListThisWeek.SelectedItem = null;
            }
            else
            {
                ListAll.SelectedItem = null;
            }
        }

        private async void ListView_Refreshing(object sender, EventArgs e)
        {
            if (sender.Equals(ListToday))
            {
                ListToday.ItemsSource = await App.Database.GetItemsTodayAsync();
                ListToday.EndRefresh();
            }
            else if (sender.Equals(ListThisWeek))
            {
                ListThisWeek.ItemsSource = await App.Database.GetItemsThisWeekAsync();
                ListThisWeek.EndRefresh();
            }
            else if (sender.Equals(ListAll))
            {
                ListAll.ItemsSource = await App.Database.GetItemsAsync();
                ListAll.EndRefresh();
            }
            else // if the toolbar button is clicked
            {
                OnAppearing();
                ListAll.EndRefresh();
                ListToday.EndRefresh();
                ListThisWeek.EndRefresh();
            }
        }
    }
}
