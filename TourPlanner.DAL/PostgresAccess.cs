using Microsoft.Extensions.Logging;
using Npgsql;
using System;

namespace TourPlanner.DAL
{
    /*
     *  Access to postgres DB via ADO.NET
     *  Singelton, Lazy initialization
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
            catch (System.Exception e)
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