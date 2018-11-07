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

        private void RemindYesOrNo_Toggled(object sender, ToggledEventArgs e)
        {
            if (RemindYesOrNo.IsToggled)
                SetReminder.IsVisible = true;
            else
                SetReminder.IsVisible = false;
        }

        void DisplayConfirmationString()
        {
            ChosenDateTime.Text = "Reminder set for " + date + " at " + time;
        }

        private void SaveTask_Clicked(object sender, EventArgs e)
        {

        }

    }
}