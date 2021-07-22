using AutoMapper;
using HumanityService.DataContracts.CompositeDesignPattern;
using HumanityService.Stores.Interfaces;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using HumanityService.Exceptions;

namespace HumanityService.Stores
{
    public class UserStore : IUserStore
    {
        private readonly IConnectionFactory _sqlConnectionFactory;
        private readonly IMapper _mapper;
        private readonly ILocationStore _locationStore;


        private static readonly string[] UsersTableColumns =
        {
            nameof(UserEntity.Username),
            nameof(UserEntity.Email),
            nameof(UserEntity.FirstName),
            nameof(UserEntity.LastName),
            nameof(UserEntity.PhoneNumber),
            nameof(UserEntity.Password)
        };

        private static readonly string[] NgosTableColumns =
        {
            nameof(NgoEntity.Name),
            nameof(NgoEntity.Username),
            nameof(NgoEntity.Email),
            nameof(NgoEntity.Password),
            nameof(NgoEntity.PhoneNumber),
            nameof(NgoEntity.RegistrationNumber),
            nameof(NgoEntity.WebsiteAddress),
            nameof(NgoEntity.Description)
        };

        public UserStore(IConnectionFactory sqlConnectionFactory, ILocationStore locationStore)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _locationStore = locationStore; 

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserEntity>();
                cfg.CreateMap<UserEntity, User>();
                cfg.CreateMap<Ngo, NgoEntity>();
                cfg.CreateMap<NgoEntity, Ngo>();
            });
            _mapper = configuration.CreateMapper();
        }

        public async Task AddNgo(Ngo ngo)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();
            var sql = new QueryBuilder().InsertInto("ngos", NgosTableColumns).Build();
            var ngoEntity = ToNgoEntity(ngo);
            await connection.ExecuteAsync(sql, ngoEntity);
            await _locationStore.AddLocation(ngo.Username, ngo.Location);
        }

        public async Task AddUser(User user)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();
            var sql = new QueryBuilder().InsertInto("users", UsersTableColumns).Build();
            var userEntity = ToUserEntity(user);
            await connection.ExecuteAsync(sql, userEntity);
            await _locationStore.AddLocation(user.Username, user.Location);
        }

        public async Task DeleteNgo(string ngoUsername)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().DeleteFrom("ngos")
                .Where("Username = @Username").Build();

            await connection.ExecuteAsync(sql, new
            {
                Username = ngoUsername
            });
            await _locationStore.DeleteLocation(ngoUsername);
        }

        public async Task DeleteUser(string username)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().DeleteFrom("users")
                .Where("Username = @Username").Build();

            await connection.ExecuteAsync(sql, new
            {
                Username = username
            });
            await _locationStore.DeleteLocation(username);
        }

        public async Task<Ngo> GetNgo(string ngoUsername)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .SelectColumns("ngos", NgosTableColumns)
                .Where("Username = @Username")
                .Build();

            var ngoEntity = await connection.QueryFirstOrDefaultAsync<NgoEntity>(sql, new { Username = ngoUsername });
            if (ngoEntity == null)
            {
                throw new StorageErrorException($"Ngo entity with username {ngoUsername} was not found", 404);
            }
            var location = await _locationStore.GetLocation(ngoUsername);
            var ngo = ToNgo(ngoEntity, location);
            return ngo;
        }

        public async Task<User> GetUser(string username)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .SelectColumns("users", UsersTableColumns)
                .Where("Username = @Username")
                .Build();

            var userEntity = await connection.QueryFirstOrDefaultAsync<UserEntity>(sql, new { Username = username });
            if (userEntity == null)
            {
                throw new StorageErrorException($"User entity with username {username} was not found", 404);
            }
            var location = await _locationStore.GetLocation(username);
            var user = ToUser(userEntity, location);

            return user;
        }

        public async Task UpdateNgo(Ngo ngo)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().Update("ngos", NgosTableColumns)
                .Where($"Username = @Username").Build();

            var ngoEntity = ToNgoEntity(ngo);
            int rowsAffected = await connection.ExecuteAsync(sql, ngoEntity);
            if (rowsAffected == 0)
            {
                throw new StorageErrorException($"Ngo entity with username {ngo.Username} was not found", 404);
            }
            await _locationStore.UpdateLocation(ngo.Username, ngo.Location);
        }

        public async Task UpdateUser(User user)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder().Update("users", UsersTableColumns)
                .Where($"Username = @Username").Build();

            var userEntity = ToUserEntity(user);
            int rowsAffected = await connection.ExecuteAsync(sql, user);
            if (rowsAffected == 0)
            {
                throw new StorageErrorException($"User entity with username {user.Username} was not found", 404);
            }
            await _locationStore.UpdateLocation(user.Username, user.Location);
        }


        public async Task<bool> UserOrNgoExists(string table, string username)
        {
            using IDbConnection connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();

            var sql = new QueryBuilder()
                .Count(table)
                .Where("Username = @Username")
                .Build();

            int count = await connection.QuerySingleAsync<int>(sql, new { Username = username });
            return count > 0;
        }


        private UserEntity ToUserEntity(User user)
        {
            var entity = _mapper.Map<UserEntity>(user);
            return entity;
        }

        private User ToUser(UserEntity entity, Location location)
        {
            if (entity == null)
            {
                return null;
            }

            var user = _mapper.Map<User>(entity);
            user.Location = location;
            return user;
        }

        private NgoEntity ToNgoEntity(Ngo ngo)
        {
            var entity = _mapper.Map<NgoEntity>(ngo);
            return entity;
        }

        private Ngo ToNgo(NgoEntity entity, Location location)
        {
            if (entity == null)
            {
                return null;
            }

            var ngo = _mapper.Map<Ngo>(entity);
            ngo.Location = location;
            return ngo;
        }
    }
}
