using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace EssentialUIKit
{
    public class ParticipantTableEntity : TableEntity
    {

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string EmergencyPhone { get; set; }
        public string EmergencyName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ParticipantTableEntity(Participant participant)
        {
            PartitionKey = "";
            RowKey = participant.id;
            FirstName = participant.firstName;
            LastName = participant.lastName;
            Phone = participant.phone;
            EmergencyPhone = participant.emergencyPhone;
            EmergencyName = participant.emergencyName;
            Email = participant.email;
            Password= participant.password;
        }

        public ParticipantTableEntity()
        {

        }
    }
}
