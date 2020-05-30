using Microsoft.Azure.Cosmos.Table;

namespace EssentialUIKit
{
    public class SessionParticipantTableEntity : TableEntity
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsAdmin { get; set; }
        public string ParticipantId { get; set; }

        public SessionParticipantTableEntity(SessionParticipant sessionParticipant)
        {
            PartitionKey = "";
            RowKey = sessionParticipant.id;
            GroupId = sessionParticipant.groupId;
            GroupName = sessionParticipant.groupName;
            IsAdmin = sessionParticipant.isAdmin;
            ParticipantId = sessionParticipant.participantId;
        }

        public SessionParticipantTableEntity()
        {

        }
    }
}
