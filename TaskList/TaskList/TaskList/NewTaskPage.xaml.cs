using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTaskPage : ContentPage
    {
        string date, time;

        public NewTaskPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            // base.OnAppearing();
            RemoveAndroidBackBtn();
            TaskName.Focus();
        }

        private void RemindYesOrNo_Toggled(object sender, ToggledEventArgs e)
        {

        }

        private void ReminderTime_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string hours = ReminderTime.Time.ToString("hh");
            string minutes = ReminderTime.Time.ToString("mm");
            time = hours + ":" + minutes;
            DisplayConfirmationString();
        }

        private void ReminderDate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            date = ReminderDate.Date.ToString("MMM d, yyyy");
            DisplayConfirmationString();
        }

        void DisplayConfirmationString()
        {
            ChosenDateTime.Text = "Reminder set for " + date + " at " + time + ".";
        }

        async private void SaveTask_Clicked(object sender, EventArgs e)
        {
            string tName = TaskName.Text;
            string tReminder = date + " at " + time;

            if(!(string.IsNullOrWhiteSpace(TaskName.Text)))
            {
                if (SetReminder.IsVisible)
                {
                    await Navigation.PushAsync(new MainPage(tName, tReminder));
                }
                else if (!SetReminder.IsVisible)
                {
                    await Navigation.PushAsync(new MainPage(tName));
                }
            }
            else
            {
                await Navigation.PopAsync();
            }
        }

        // override the back navigation button to display an alert
        protected override bool OnBackButtonPressed()
        {
            DisplayConfirmationPrompt();
            return true;
        }

        // Because the alert will only display by using android's hardware button
        private void RemoveAndroidBackBtn()
        {
            if (Device.RuntimePlatform == Device.Android)
                NavigationPage.SetHasBackButton(this, false);
        }

        async void DisplayConfirmationPrompt()
        {
            // the alert only shows if any of the fields have been modified
            if (SetReminder.IsVisible || !string.IsNullOrWhiteSpace(TaskName.Text)) 
            {
                bool choice = await DisplayAlert("Warning", "Changes will be discarded.", "OK", "Cancel");

                if (choice)
                    await Navigation.PopAsync();
            }
            else
            {
                await Navigation.PopAsync();
            }
        }
    }
}