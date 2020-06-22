using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EssentialUIKit.Sensors
{
    class AlertMethods
    {
        /*public async double CheckGroupDistanceAsync()
        {
            var participants = App._activeUsers;
            var location = await Geolocation.GetLastKnownLocationAsync();
            foreach (var participant in participants)
            {
                var distance = Location.CalculateDistance(location, participant.location, DistanceUnits.Kilometers);
                if (distance > 1)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert!", participant.FirstName + " " + participant.LastName + "is out of range!", "Thanks");
                }
            }
        }

        public async double CheckParticipantsBatteryLevel()
        {
            var participants = App._activeUsers;
            foreach (var participant in participants)
            {
                var batteryLevel = participant.batterylevel;
                if (batteryLevel < 15)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert!", participant.FirstName + " " + participant.LastName + "has a low battery level!", "Thanks");
                }
            }
        }

        public async double CheckParticipantsAccessNetworkStateAsync()
        {
            var participants = App._activeUsers;
            foreach (var participant in participants)
            {
                var networkState = participant.networkState;
                if (networkState == NetworkAccess.None || networkState == NetworkAccess.Unknown)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert!", participant.FirstName + " " + participant.LastName + "has a connectivity issue!", "Thanks");
                }
            }

        }*/
    }
}
