using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskList
{
    public class AddNewItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Reminder { get; set; }
        public bool IsComplete { get; set; }
        public string CheckboxSource { get; set; }
        public string ReminderDate { get; set; }

        // constructors
        public AddNewItem()
        {

        }

        public AddNewItem(string Title)
        {
            this.Title = Title;

            Reminder = null;
            IsComplete = false;
            CheckboxSource = "unchecked.png";
        }

        public AddNewItem(string Title, string Reminder, string ReminderDate)
        {
            this.Title = Title;
            this.Reminder = Reminder;
            this.ReminderDate = ReminderDate;

            IsComplete = false;
            CheckboxSource = "unchecked.png";
        }
    }
}
