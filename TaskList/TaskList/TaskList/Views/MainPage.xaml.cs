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
        private ObservableCollection<AddNewItem> _searchItems;

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
            HideSearchBar();

            var allItems = await App.Database.GetItemsAsync();
            _allItems = new ObservableCollection<AddNewItem>(allItems);
            ListAll.ItemsSource = _allItems;
        }

        private async Task LoadTodayTab()
        {
            var todayItems = await App.Database.GetItemsTodayAsync();
            _todayItems = new ObservableCollection<AddNewItem>(todayItems);
            ListToday.ItemsSource = _todayItems;
        }

        private async Task LoadThisWeekTab()
        {
            var thisWeekItems = await App.Database.GetItemsThisWeekAsync();
            _thisWeekItems = new ObservableCollection<AddNewItem>(thisWeekItems);
            ListThisWeek.ItemsSource = _thisWeekItems;
        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchItems = await App.Database.GetItemAsync(e.NewTextValue);
            _searchItems = new ObservableCollection<AddNewItem>(searchItems);
            ListAll.ItemsSource = _searchItems;
        }

        private void SearchIcon_Clicked(object sender, EventArgs e)
        {
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
            if (SearchSL.IsVisible)
            {
                SearchList.Text = null;
                SearchSL.IsVisible = false;
            }
        }

        // Hide the searchbar if it is visible, if not then close the app
        protected override bool OnBackButtonPressed()
        {
            if (SearchSL.IsVisible)
            {
                HideSearchBar();
                return true;
            }
            else
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
            bool choice = await DisplayAlert("Warning", "Are you sure you want to delete everything?\n\nThis action is irreversible.\n", "Yes", "No");

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
            if (sender.Equals(_todayItems))
            {
                await LoadTodayTab();
                ListToday.EndRefresh();
            }
            else if (sender.Equals(_thisWeekItems))
            {
                await LoadThisWeekTab();
                ListThisWeek.EndRefresh();
            }
            else if (sender.Equals(_allItems))
            {
                await LoadAllTab();
                ListAll.EndRefresh();
            }
            else // if the toolbar button is clicked
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
}
