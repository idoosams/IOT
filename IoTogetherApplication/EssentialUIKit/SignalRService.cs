using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EssentialUIKit.Models;
using EssentialUIKit.Schema;
using Xamarin.Essentials;

namespace EssentialUIKit
{
    public class SignalRService
    {
        HttpClient client;

        public delegate Task UserStatsHandler(object sender, UserStats user);
        public delegate void ConnectionHandler(object sender, bool successful, string message);

        public event UserStatsHandler NewUserStats;
        public event ConnectionHandler Connected;
        public event ConnectionHandler ConnectionFailed;
        public bool IsConnected { get; private set; }
        public bool IsBusy { get; private set; }

        public SignalRService()
        {
            client = new HttpClient();
        }

        public async Task SendUserAsync(User newUser)
        {
            IsBusy = true;

            var json = JsonConvert.SerializeObject(newUser);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"{Constants.HostName}/api/talk", content);

            IsBusy = false;
        }

        public async Task SendUserStats(string userId)
        {
            var location = await Geolocation.GetLocationAsync();
            var userStats = new UserStats(userId, location.Latitude, location.Longitude, (double)location.Speed, Battery.ChargeLevel, true);
            var json = JsonConvert.SerializeObject(userStats);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"{Constants.HostName}/api/UpdateUserStats", content);
           
            IsBusy = false;
        }

        public async Task ConnectAsync(string userId)
        {
            try
            {
                IsBusy = true;

                string negotiateJson = await client.GetStringAsync($"{Constants.HostName}/api/{userId}/negotiate");
                NegotiateInfo negotiate = JsonConvert.DeserializeObject<NegotiateInfo>(negotiateJson);

                HubConnection connection = new HubConnectionBuilder()
                    .AddNewtonsoftJsonProtocol()
                    .WithUrl(negotiate.Url, options =>
                    {
                        options.AccessTokenProvider = async () => negotiate.AccessToken;
                    })
                    .Build();

                connection.Closed += Connection_Closed;
                connection.On<UserStats>("userStatsUpdate", UpdateNewUserStats);
                await connection.StartAsync();

                IsConnected = true;
                IsBusy = false;

                //Connected?.Invoke(this, true, "Connection successful.");
            }
            catch (Exception ex)
            {
                ConnectionFailed?.Invoke(this, false, ex.Message);
                IsConnected = false;
                IsBusy = false;
            }
        }

        Task Connection_Closed(Exception arg)
        {
            ConnectionFailed?.Invoke(this, false, arg.Message);
            IsConnected = false;
            IsBusy = false;
            return Task.CompletedTask;
        }

        void UpdateNewUserStats(UserStats jsonUserStats)
        {
            var userStats = jsonUserStats;
            NewUserStats?.Invoke(this, userStats);
        }

    }
}
