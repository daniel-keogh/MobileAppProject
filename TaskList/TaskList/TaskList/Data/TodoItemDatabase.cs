using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public Task<List<AddNewItem>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<AddNewItem>("SELECT * FROM [AddNewItem] WHERE [IsComplete] = 0");
        }

        public Task<AddNewItem> GetItemAsync(int id)
        {
            return database.Table<AddNewItem>().Where(i => i.ID == id).FirstOrDefaultAsync();
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
    }
}
