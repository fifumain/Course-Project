using course_project_filip.Models;
using Npgsql;
using System;
using System.Collections.ObjectModel;

namespace course_project_filip
{
	public class Suppliers : ObservableCollection<Supplier>
	{
		public void AddSupplier(
			int Id,
			string Title,
			string Info)
		{
			this.Add(new Supplier
			{
				Id = Id,
				Title = Title,
				Info = Info,

			});
		}

		public Suppliers()
		{
			FillSupplier();
		}

		public ObservableCollection<Supplier> FillSupplier()
		{
			this.Clear();

			// Змініть це на ваші дані підключення до PostgreSQL
			string connString = "Host=localhost:5432;Username=postgres;Password=postgres;Database=Course_Project";
			using (NpgsqlConnection npgsql_conn = new NpgsqlConnection(connString))
			{
				npgsql_conn.Open();

				string sql = "SELECT * FROM supplier ORDER BY Id";

				using (NpgsqlCommand npgsql_cmd = new NpgsqlCommand(sql, npgsql_conn))
				{
					try
					{
						using (NpgsqlDataReader reader = npgsql_cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								int id = reader.GetInt32(reader.GetOrdinal("Id"));
								string title = reader.GetString(reader.GetOrdinal("Title"));
								string info = reader.GetString(reader.GetOrdinal("Info"));
  

								AddSupplier(id, title, info);
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
