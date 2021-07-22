using System.Data;

namespace HumanityService.Stores
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}