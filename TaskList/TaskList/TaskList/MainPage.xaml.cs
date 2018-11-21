using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskList.Models;
using Xamarin.Forms;

namespace TaskList
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // temp
            ListView.ItemsSource = new List<AddNewItem>
            {
                new AddNewItem { Id = 1, Title = "Do Stuff", Reminder = "Today", IsComplete = false}
            };
        }

        async private void CreateNewTask_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewTaskPage());
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void ContextDelete_Clicked(object sender, EventArgs e)
        {

        }
    }
}
