﻿using System;
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
		public NewTaskPage ()
		{
			InitializeComponent ();
		}

        // method to display the date/time pickers if the switch is toggled on
        void SwitchToggled(object sender, ToggledEventArgs e)
        {
            if (RemindYesOrNo.IsToggled)
                SetReminder.IsVisible = true;
            else
                SetReminder.IsVisible = false;

        }

        // time picker event handler
        void TimeSelected(object sender, PropertyChangedEventArgs e)
        {

        }

        // date picker event handler
        void DateSelected(object sender, DateChangedEventArgs e)
        {

        }
    }
}