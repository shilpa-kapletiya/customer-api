namespace Customer.Api.Infrastructure.Data;

public static class DbConnnectionProviderFactory
{
    public static IDbConnectionProvider GetDbConnectionProvider()
    {
        return new PostgressConnectionProvider("Server=127.0.0.1;Port=5433;Pooling=true;Database=shilpa-db;User Id=postgres;Password=password;");
    }
}

