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

        public PostgresAccess(ILogger<PostgresAccess> logger)
        {
            _logger = logger;
            
            try
            {
                _connection = new NpgsqlConnection("Host=localhost;Username=docker;Password=pass123;Database=tourplannerdb;Port=5432");
                _connection.Open();

            }
            catch
            {
                _logger.LogError("Error connecting to PostgresDB");
                throw; 
            }

            _logger.LogInformation("Connection to PostgresDB established.");
        }

        public NpgsqlConnection GetConnection()
        {
            return _connection;
        }
    }
}