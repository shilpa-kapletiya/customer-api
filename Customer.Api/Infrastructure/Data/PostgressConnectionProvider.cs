using Npgsql;
using System.Data;
using System.Threading.Tasks;

namespace Customer.Api.Infrastructure.Data;

public class PostgressConnectionProvider : IDbConnectionProvider
{
    private readonly string _connectionString;

    public PostgressConnectionProvider(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task<IDbConnection> GetConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();
        return connection;
    }
}