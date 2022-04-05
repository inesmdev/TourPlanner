using Npgsql;
using System;
using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.DAL
{
    public class TourRepository : IRepository<Tour>, IDisposable
    {      
        private NpgsqlConnection conn;
        private const string TABLE_NAME = "tour";

        public TourRepository()
        {
            PostgresAccess db = PostgresAccess.Instance;
            conn = db.GetConnection();
            conn.MapEnum<EnumTransportType>("transporttype"); //?
            conn.ReloadTypes();
        }

        // 2nd Constructor for Unit Testing?

        public bool Create(Tour tour)
        {
            try
            {
                using (var command = conn.CreateCommand())
                {
                    string sql = $@"INSERT INTO {TABLE_NAME} 
                                (tour_id, tour_name, description, creation_date, estimated_time, distance, tour_from, tour_to, transport_type) 
                                VALUES (@tour_name, @description, @creation_date, @estimated_time, @distance, @tour_from, @tour_to, @transport_type)";

                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@tour_name", tour.TourId);
                    command.Parameters.AddWithValue("@description", tour.Tourname);
                    command.Parameters.AddWithValue("@creation_date", tour.CreationDate);
                    command.Parameters.AddWithValue("@estimated_time", tour.EstimatedTime);
                    command.Parameters.AddWithValue("@distance", tour.Distance);
                    command.Parameters.AddWithValue("@tour_from", tour.From);
                    command.Parameters.AddWithValue("@tour_to", tour.To);
                    command.Parameters.AddWithValue("@transport_type", tour.TransportType);

                    int affectedRows = command.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(Tour tour)
        {
            using (var command = conn.CreateCommand())
            {
                string sql = $"DELETE FROM {TABLE_NAME} WHERE tour_id=@tour_id;";

                command.CommandText = sql;
                command.Parameters.AddWithValue("@tour_id", tour.TourId);

                return command.ExecuteNonQuery() > 0 ? true : false;
            }
        }
   
        public IEnumerable<Tour> GetAll(/*Tour criteria*/)
        {
            List<Tour> tours = new();

            using(var command = conn.CreateCommand())
            {
                string sql = $"SELECT *  FROM {TABLE_NAME};";
                command.CommandText = sql;

                using NpgsqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tours.Add(new Tour {
                        TourId = Guid.Parse(reader.GetString(0)), 
                        Tourname = reader.GetString(1),
                        Description = reader.GetString(2),
                        CreationDate = reader.GetDateTime(3),
                        EstimatedTime = reader.GetInt32(4),
                        Distance = reader.GetDouble(5),
                        From = new Location { Street = reader.GetString(6)}, // !!
                        To = new Location { Street = reader.GetString(7)},
                        TransportType = reader.GetFieldValue<EnumTransportType>(8)
                    }
                    );
                }
            }

            return tours;
        }

        public Tour GetByID(Guid id)
        {
            using (var command = conn.CreateCommand())
            {
                string sql = $"SELECT *  FROM {TABLE_NAME} WHERE tour_id = @tour_id;";
                command.CommandText = sql;
                command.Parameters.AddWithValue("@tour_id", id);

                using NpgsqlDataReader reader = command.ExecuteReader();

                reader.Read();

                Tour tour = new Tour
                {
                    TourId = reader.GetGuid(0),
                    Tourname = reader.GetString(1),
                    Description = reader.GetString(2),
                    CreationDate = reader.GetDateTime(3),
                    EstimatedTime = reader.GetInt32(4),
                    Distance = reader.GetFloat(5),
                    From = new Location { Street = reader.GetString(6) }, // !!
                    To = new Location { Street = reader.GetString(7) },
                    TransportType = (EnumTransportType)reader.GetValue(8)  // !!
                };

                return tour;
            }
        }

        public bool Update(Tour tour)
        {
            using (var command = conn.CreateCommand())
            {
                string sql = $"UPDATE {TABLE_NAME} " +
                             $"SET tour_name=@tour_name, description=@description, estimated_time=@estimated_time" +
                             $"distance=@distance, tour_from=@tour_from, tour_to=@tour_to, transport_type=@transport_type " +
                             $"WHERE tour_id=@tour_id;";

                command.CommandText = sql;
                command.Parameters.AddWithValue("@tour_name", tour.TourId);
                command.Parameters.AddWithValue("@description", tour.Tourname);
                command.Parameters.AddWithValue("@estimated_time", tour.EstimatedTime);
                command.Parameters.AddWithValue("@distance", tour.Distance);
                command.Parameters.AddWithValue("@tour_from", tour.From);
                command.Parameters.AddWithValue("@tour_to", tour.To);
                command.Parameters.AddWithValue("@transport_type", tour.TransportType);
                command.Parameters.AddWithValue("@tour_id", tour.TourId);

                return command.ExecuteNonQuery() > 0 ? true : false;
            }
        }

        /*
         *  IDisposeable
         */
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    conn.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //???
        }
    }
}
