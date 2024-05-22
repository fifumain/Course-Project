using Avalonia.Controls.Chrome;
using course_project_filip.Models;
using Npgsql;
using System;
using System.Collections.ObjectModel;

namespace course_project_filip
{
   public class Products : ObservableCollection<Product>
{
    public void AddProduct(
        int ProductId,
        string Title,
        string Category,
        decimal Price,
        int Quantity)
    {
        this.Add(new Product
        {
            ProductId = ProductId,
            Title = Title,
            Category = Category,
            Price = Price,
            Quantity = Quantity
        });
    }

    public Products()
    {
        Fill_Product();
    }

     public ObservableCollection<Product> Fill_Product()
        {
            this.Clear();

            string connString = "Host=localhost:5432;Username=postgres;Password=postgres;Database=Course_Project";
            using (NpgsqlConnection npgsql_conn = new NpgsqlConnection(connString))
            {
                npgsql_conn.Open();

                string sql = "SELECT * FROM Products ORDER BY ProductId";
                
                using (NpgsqlCommand npgsql_cmd = new NpgsqlCommand(sql, npgsql_conn))
                {
                    try
                    {
                        using (NpgsqlDataReader reader = npgsql_cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int productId = reader.GetInt32(reader.GetOrdinal("Productid"));
                                string title = reader.GetString(reader.GetOrdinal("Title"));
                                string category = reader.GetString(reader.GetOrdinal("Category"));
                                decimal price = reader.GetDecimal(reader.GetOrdinal("Price"));
                                int quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));

                                AddProduct(productId, title, category, price, quantity);
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
