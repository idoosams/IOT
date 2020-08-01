using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnTrackAzureFunctions
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

        public async static Task<ParticipantTableEntity> InsertUpdate(Participant participant)
        {
            var entity = new ParticipantTableEntity(participant);
            TableOperation replace = TableOperation.InsertOrReplace(entity);
            TableResult result = await UsersInfo.ExecuteAsync(replace);
            return ((ParticipantTableEntity)result.Result);
        }

        public async static Task<UserStatsTableEntity> SaveUserStats(string id, double latitude, double longitude, double speed, double chargeLevel)
        {
            var entity = new UserStatsTableEntity(id, latitude, longitude, speed, chargeLevel, true);
            TableOperation replace = TableOperation.InsertOrReplace(entity);
            TableResult result = await UserStats.ExecuteAsync(replace);
            return ((UserStatsTableEntity)result.Result);
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


        public async static Task<UserStatsTableEntity> GetUserStats(string id)
        {
            TableOperation retrieve = TableOperation.Retrieve<UserStatsTableEntity>("", id);
            TableResult result = await UserStats.ExecuteAsync(retrieve);
            return ((UserStatsTableEntity)result.Result);
        }

        public async static Task<SessionParticipantTableEntity> AddParticipantToGroup(SessionParticipant sessionParticipant)
        {
            var entity = new SessionParticipantTableEntity(sessionParticipant);
            TableOperation replace = TableOperation.InsertOrReplace(entity);
            TableResult result = await GroupInfo.ExecuteAsync(replace);
            return ((SessionParticipantTableEntity)result.Result);
        }

        public async static Task<SessionParticipantTableEntity> GetParticipantGroup(string id)
        {
            TableOperation retrieve = TableOperation.Retrieve<SessionParticipantTableEntity>("", id);
            TableResult result = await GroupInfo.ExecuteAsync(retrieve);
            return ((SessionParticipantTableEntity)result.Result);
        }
    }
}
