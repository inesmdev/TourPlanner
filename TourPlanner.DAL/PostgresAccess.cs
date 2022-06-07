using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace TourPlanner.DAL
{
    /*
     *  Access to postgres DB via ADO.NET
     */
    public sealed class PostgresAccess 
    {
        private NpgsqlConnection _connection;
        ILogger _logger;
        

        public PostgresAccess(ILogger<PostgresAccess> logger, IConfiguration config)
        {
            _logger = logger;
            
            try
            {
                _connection = new NpgsqlConnection(config.GetConnectionString("Default"));
                _connection.Open();
            }
            catch
            {
                _logger.LogCritical("Error connecting to PostgresDB");
                throw new System.Exception("DB Connection failed");
            }

            _logger.LogInformation("Connection to PostgresDB established.");
        }

        public NpgsqlConnection GetConnection()
        {
            return _connection;
        }
    }
}