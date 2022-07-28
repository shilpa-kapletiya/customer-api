using System.Data;

namespace Customer.Api.Infrastructure.Data;

public interface IDbConnectionProvider
{
    Task<IDbConnection> GetConnection();
}