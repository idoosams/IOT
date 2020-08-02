using System;
using System.Collections.Generic;
using System.Text;

namespace OnTrackAzureFunctions
{
    class Constants
    {
        // NOTE: for clients to receive messages, this value must match
        // the value in the ChatClient Constants.cs file.
        public static string UserTarget { get; set; } = "newMessage";
    }
}
