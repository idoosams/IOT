﻿using Xamarin.Forms;

namespace EssentialUIKit
{
    public static class Constants
    {
        public static string HostName { get; set; } = "https://ontracksignalrfunctions.azurewebsites.net/";

        // Used to differentiate message types sent via SignalR. This
        // sample only uses a single message type.
        public static string MessageName { get; set; } = "newMessage";

        public static string Username
        {
            get
            {
                return $"{Device.RuntimePlatform} User";
            }
        }
    }
}
