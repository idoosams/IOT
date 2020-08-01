namespace OnTrackAzureFunctions
{
    public class SessionParticipant
    {
        public string id;
        public string groupId;
        public string groupName;
        public bool isAdmin;
        public string participantId;

        public SessionParticipant(string id, string groupId, string groupName, bool isAdmin, string participantId)
        {
            this.id = id;
            this.groupId = groupId;
            this.groupName = groupName;
            this.isAdmin = isAdmin;
            this.participantId = participantId;
        }
    }
}
