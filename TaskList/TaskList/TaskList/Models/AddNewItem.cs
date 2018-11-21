using System;
using System.Collections.Generic;
using System.Text;

namespace TaskList.Models
{
    class AddNewItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Reminder { get; set; }
        public bool IsComplete { get; set; }
    }
}
