using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using TourPlanner.Exceptions;
using TourPlanner.Models;

namespace TourPlanner.DAL.Repositories
{
    public class TourLogRepository : ITourLogRepository
    {
        private NpgsqlConnection conn;
        private const string TABLE_NAME = "tour_log";
        ILogger<TourLogRepository> _logger;

        /*
         *  Constructor
         */
        public TourLogRepository(PostgresAccess db, ILogger<TourLogRepository> logger)
        {
            _logger = logger;
            conn = db.GetConnection();
            conn.TypeMapper.MapEnum<EnumTourRating>("tourrating");
            conn.TypeMapper.MapEnum<EnumTourDifficulty>("tourdifficulty");
        }


        /*
         *  Create new TourLog
         */
        public void Create(TourLog tourlog)
        {
            using (var command = conn.CreateCommand())
            {
                string sql = $@"INSERT INTO {TABLE_NAME} 
                                (tour_log_id, tour_id, date_time, rating, difficulty, total_time, comment) 
                                VALUES (@tour_log_id, @tour_id, @date_time, @rating, @difficulty, @total_time, @comment);";

                command.CommandText = sql;
                command.Parameters.AddWithValue("@tour_log_id", tourlog.Id.ToString("N"));
                command.Parameters.AddWithValue("@tour_id", tourlog.TourId.ToString("N"));
                command.Parameters.AddWithValue("@date_time", tourlog.DateTime);
                command.Parameters.AddWithValue("@rating", tourlog.TourRating);
                command.Parameters.AddWithValue("@difficulty", tourlog.TourDifficulty);
                command.Parameters.AddWithValue("@total_time", tourlog.TotalTime);
                command.Parameters.AddWithValue("@comment", tourlog.Comment);

                int affectedRows = command.ExecuteNonQuery();
                if (affectedRows <= 0)
                {
                    throw new CouldNotCreateTourException("Error creating Tourlog");
                }
            }
        }


        /*
         *  Delete TourLog
         */
        public bool Delete(Guid id)
        {
            using (var command = conn.CreateCommand())
            {
                string sql = $"DELETE FROM {TABLE_NAME} WHERE tour_log_id=@tour_log_id;";

                command.CommandText = sql;
                command.Parameters.AddWithValue("@tour_log_id", id.ToString("N"));

                return command.ExecuteNonQuery() > 0 ? true : false;
            }
        }


        /*
         *  Get all TourLogs
         */
        public IEnumerable<TourLog> GetAll(Guid tourid)
        {
            List<TourLog> tourslogs = new();

            using (var command = conn.CreateCommand())
            {
                string sql = $"SELECT *  FROM {TABLE_NAME} WHERE tour_id=@tour_id;";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@tour_id", tourid.ToString("N"));

                using NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tourslogs.Add(new TourLog()
                    {
                        Id = Guid.Parse(reader.GetString(0)),
                        TourId = Guid.Parse(reader.GetString(1)),
                        DateTime = reader.GetDateTime(2),
                        TourRating = reader.GetFieldValue<EnumTourRating>(3),
                        TourDifficulty = reader.GetFieldValue<EnumTourDifficulty>(4),
                        TotalTime = reader.GetFloat(5),
                        Comment = reader.GetString(6)
                    });
                }
            }

            
            //(tourslogs.ToString());

            return tourslogs;
        }


        /*
         *  Get all TourLogs
         */
        public IEnumerable<TourLog> GetAll()
        {
            List<TourLog> tourslogs = new();

            using (var command = conn.CreateCommand())
            {
                string sql = $"SELECT *  FROM {TABLE_NAME};";
                command.CommandText = sql;

                using NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tourslogs.Add(new TourLog()
                    {
                        Id = Guid.Parse(reader.GetString(0)),
                        TourId = Guid.Parse(reader.GetString(1)),
                        DateTime = reader.GetDateTime(2),
                        TourRating = reader.GetFieldValue<EnumTourRating>(3),
                        TourDifficulty = reader.GetFieldValue<EnumTourDifficulty>(4),
                        TotalTime = reader.GetFloat(5),
                        Comment = reader.GetString(6)
                    });
                }
            }

           // _logger.LogDebug(tourslogs.ToString());

            return tourslogs;
        }


        /*
         *  Get specific TourLog by Id
         */
        public TourLog GetByID(Guid id)
        {
            using (var command = conn.CreateCommand())
            {
                string sql = $"SELECT * FROM {TABLE_NAME} WHERE tour_log_id=@tour_log_id;";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@tour_log_id", id.ToString("N"));

                using NpgsqlDataReader reader = command.ExecuteReader();

                reader.Read();

                TourLog tourlog = new TourLog
                {
                    Id = Guid.Parse(reader.GetString(0)),
                    TourId = Guid.Parse(reader.GetString(1)),
                    DateTime = reader.GetDateTime(2),
                    TourRating = reader.GetFieldValue<EnumTourRating>(3),
                    TourDifficulty = reader.GetFieldValue<EnumTourDifficulty>(4),
                    TotalTime = reader.GetFloat(5),
                    Comment = reader.GetString(6)
                };

                return tourlog;
            }
        }


        /*
         *  Update a TourLog
         */
        public bool Update(TourLog tourlog)
        {
            using (var command = conn.CreateCommand())
            {
                string sql = @$"UPDATE {TABLE_NAME} 
                             SET date_time=@date_time, rating=@rating, difficulty=@difficulty, total_time=@total_time, comment=@comment 
                             WHERE tour_log_id=@tour_log_id;";

               // _logger.LogDebug($"Execute SQL Statement: {sql}");

                command.CommandText = sql;
                command.Parameters.AddWithValue("@date_time", tourlog.DateTime);
                command.Parameters.AddWithValue("@rating", tourlog.TourRating);
                command.Parameters.AddWithValue("@difficulty", tourlog.TourDifficulty);
                command.Parameters.AddWithValue("@total_time", tourlog.TotalTime);
                command.Parameters.AddWithValue("@comment", tourlog.Comment);
                command.Parameters.AddWithValue("@tour_log_id", tourlog.Id.ToString("N"));

                return command.ExecuteNonQuery() > 0 ? true : false;
            }
        }
    }
}
