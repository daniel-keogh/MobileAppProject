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
            ((App)Application.Current).ResumeAtTodoId = -1;
            ListView.ItemsSource = await App.Database.GetItemsAsync();
        }

        async private void CreateNewTask_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewTaskPage());
        }

        private void Checkbox_Tapped(object sender, EventArgs e)
        {
            ImageButton checkbox = (ImageButton)sender;
            FileImageSource imgSource = (FileImageSource)checkbox.Source;  // return the name of the image as a string

            if (imgSource == "checked.png")
            {
                checkbox.Source = "unchecked.png";
                //OnUnchecked();
            }
            else
            {
                checkbox.Source = "checked.png";
                //OnChecked();
            }
        }

        private void OnChecked()
        {
            throw new NotImplementedException();
        }

        private void OnUnchecked()
        {
            throw new NotImplementedException();
        }

        async void ContextDelete_Clicked(object sender, EventArgs e)
        {
            var todoItem = (sender as MenuItem).CommandParameter as AddNewItem;
            await App.Database.DeleteItemAsync(todoItem);

            ListView.ItemsSource = await App.Database.GetItemsAsync(); // this refreshes the page
        }
    }
}
