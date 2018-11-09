using System;
using System.Collections.Generic;
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

        async private void CreateNewTask_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewTaskPage());
        }
    }
}
