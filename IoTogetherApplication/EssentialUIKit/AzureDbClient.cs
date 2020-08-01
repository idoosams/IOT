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

        public async static Task<ParticipantTableEntity> SaveParticipant(Participant participant)
        {
            var entity = new ParticipantTableEntity(participant);
            TableOperation replace = TableOperation.InsertOrReplace(entity);
            TableResult result = await UsersInfo.ExecuteAsync(replace);
            return ((ParticipantTableEntity)result.Result);
        }

        public async static void DeleteParticipantFromGroup(string rowKey)
        {
            TableOperation retrieve = TableOperation.Retrieve<SessionParticipantTableEntity>("", rowKey);
            TableResult result = await GroupInfo.ExecuteAsync(retrieve);
            SessionParticipantTableEntity entity = (SessionParticipantTableEntity)result.Result;
            
            TableOperation delete = TableOperation.Delete(entity);
            await GroupInfo.ExecuteAsync(delete);
        }

        public async static Task<ParticipantTableEntity> GetParticipant(string id)
        {
            TableOperation retrieve = TableOperation.Retrieve<ParticipantTableEntity>("", id);
            TableResult result = await UsersInfo.ExecuteAsync(retrieve);
            return ((ParticipantTableEntity)result.Result);
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
            var client = new HttpClient();
            var result = await client.GetAsync($"{Constants.HostName}/api/GetGroup?groupId={groupId}");
            var jsonString = await result.Content.ReadAsStringAsync();
            var participantTableEntity = JsonConvert.DeserializeObject<List<ParticipantTableEntity>>(jsonString);
            return participantTableEntity;
        }

        public static Dictionary<string, UserStatsTableEntity> GetGroupStats()
        {
            List<string> participantsIds = App._activeUsers.Select(x => x.RowKey).ToList();

            int i = 0;
            string activeUsersQuery = string.Empty;
            foreach (string id in participantsIds)
            {
                i++;
                if (i == 1) { activeUsersQuery = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id); }
                else
                {
                    activeUsersQuery = TableQuery.CombineFilters(
                        activeUsersQuery,
                        TableOperators.Or,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id)
                        );
                }
            }
            TableQuery<UserStatsTableEntity> finalQuery = new TableQuery<UserStatsTableEntity>().Where(activeUsersQuery);

            Dictionary<string, UserStatsTableEntity> dict = new Dictionary<string, UserStatsTableEntity>();
            UserStats.ExecuteQuery(finalQuery).ForEach(u => dict.Add(u.RowKey, u));

            App._adminLocation = new List<double> { 0, 0 };

            return dict;
        }

        public static ParticipantTableEntity TryGetUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var query = new TableQuery<ParticipantTableEntity>();
            query.Where(TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, email),
                TableOperators.And,
                TableQuery.GenerateFilterCondition("Password", QueryComparisons.Equal, password)));
            var items = UsersInfo.ExecuteQuery(query).ToList();
            if (items.Count != 1)
            {
                return null;
            }
            return (ParticipantTableEntity)items.First();
        }

        public async static Task<UserStatsTableEntity> GetUserStats(string id)
        {
            TableOperation retrieve = TableOperation.Retrieve<UserStatsTableEntity>("", id);
            TableResult result = await UserStats.ExecuteAsync(retrieve);
            return ((UserStatsTableEntity)result.Result);
        }

        public async static Task<HttpResponseMessage> AddParticipantToGroup(SessionParticipant sessionParticipant)
        {
            //var entity = new SessionParticipantTableEntity(sessionParticipant);
            //TableOperation replace = TableOperation.InsertOrReplace(entity);
            //TableResult result = await GroupInfo.ExecuteAsync(replace);
            //return ((SessionParticipantTableEntity)result.Result);

            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(sessionParticipant);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync($"{Constants.HostName}/api/JoinGroup", content);
            return result;
        }

        public async static Task<SessionParticipantTableEntity> GetParticipantGroup(string id)
        {
            TableOperation retrieve = TableOperation.Retrieve<SessionParticipantTableEntity>("", id);
            TableResult result = await GroupInfo.ExecuteAsync(retrieve);
            return ((SessionParticipantTableEntity)result.Result);
        }
    }
}
