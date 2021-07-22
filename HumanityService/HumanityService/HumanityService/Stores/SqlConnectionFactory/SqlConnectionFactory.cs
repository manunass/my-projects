using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace HumanityService.Stores
{
    public class SqlConnectionFactory : IConnectionFactory
    {
        private readonly IOptions<SqlDatabaseSettings> _options;

        public SqlConnectionFactory(IOptions<SqlDatabaseSettings> options)
        {
            _options = options;
        }
        
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_options.Value.ConnectionString);
        }
    }
}