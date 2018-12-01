using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskList.Models;
using Xamarin.Forms;

namespace TaskList
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<AddNewItem> _items = new ObservableCollection<AddNewItem>();

        public MainPage()
        {
            InitializeComponent();
        }

        public MainPage(string tName, string tReminder)
        {
            InitializeComponent();

            ListView.ItemsSource = _items;

            CreateNew(tName, tReminder);
        }

        public MainPage(string tName)
        {
            InitializeComponent();

            ListView.ItemsSource = _items;

            CreateNew(tName);
        }

        // Methods to create a new list item
        private void CreateNew(string tName, string tReminder) // with reminder
        {
            _items.Add(new AddNewItem()
            {
                Title = tName,
                Reminder = tReminder,
                IsComplete = false
            });
        }
        private void CreateNew(string tName) // without reminder
        {
            _items.Add(new AddNewItem()
            {
                Title = tName,
                IsComplete = false
            });
        }

        async private void CreateNewTask_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewTaskPage());
        }

        private void ContextDelete_Clicked(object sender, EventArgs e)
        {
            var item = (sender as MenuItem).CommandParameter as AddNewItem;
            _items.Remove(item);
        }

        private void Checkbox_Tapped(object sender, EventArgs e)
        {
            Image checkbox = (Image)sender;
            FileImageSource imgSource = (FileImageSource)checkbox.Source;  // return the name of the image as a string

            if (imgSource == "checked.png")
            {
                checkbox.Source = "unchecked.png";
            }
            else
            {
                checkbox.Source = "checked.png";
            }
        }
    }
}
