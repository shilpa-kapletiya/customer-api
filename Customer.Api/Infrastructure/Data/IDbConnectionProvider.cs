using System.Data;
using System.Threading.Tasks;

namespace Customer.Api.Infrastructure.Data;

public interface IDbConnectionProvider
{ 
    Task<IDbConnection> GetConnection();
}