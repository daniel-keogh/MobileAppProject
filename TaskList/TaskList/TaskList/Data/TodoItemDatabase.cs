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
            return database.Table<AddNewItem>().ToListAsync();
        }

        // Search Function
        public Task<List<AddNewItem>> GetItemAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return GetItemsAsync();

            return database.Table<AddNewItem>().Where(s => s.Title.Contains(searchText)).ToListAsync();
        }

        public Task<List<AddNewItem>> GetItemsTodayAsync()
        {
            string todaysDate = DateTime.Now.ToString("yyyy-MM-dd");

            string sql = "SELECT * FROM [AddNewItem] WHERE [ReminderDate] = '" + todaysDate + "'";
            return database.QueryAsync<AddNewItem>(sql);
        }

        public Task<List<AddNewItem>> GetItemsThisWeekAsync()
        {
            string tomorrowsDate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string nextWeekDate = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");

            string sql = "SELECT * FROM [AddNewItem] WHERE [ReminderDate] BETWEEN '" + tomorrowsDate + "' AND '" + nextWeekDate + "'";
            return database.QueryAsync<AddNewItem>(sql);
        }

        public Task<int> SaveItemAsync(AddNewItem item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
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
