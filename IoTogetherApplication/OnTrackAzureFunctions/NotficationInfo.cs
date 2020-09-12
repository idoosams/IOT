using System;
using System.Collections.Generic;
using System.Text;

namespace OnTrackAzureFunctions
{
    public class NotficationInfo
    {

            public string UserName;
            public string Message;

            public NotficationInfo(string userName, string message)
            {
                this.UserName = userName;
                this.Message = message;
            }


}
}
