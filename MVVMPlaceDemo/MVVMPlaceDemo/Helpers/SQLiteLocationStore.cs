using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MVVMPlaceDemo.Interfaces;
using MVVMPlaceDemo.Models;
using SQLite;

namespace MVVMPlaceDemo.Helpers
{
    public class SQLiteLocationStore : ILocationStore
    {
        private SQLiteAsyncConnection _connection;
        public SQLiteLocationStore(ISQLiteDb db)
        {
            _connection = db.GetConnection();
            _connection.CreateTableAsync<LocationEntry>();
        }


        public async Task AddLocation(LocationEntry location)
        {
          int id=  await _connection.InsertAsync(location);
            Console.WriteLine(id);
        }

        public async Task DeleteLocation(LocationEntry location)
        {
            await _connection.DeleteAsync(location);
        }

        public async Task<LocationEntry> GetLocation(int id)
        {
            return await _connection.FindAsync<LocationEntry>(id);
        }

        public async Task<IEnumerable<LocationEntry>> GetLocationAsync()
        {
            return await _connection.Table<LocationEntry>().ToListAsync();
        }

        public async Task UpdateLocation(LocationEntry location)
        {
            await _connection.UpdateAsync(location);
        }
    }
}
