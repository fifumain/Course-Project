using Avalonia.Controls.Chrome;
using course_project_filip.Models;
using Npgsql;
using System;
using System.Collections.ObjectModel;

namespace course_project_filip
{
   public class Resources : ObservableCollection<Resource>
{
	public void AddResource(
		int ResourceId,
		string Title,
		int Quantity)
	{
		this.Add(new Resource
		{
			ResourceId = ResourceId,
			Title = Title,
			Quantity = Quantity
		});
	}

	public Resources()
	{
		Fill_Resource();
	}

	 public ObservableCollection<Resource> Fill_Resource()
		{
			this.Clear();

			string connString = "Host=localhost:5432;Username=postgres;Password=postgres;Database=Course_Project";
			using (NpgsqlConnection npgsql_conn = new NpgsqlConnection(connString))
			{
				npgsql_conn.Open();

				string sql = "SELECT * FROM Resources ORDER BY ResourceId";
				
				using (NpgsqlCommand npgsql_cmd = new NpgsqlCommand(sql, npgsql_conn))
				{
					try
					{
						using (NpgsqlDataReader reader = npgsql_cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								int ResourceId = reader.GetInt32(reader.GetOrdinal("ResourceId"));
								string title = reader.GetString(reader.GetOrdinal("Title"));
								int quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));

								AddResource(ResourceId, title, quantity);
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
