using AutoMapper;
using Dapper;
using HumanityService.DataContracts.CompositeDesignPattern;
using HumanityService.Exceptions;
using HumanityService.Stores.Interfaces;
using System.Data;
using System.Threading.Tasks;

namespace HumanityService.Stores
{
    public class LocationStore : ILocationStore
    {
        private readonly IConnectionFactory _sqlConnectionFactory;

        public LocationStore(IConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        private static readonly string[] LocationsTableColumns =
        {
            nameof(LocationEntity.Username),
            nameof(LocationEntity.Longitude),
            nameof(LocationEntity.Latitude),
            nameof(LocationEntity.Description)
        };

        public async Task AddLocation(string username, Location location)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();
            var sql = new QueryBuilder().InsertInto("locations", LocationsTableColumns).Build();
            var locationEntity = ToLocationEntity(username, location);
            await connection.ExecuteAsync(sql, locationEntity);
        }

        public async Task DeleteLocation(string username)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().DeleteFrom("locations")
                .Where("Username = @Username").Build();

            await connection.ExecuteAsync(sql, new
            {
                Username = username
            });
        }

        public async Task<Location> GetLocation(string username)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .SelectColumns("locations", LocationsTableColumns)
                .Where("Username = @Username")
                .Build();

            var locationEntity = await connection.QueryFirstOrDefaultAsync<LocationEntity>(sql, new { Username = username });
            var location = ToLocation(locationEntity);
            if (location == null)
            {
                throw new StorageErrorException($"Location entity for username {username} was not found", 404);
            }

            return location;
        }

        public async Task UpdateLocation(string username, Location location)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().Update("locations", LocationsTableColumns)
                .Where($"Username = @Username").Build();

            var locationEntity = ToLocationEntity(username, location);
            int rowsAffected = await connection.ExecuteAsync(sql, locationEntity);
            if (rowsAffected == 0)
            {
                throw new StorageErrorException($"Location entity with username {username} was not found", 404);
            }
        }

        public async Task<bool> LocationExists(string username)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .Count("locations")
                .Where("Username = @Username")
                .Build();

            int count = await connection.QuerySingleAsync<int>(sql, new { Username = username });
            return count > 0;
        }

        private LocationEntity ToLocationEntity(string username, Location location)
        {
            return new LocationEntity
            {
                Username = username,
                Longitude = location.Longitude,
                Latitude = location.Latitude,
                Description = location.Description
            };
        }

        private Location ToLocation(LocationEntity location)
        {
            return new Location
            {
                Longitude = location.Longitude,
                Latitude = location.Latitude,
                Description = location.Description
            };
        }
    }
}
