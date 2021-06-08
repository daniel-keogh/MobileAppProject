using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaskList
{
    public class TodoItemDatabase
    {
        readonly SQLiteAsyncConnection database;

        public TodoItemDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<AddNewItem>().Wait();
        }

        public Task<List<AddNewItem>> GetItemsAsync()
        {
            // return all items in the database
            return database.Table<AddNewItem>().ToListAsync();
        }

        // Search Function
        public Task<List<AddNewItem>> GetItemAsync(string searchText)
        {
            // if the searchbox is empty, return all items
            if (string.IsNullOrWhiteSpace(searchText))
                return GetItemsAsync();

            // return all items that contains searchText (case-insensitive)
            return database.Table<AddNewItem>().Where(s => s.Title.ToUpper().Contains(searchText.ToUpper())).ToListAsync();
        }

        public Task<List<AddNewItem>> GetItemsTodayAsync()
        {
            // because reminder date is stored as "yyyy-MM-dd"
            string todaysDate = DateTime.Now.ToString("yyyy-MM-dd");

            // return all items where ReminderDate matches todaysDate
            string sql = "SELECT * FROM [AddNewItem] WHERE [ReminderDate] = '" + todaysDate + "'";
            return database.QueryAsync<AddNewItem>(sql);
        }

        public Task<List<AddNewItem>> GetItemsThisWeekAsync()
        {
            // because reminder date is stored as "yyyy-MM-dd"
            string tomorrowsDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string nextWeekDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");

            // return all items whose ReminderDate is between tomorrow and seven days time
            string sql = "SELECT * FROM [AddNewItem] WHERE [ReminderDate] BETWEEN '" + tomorrowsDate + "' AND '" + nextWeekDate + "'";
            return database.QueryAsync<AddNewItem>(sql);
        }

        public Task<int> SaveItemAsync(AddNewItem item)
        {
            if (item.ID != 0)
            {
                // if the item already exists (eg. used if OnCheckbox is called)
                return database.UpdateAsync(item);
            }
            else
            {
                // if the item doesn't already exist
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(AddNewItem item)
        {
            return database.DeleteAsync(item);
        }

        public Task<int> DeleteAllItemsAsync()
        {
            return database.DeleteAllAsync<AddNewItem>();
        }

        public Task<int> DeleteDoneItemsAsync()
        {
            // delete items where IsComplete is true
            return database.Table<AddNewItem>().Where(i => i.IsComplete == true).DeleteAsync();
        }

        public void OnChecked(AddNewItem item)
        {
            if (item.IsComplete)
                item.CheckboxSource = "unchecked.png";
            else
                item.CheckboxSource = "checked.png";

            item.IsComplete = !item.IsComplete;
            SaveItemAsync(item);
        }
    }
}
