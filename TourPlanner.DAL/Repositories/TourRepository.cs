using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using TourPlanner.Exceptions;
using TourPlanner.Models;

namespace TourPlanner.DAL.Repositories
{
    public class TourRepository : ITourRepository
    {
        private NpgsqlConnection conn;
        private const string TABLE_NAME = "tour";
        ILogger _logger;

        public TourRepository(PostgresAccess db, ILogger<TourRepository> logger)
        {
            conn = db.GetConnection();
            conn.TypeMapper.MapEnum<EnumTransportType>("transporttype"); 
            _logger = logger;
        }


        public void Create(Tour tour)
        {
            using (var command = conn.CreateCommand())
            {
                string sql = $@"INSERT INTO {TABLE_NAME} 
                                (tour_id, tour_name, description, estimated_time, distance, tour_from, tour_to, transport_type) 
                                VALUES (@tour_id, @tour_name, @description, @estimated_time, @distance, @tour_from, @tour_to, @transport_type);";

                command.CommandText = sql;
                command.Parameters.AddWithValue("@tour_id", tour.Id.ToString("N"));
                command.Parameters.AddWithValue("@tour_name", tour.Name);
                command.Parameters.AddWithValue("@description", tour.Description);
                command.Parameters.AddWithValue("@estimated_time", tour.EstimatedTime);
                command.Parameters.AddWithValue("@distance", tour.Distance);
                command.Parameters.AddWithValue("@tour_from", tour.From);
                command.Parameters.AddWithValue("@tour_to", tour.To);
                command.Parameters.AddWithValue("@transport_type", tour.TransportType);

                int affectedRows = command.ExecuteNonQuery();
                if (affectedRows <= 0)
                {
                    throw new CouldNotCreateTourException();
                }
            }
        }


        public bool Delete(Guid id)
        {
            using (var command = conn.CreateCommand())
            {
                string sql = $"DELETE FROM {TABLE_NAME} WHERE tour_id=@tour_id;";

                command.CommandText = sql;
                command.Parameters.AddWithValue("@tour_id", id.ToString("N"));

                return command.ExecuteNonQuery() > 0 ? true : false;
            }
        }


        public IEnumerable<Tour> GetAll()
        {
            List<Tour> tours = new();

            using (var command = conn.CreateCommand())
            {
                string sql = $"SELECT *  FROM {TABLE_NAME};";
                command.CommandText = sql;

                using NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Tour newTour =
                    new Tour
                    {
                        Id = Guid.Parse(reader.GetString(0)),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        CreationDate = reader.GetDateTime(3),
                        EstimatedTime = reader.GetInt32(4),
                        Distance = reader.GetDouble(5),
                        From = reader.GetString(6), 
                        To = reader.GetString(7),
                        TransportType = reader.GetFieldValue<EnumTransportType>(8)
                    };

                    newTour.GenerateSummary();
                    tours.Add(newTour);
                }
            }

            return tours;
        }


        public Tour GetByID(Guid id)
        {
            using (var command = conn.CreateCommand())
            {
                string sql = $"SELECT *  FROM {TABLE_NAME} WHERE tour_id=@tour_id;";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@tour_id", id.ToString("N"));

                using NpgsqlDataReader reader = command.ExecuteReader();

                reader.Read();

                Tour tour = new Tour
                {
                    Id = Guid.Parse(reader.GetString(0)),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    CreationDate = reader.GetDateTime(3),
                    EstimatedTime = reader.GetInt32(4),
                    Distance = reader.GetDouble(5),
                    From = reader.GetString(6), 
                    To = reader.GetString(7),
                    TransportType = (EnumTransportType)reader.GetValue(8) 
                };
                tour.GenerateSummary();
                return tour;
            }
        }


        public bool Update(Tour tour)
        {
            using (var command = conn.CreateCommand())
            {
                string sql = $"UPDATE {TABLE_NAME} " +
                             $"SET tour_name=@tour_name, description=@description, estimated_time=@estimated_time, " +
                             $"distance=@distance, tour_from=@tour_from, tour_to=@tour_to, transport_type=@transport_type " +
                             $"WHERE tour_id=@tour_id;";

                command.CommandText = sql;
                command.Parameters.AddWithValue("@tour_name", tour.Name);
                command.Parameters.AddWithValue("@description", tour.Description);
                command.Parameters.AddWithValue("@estimated_time", tour.EstimatedTime);
                command.Parameters.AddWithValue("@distance", tour.Distance);
                command.Parameters.AddWithValue("@tour_from", tour.From);
                command.Parameters.AddWithValue("@tour_to", tour.To);
                command.Parameters.AddWithValue("@transport_type", tour.TransportType);
                command.Parameters.AddWithValue("@tour_id", tour.Id.ToString("N"));

                return command.ExecuteNonQuery() > 0 ? true : false;
            }
        }
    }
}
