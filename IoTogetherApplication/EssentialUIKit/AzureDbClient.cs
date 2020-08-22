using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Syncfusion.DataSource.Extensions;
using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EssentialUIKit
{
    public static class AzureDbClient
    {
        static readonly CloudStorageAccount StorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=skeletonfunctions2020040;AccountKey=Dm0H3WYPgLHfeIIfr/f3SvSUahywA53W320J6MzMLKwWviowCPMEc8QzHHjtSb0XboribkML0kHTVOok8MbdXA==;EndpointSuffix=core.windows.net");
        static readonly CloudTableClient TableClient = StorageAccount.CreateCloudTableClient();
        static readonly CloudTable UsersInfo = TableClient.GetTableReference("UsersInfo");
        static readonly CloudTable GroupInfo = TableClient.GetTableReference("GroupInfo");
        static readonly CloudTable UserStats = TableClient.GetTableReference("UserStats");
        static readonly List<string> ParticipantIdAndAdminColumn = new List<string> { "ParticipantId", "IsAdmin" };
        static readonly List<string> GroupColumns = new List<string> { "GroupId", "GroupName" };
        static HttpClient client = new HttpClient();

        public async static Task SaveParticipant(Participant participant)
        {
            var json = JsonConvert.SerializeObject(participant);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"{Constants.HostName}/api/SignUp", content);
        }

        public async static Task<HttpResponseMessage> DeleteParticipantFromGroup(SessionParticipant sessionParticipant)
        {
            var json = JsonConvert.SerializeObject(sessionParticipant);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"{Constants.HostName}/api/LeaveGroup", content);
            return result;
        }

        public async static Task<HttpResponseMessage> AddParticipantToGroup(SessionParticipant sessionParticipant)
        {
            var json = JsonConvert.SerializeObject(sessionParticipant);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"{Constants.HostName}/api/JoinGroup", content);
            return result;
        }

        public static string GroupIdToName(string groupId)
        {
            var query = new TableQuery<SessionParticipantTableEntity>();
            query.Where(TableQuery.GenerateFilterCondition("GroupId", QueryComparisons.Equal, groupId));
            query.Select(GroupColumns);
            var result = GroupInfo.ExecuteQuery(query).ToList();
            if (result.Count < 1)
            {
                return null;
            }
            return result.First().GroupName;
        }

        public static async Task<List<ParticipantTableEntity>> GetGroupParticipantsAsync(string groupId)
        {
            var result = await client.GetAsync($"{Constants.HostName}/api/GetGroup?groupId={groupId}");
            var jsonString = await result.Content.ReadAsStringAsync();
            var participantTableEntity = JsonConvert.DeserializeObject<List<ParticipantTableEntity>>(jsonString);
            return participantTableEntity;
        }

        public static async Task<Dictionary<string, UserStatsTableEntity>> GetGroupStatsAsync()
        {
            List<string> participantsIds = App._activeUsers.Select(x => x.RowKey).ToList();
            var json = JsonConvert.SerializeObject(participantsIds);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"{Constants.HostName}/api/GetGroupStats", content);
            var jsonString = await result.Content.ReadAsStringAsync();
            var dict = JsonConvert.DeserializeObject<Dictionary<string, UserStatsTableEntity>>(jsonString);

            return dict;
        }

        public static async Task<ParticipantTableEntity> TryGetUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var client = new HttpClient();
            var result = await client.GetAsync($"{Constants.HostName}/api/Login?password={password}&email={email}");
            var jsonString = await result.Content.ReadAsStringAsync();
            var participantTableEntity = JsonConvert.DeserializeObject<ParticipantTableEntity>(jsonString);
            return participantTableEntity;
        }
    }
}
