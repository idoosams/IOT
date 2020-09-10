namespace EssentialUIKit
{
    public class UserStats
    {
        public string Id { get; set; } 
        public double Latitude { get; set; }
        public double Longtitude { get; set; }
        public double Speed { get; set; }
        public double BaterryCharge { get; set; }
        public bool Connectivity { get; set; }

        public UserStats(string id, double latitude, double longtitude, double speed, double batteryCharge, bool connectivity)
        {
            Id = id;
            Latitude = latitude;
            Longtitude = longtitude;
            Speed = speed;
            BaterryCharge = batteryCharge;
            Connectivity = connectivity;
        }
    }
}
