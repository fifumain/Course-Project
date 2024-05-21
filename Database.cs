using Npgsql;
using System;

namespace course_project_filip
{
    internal class Database
    {
        public static bool Exec_SQL(string sql)
        {
            bool result = false;

            // Змініть це на ваші дані підключенsня до PostgreSQL
            string connString = "Host=localhost:5432;Username=postgres;Password=postgres;Database=Course_Project";
            using (NpgsqlConnection npgsql_conn = new NpgsqlConnection(connString))
            {
                npgsql_conn.Open();

                using (NpgsqlCommand npgsql_cmd = new NpgsqlCommand(sql, npgsql_conn))
                {
                    try
                    {
                        npgsql_cmd.ExecuteNonQuery();
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        result = false;
                    }
                }
            }

            return result;
        }
    }
}
