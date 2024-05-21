using course_project_filip.Models;
using Npgsql;
using System;
using System.Collections.ObjectModel;

namespace course_project_filip
{
    public class Logs : ObservableCollection<Log>
    {
        public void AddLog(
            int logid,
            string text,
            DateTime timestamp)
        {
            this.Add(new Log
            {
                LogID = logid,
                Text = text,
                Timestamp = timestamp
            });
        }

        public Logs()
        {
            Fill_Logs();
        }
        public ObservableCollection<Log> Fill_Logs()
        {
            this.Clear();

            // Змініть це на ваші дані підключення до PostgreSQL
            string connString = "Host=localhost:5432;Username=postgres;Password=postgres;Database=Course_Project";
            using (NpgsqlConnection npgsql_conn = new NpgsqlConnection(connString))
            {
                npgsql_conn.Open();

                string sql = "SELECT * FROM Logs";

                using (NpgsqlCommand npgsql_cmd = new NpgsqlCommand(sql, npgsql_conn))
                {
                    try
                    {
                        using (NpgsqlDataReader reader = npgsql_cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int logid = reader.GetInt32(reader.GetOrdinal("logid"));
                                string text = reader.GetString(reader.GetOrdinal("text"));
                                DateTime timestamp = reader.GetDateTime(reader.GetOrdinal("timestamp"));

                                AddLog(logid, text, timestamp);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                }
            }

            return this;
        }
    }
}
