namespace OnTrackAzureFunctions
{
    public class Participant
    {
        public string id;
        public string firstName;
        public string lastName;
        public string phone;
        public string emergencyPhone;
        public string emergencyName;
        public string email;
        public string password;

        public Participant(string id, string firstName, string lastName, string phone, string emergencyPhone, string emergencyName, string email, string password)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.phone = phone;
            this.emergencyPhone = emergencyPhone;
            this.emergencyName = emergencyName;
            this.email = email;
            this.password = password;
        }
    }
}
