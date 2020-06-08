using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace EssentialUIKit
{
    public class UserStatsTableEntity : TableEntity
    {
        
        double Latitude { get; set; } //Geolocation
        double Longtitude { get; set; } //Geolocation
        double Speed { get; set; }//Geolocation
        double BaterryCharge { get; set; }//Battery
        bool Connectivity { get; set; }//Connectivity
        string Id { get; set; }


        public UserStatsTableEntity(double Latitude, double Longtitude, double Speed, double BatteryCharge, bool Connectivity, string Id)
        {
            PartitionKey = "";
            RowKey = Id;
            this.Latitude = Latitude;
            this.Longtitude = Longtitude;
            this.Speed = Speed;
            this.BaterryCharge = BatteryCharge;
            this.Connectivity = Connectivity;
            this.Id = Id;
            
        }
        public UserStatsTableEntity()
        {

        }
    }
}
