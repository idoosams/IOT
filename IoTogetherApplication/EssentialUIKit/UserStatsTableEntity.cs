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
        double baterryCharge;//Battery
        bool connectivity;//Connectivity
        
        public UserStatsTableEntity(double latitude, double longtitude, double speed, double batteryCharge, bool connectivity)
        {
            this.latitude = latitude;
            this.longtitude = longtitude;
            this.speed = speed;
            this.baterryCharge = batteryCharge;
            this.connectivity = connectivity;
            
        }
    }
}
