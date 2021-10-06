using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Models;
using SQLite;

namespace ToDoList.Services
{
    public class SqLiteDataStore : IDataStore<Item>
    {
        public SqLiteDataStore()
        {
            _dataBase = new SQLiteAsyncConnection($"{Environment.GetFolderPath(Environment.SpecialFolder.Personal)}/todolist.db3");
            _dataBase.CreateTableAsync<Item>().Wait();
        }

        private readonly SQLiteAsyncConnection _dataBase;

        public async Task<bool> AddItemAsync(Item item)
        {
            await _dataBase.InsertAsync(item);

            return await Task.FromResult(true);
        }
        public async Task<bool> UpdateItemAsync(Item item)
        {
            await _dataBase.Table<Item>().DeleteAsync(arg => arg.Id == item.Id);
            await _dataBase.InsertAsync(item);
            
            return await Task.FromResult(true);
        }
        public async Task<bool> DeleteItemAsync(string id)
        {
            await _dataBase.Table<Item>().DeleteAsync(arg => arg.Id == id);

            return await Task.FromResult(true);
        }
        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(_dataBase.Table<Item>().FirstOrDefaultAsync(s => s.Id == id).Result);
        }
        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_dataBase.Table<Item>().ToListAsync().Result.OrderBy(item => item.Date));
        }
    }
}