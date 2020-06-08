using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EssentialUIKit
{
    public static class AzureDbClient
    {
        static readonly CloudStorageAccount StorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=skeletonfunctions2020040;AccountKey=Dm0H3WYPgLHfeIIfr/f3SvSUahywA53W320J6MzMLKwWviowCPMEc8QzHHjtSb0XboribkML0kHTVOok8MbdXA==;EndpointSuffix=core.windows.net");
        static readonly CloudTableClient TableClient = StorageAccount.CreateCloudTableClient();
        static readonly CloudTable UsersInfo = TableClient.GetTableReference("UsersInfo");
        static readonly CloudTable GroupInfo = TableClient.GetTableReference("GroupInfo");
        static readonly List<string> ParticipantIdColumn = new List<string> { "ParticipantId" };
        static readonly List<string> GroupColumns = new List<string> { "GroupId", "GroupName" };

        public async static Task<ParticipantTableEntity> SaveParticipant(Participant participant)
        {
            var entity = new ParticipantTableEntity(participant);
            TableOperation replace = TableOperation.InsertOrReplace(entity);
            TableResult result = await UsersInfo.ExecuteAsync(replace);
            return ((ParticipantTableEntity)result.Result);
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

        public static List<ParticipantTableEntity> GetGroupParticipants(string groupId)
        {
            var query = new TableQuery<SessionParticipantTableEntity>();
            query.Where(TableQuery.GenerateFilterCondition("GroupId", QueryComparisons.Equal, groupId));
            query.Select(ParticipantIdColumn);
            var x = GroupInfo.ExecuteQuery(query).ToList();
            List<string> participantsIds = GroupInfo.ExecuteQuery(query).ToList().Where(entry => !string.IsNullOrWhiteSpace(entry.ParticipantId)).Select(entry => entry.ParticipantId).ToList();

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
            TableQuery<ParticipantTableEntity> finalQuery = new TableQuery<ParticipantTableEntity>().Where(activeUsersQuery);
            List<ParticipantTableEntity> activeUsers = UsersInfo.ExecuteQuery(finalQuery).ToList();

            return activeUsers;
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
