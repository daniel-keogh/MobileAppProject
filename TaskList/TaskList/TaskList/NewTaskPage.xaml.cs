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

		public NewTaskPage ()
		{
			InitializeComponent ();
		}

        // method to display the date/time pickers if the switch is toggled on
        void OnSwitchToggled(object sender, ToggledEventArgs e)
        {
            if (RemindYesOrNo.IsToggled)
                SetReminder.IsVisible = true;
            else
                SetReminder.IsVisible = false;

        }

        // time picker event handler
        void OnTimeSelected(object sender, PropertyChangedEventArgs e)
        {
            string hours = ReminderTime.Time.ToString("hh");
            string minutes = ReminderTime.Time.ToString("mm");
            time = hours + ":" + minutes;
            DisplayConfirmationString();
        }

        // date picker event handler
        void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            date = ReminderDate.Date.ToString("MMM d, yyyy");
            DisplayConfirmationString();
        }

        void DisplayConfirmationString()
        {
            ChosenDateTime.Text = "Reminder set for " + date + " at " + time;
        }
    }
}