using Microsoft.Azure.Cosmos.Table;

namespace EssentialUIKit
{
    public class UserStatsTableEntity : TableEntity
    {
        public string Id { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public double Speed { get; set; }
        public double BaterryCharge { get; set; }
        public bool Connectivity { get; set; }
        
        public UserStatsTableEntity(string id, double latitude, double longtitude, double speed, double batteryCharge, bool connectivity)
        {
            PartitionKey = "";
            RowKey = id;
            Latitude = latitude;
            Longtitude = longtitude;
            Speed = speed;
            BaterryCharge = batteryCharge;
            Connectivity = connectivity;            
        }

        public UserStatsTableEntity()
        {

        }
    }
}
