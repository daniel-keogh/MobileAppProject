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
        private ObservableCollection<AddNewItem> _allItems;
        private ObservableCollection<AddNewItem> _todayItems;
        private ObservableCollection<AddNewItem> _thisWeekItems;

        public MainPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await LoadAllTab();
            await LoadTodayTab();
            await LoadThisWeekTab();
        }

        private async void AllTab_Appearing(object sender, EventArgs e)
        {
            await LoadAllTab();
        }

        private async void TodayTab_Appearing(object sender, EventArgs e)
        {
            await LoadTodayTab();
        }

        private async void ThisWeekTab_Appearing(object sender, EventArgs e)
        {
            await LoadThisWeekTab();
        }

        private async Task LoadAllTab()
        {
            if (SearchSL.IsVisible)
                HideSearchBar();

            // Populates ListAll with all the items in the database
            var allItems = await App.Database.GetItemsAsync();
            _allItems = new ObservableCollection<AddNewItem>(allItems);
            ListAll.ItemsSource = _allItems;
        }

        private async Task LoadTodayTab()
        {
            // Populates ListToday with items from the database whose reminder was set to today's date
            var todayItems = await App.Database.GetItemsTodayAsync();
            _todayItems = new ObservableCollection<AddNewItem>(todayItems);
            ListToday.ItemsSource = _todayItems;
        }

        private async Task LoadThisWeekTab()
        {
            // Populates ListThisWeek with items whose reminder was set to a date within the next 7 days
            var thisWeekItems = await App.Database.GetItemsThisWeekAsync();
            _thisWeekItems = new ObservableCollection<AddNewItem>(thisWeekItems);
            ListThisWeek.ItemsSource = _thisWeekItems;
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Filter the contents of the list to display only those whose title's match the search text
            var searchItems = await App.Database.GetItemAsync(e.NewTextValue);
            ObservableCollection<AddNewItem> _searchItems = new ObservableCollection<AddNewItem>(searchItems);
            ListAll.ItemsSource = _searchItems;
        }

        private void SearchIcon_Clicked(object sender, EventArgs e)
        {
            // Switch to AllTab and display the searchbox
            CurrentPage = AllTab;
            SearchSL.IsVisible = true;
            SearchList.Text = null;
            SearchList.Focus();
        }

        private void CloseSearchButton_Clicked(object sender, EventArgs e)
        {
            HideSearchBar();
        }

        private void HideSearchBar()
        {
            // Set search text to null so the list will display all items
            SearchList.Text = null;
            SearchSL.IsVisible = false;
        }

        protected override bool OnBackButtonPressed()
        {
            // Hide the searchbar if it is visible
            if (SearchSL.IsVisible) 
            {
                HideSearchBar();
                return true;
            }
            else // If it's not visible then close the app
            {
                return false;
            }        
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

            _allItems.Remove(todoItem);
            _todayItems.Remove(todoItem);
            _thisWeekItems.Remove(todoItem);

            await App.Database.DeleteItemAsync(todoItem);
        }

        private async void ListView_DeleteAll(object sender, EventArgs e)
        {
            bool choice = await DisplayAlert("Confirm", "Are you sure you want to delete everything?\n\nThis action is irreversible.\n", "Yes", "No");

            if (choice)
            {
                await App.Database.DeleteAllItemsAsync();
                ListView_Refreshing(sender, e);
            }
        }

        private async void ListView_DeleteDone(object sender, EventArgs e)
        {
            bool choice = await DisplayAlert("Confirm", "Delete all completed items?", "Yes", "No");

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

            // Manually deselect item
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
            await LoadAllTab();
            await LoadTodayTab();
            await LoadThisWeekTab();

            ListAll.EndRefresh();
            ListToday.EndRefresh();
            ListThisWeek.EndRefresh();
        }
    }
}
