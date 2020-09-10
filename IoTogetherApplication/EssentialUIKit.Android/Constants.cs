using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EssentialUIKit.Droid
{
    public static class Constants
    {
        public const string ListenConnectionString = "Endpoint=sb://iotogethernotificationsns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=vnEBSOEvJrgYzEXzG2RINKFa807PV2+VK+z27bhjniQ=";
        public const string NotificationHubName = "IOTogetherNotifications";
    }

}