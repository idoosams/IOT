using EssentialUIKit.AppLayout.Views;
using EssentialUIKit.ViewModels.Forms;
using EssentialUIKit.Views.Catalog;
using EssentialUIKit.Views.Detail;
using EssentialUIKit.Views.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using Map = Xamarin.Forms.Maps.Map;

namespace EssentialUIKit.ViewModels
{
    /// <summary>
    /// ViewModel for login page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class MapPageViewModel : BaseViewModel
    {
        #region Fields

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance for the <see cref="MapPageViewModel" /> class.
        /// </summary>
        public MapPageViewModel()
        {
        }

        #endregion

        #region property
        
        #endregion

        #region Command

        #endregion

        #region methods

        public async static Task<MapPage> GetMap(string groupId)
        {
            var users = App._activeUsers;
            var userStats = App._userStats;
            var cords = new List<List<double>>();

            var map = new Map
            {
                IsShowingUser = true,
            };

            users.ForEach(u => map.Pins.Add( new Pin { Label = $"{u.FirstName} {u.LastName}", Position = new Position(userStats[u.RowKey].Latitude, userStats[u.RowKey].Longtitude) }));

            var location = await Geolocation.GetLocationAsync();
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(20)));                      

            var newMap = new MapPage();
            newMap.Content = map;
            return newMap;
        }

        #endregion
    }
}