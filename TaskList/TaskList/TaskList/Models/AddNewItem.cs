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

        // constructors
        public AddNewItem()
        {

        }

        public AddNewItem(string Title)
        {
            this.Title = Title;
            IsComplete = false;
        }

        public AddNewItem(string Title, string Reminder)
        {
            this.Title = Title;
            this.Reminder = Reminder;
            IsComplete = false;
        }
    }
}
