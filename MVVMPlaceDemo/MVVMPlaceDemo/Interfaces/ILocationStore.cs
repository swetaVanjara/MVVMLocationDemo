using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MVVMPlaceDemo.Models;

namespace MVVMPlaceDemo.Interfaces
{
    public interface ILocationStore
    {
        Task<IEnumerable<LocationEntry>> GetLocationAsync();
        Task<LocationEntry> GetLocation(int id);
        Task AddLocation(LocationEntry location);
        Task UpdateLocation(LocationEntry location);
        Task DeleteLocation(LocationEntry location);
    }
}
