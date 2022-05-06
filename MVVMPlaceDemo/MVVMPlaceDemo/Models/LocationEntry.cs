using System;
using SQLite;

namespace MVVMPlaceDemo.Models
{
    public class LocationEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        
    }
}
