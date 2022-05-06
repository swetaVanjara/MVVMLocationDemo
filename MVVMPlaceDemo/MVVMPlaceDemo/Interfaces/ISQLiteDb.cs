using System;
using SQLite;

namespace MVVMPlaceDemo.Interfaces
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
