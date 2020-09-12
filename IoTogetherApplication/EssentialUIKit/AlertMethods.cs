﻿using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using GeoCoordinatePortable;
using System.Threading.Tasks;

namespace EssentialUIKit
{
    public class AlertMethods
    {
        public static async Task ManageAlertNotfication()
        {
            UserStatsTableEntity adminStatEntity;
            if (App._userStats.TryGetValue(App._adminId, out adminStatEntity))
            {
                string prefixNotfication = $"{App._user.FirstName} {App._user.LastName}";
                var adminLocation = new GeoCoordinate(adminStatEntity.Latitude, adminStatEntity.Longtitude);
                var location = await Geolocation.GetLocationAsync();
                var userLocation = new GeoCoordinate(location.Latitude, location.Longitude);
                var distanceFromAdmin = (int)adminLocation.GetDistanceTo(userLocation);
                if (distanceFromAdmin >= 5000)
                {
                    await AzureClient.SendNotfication(new NotficationInfo(prefixNotfication, "is loosing connection!"));
                }
            }
            if(Battery.ChargeLevel <= 0.15)
            {
                await AzureClient.SendNotfication(new NotficationInfo("ido samselik", " is getting far away!"));
            }
        }
    }
}
