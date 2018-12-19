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
        private string date, time;

        public NewTaskPage()
        {
            InitializeComponent();
            RemoveAndroidBackBtn();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            TaskName.Focus();
        }

        // Override the back navigation button to display an alert
        protected override bool OnBackButtonPressed()
        {
            DisplayConfirmationPrompt();
            return true;
        }

        // Remove the back button on Android since the alert will only display by using the device's hardware button
        private void RemoveAndroidBackBtn()
        {
            if (Device.RuntimePlatform == Device.Android)
                NavigationPage.SetHasBackButton(this, false);
        }

        private async void DisplayConfirmationPrompt()
        {
            // The alert only shows if any of the fields have been modified
            if (!string.IsNullOrWhiteSpace(TaskName.Text) || SetReminder.IsVisible)
            {
                bool choice = await DisplayAlert("Warning", "Your changes will be discarded.", "OK", "Cancel");

                if (choice)
                    await Navigation.PopAsync();
            }
            else
            {
                await Navigation.PopAsync();
            }
        }

        private async void SaveTask_Clicked(object sender, EventArgs e)
        {
            string tName = TaskName.Text;
            string tReminder = date + " at " + time;

            if (!string.IsNullOrWhiteSpace(tName))
            {
                if (SetReminder.IsVisible)
                {
                    var todoItem = new AddNewItem(tName, tReminder, GetFormattedDate());
                    await App.Database.SaveItemAsync(todoItem);
                }
                else if (!SetReminder.IsVisible)
                {
                    var todoItem = new AddNewItem(tName);
                    await App.Database.SaveItemAsync(todoItem);
                }
            }
            await Navigation.PopAsync();
        }

        private void ReminderTime_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            time = ReminderTime.Time.ToString("hh") + ":" + ReminderTime.Time.ToString("mm");
            DisplayConfirmationString();
        }

        private void ReminderDate_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            date = ReminderDate.Date.ToString("ddd, d MMM yyyy");
            DisplayConfirmationString();
        }

        private void DisplayConfirmationString()
        {
            ChosenDateTime.Text = "Reminder set for " + date + " at " + time;
        }

        private string GetFormattedDate()
        {
            // Formatted date will be used to determine in which tab(s) the item ought to be displayed
            return ReminderDate.Date.ToString("yyyy-MM-dd");
        }
    }
}