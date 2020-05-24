using System.Threading.Tasks;
using Aub.Eece503e.ChatService.Datacontracts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Extensions.Options;
using Aub.Eece503e.ChatService.Web.Store.Exceptions;

namespace Aub.Eece503e.ChatService.Web.Store.Azure
{
    public class AzureTableProfileStore : IProfileStore
    {
        private readonly CloudTable _table;

        public AzureTableProfileStore(IOptions<AzureStorageSettings> options)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(options.Value.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(options.Value.UsersTableName);
        }


        public async Task AddUser(User user)
        {
            UserTableEntity entity = ToEntity(user);
            var insertOperation = TableOperation.Insert(entity);
            try
            {
                await _table.ExecuteAsync(insertOperation);
            }
            catch (StorageException e)
            {
                throw new StorageErrorException($"Failed to add user {user.Username}", e, e.RequestInformation.HttpStatusCode);
            }
        }

        public async Task<User> GetUser(string username)
        {
            UserTableEntity entity = await RetrieveUserEntity(username);
            return ToUser(entity);
        }
        
        public async Task UpdateUser(User user)
        { 
            await RetrieveUserEntity(user.Username);
            UserTableEntity entity = ToEntity(user);
            var updateOperation = TableOperation.InsertOrReplace(entity);    
            try
            {
                await _table.ExecuteAsync(updateOperation);
            }
            catch (StorageException e)
            {
                throw new StorageErrorException($"Failed to update user {user.Username}", e, e.RequestInformation.HttpStatusCode);
            }
        }

        public async Task DeleteUser(string username)
        {
            UserTableEntity entity = await RetrieveUserEntity(username);
            var deleteOperation = TableOperation.Delete(entity);
            await _table.ExecuteAsync(deleteOperation);
        }

        private static UserTableEntity ToEntity(User user)
        {
            return new UserTableEntity
            {
                PartitionKey = "username",
                RowKey = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePictureId = user.ProfilePictureId
            };
        }

        private static User ToUser(UserTableEntity entity)
        {
            return new User
            {
                Username = entity.RowKey,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                ProfilePictureId = entity.ProfilePictureId
            };
        }

        private async Task<UserTableEntity> RetrieveUserEntity(string username)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<UserTableEntity>(partitionKey: "username", rowkey: username);
            TableResult tableResult = await _table.ExecuteAsync(retrieveOperation);
            var entity = (UserTableEntity)tableResult.Result;
            if (entity == null)
            {
                throw new StorageErrorException($"User with username {username} was not found", 404);
            }
            return entity;
        }
    }
}
