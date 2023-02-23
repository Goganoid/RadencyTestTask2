using System.Data;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace Persistence;



public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("PostgresConnection") ?? throw new ArgumentException("Connection string was not found");
    }
    public IDbConnection CreateConnection()
        => new NpgsqlConnection(_connectionString);
}
