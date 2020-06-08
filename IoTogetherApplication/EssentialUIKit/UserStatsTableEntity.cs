using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace EssentialUIKit
{
    public class UserStatsTableEntity : TableEntity
    {
        
        double latitude; //Geolocation
        double longtitude; //Geolocation
        double speed;//Geolocation
        double baterry_charge;//Battery
        bool connectivity;//Connectivity
        
        public UserStatsTableEntity(double latitude, double longtitude, double speed, double battery_charge, bool connectivity)
        {
            this.latitude = latitude;
            this.longtitude = longtitude;
            this.speed = speed;
            this.baterry_charge = battery_charge;
            this.connectivity = connectivity;
            
        }
    }
}
