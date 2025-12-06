using GymProjectApp.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace GymProjectApp.DAL
{
    public class ProductRepository
    {
        private const string DbFile = "gym.db";

        public ProductRepository()
        {
            using var connection = new SqliteConnection($"Data Source={DbFile}");
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Products (
                    ProductID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL,
                    Category TEXT,
                    Stock INTEGER NOT NULL
                );";
            cmd.ExecuteNonQuery();
        }

        public void Insert(Product p)
        {
            using var connection = new SqliteConnection($"Data Source={DbFile}");
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Products (Name, Price, Category, Stock) VALUES (@n, @p, @c, @s)";
            cmd.Parameters.AddWithValue("@n", p.Name);
            cmd.Parameters.AddWithValue("@p", p.Price);
            cmd.Parameters.AddWithValue("@c", p.Category);
            cmd.Parameters.AddWithValue("@s", p.Stock);
            cmd.ExecuteNonQuery();
        }

        public void Update(Product p)
        {
            using var connection = new SqliteConnection($"Data Source={DbFile}");
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Products SET Name=@n, Price=@p, Category=@c, Stock=@s WHERE ProductID=@id";
            cmd.Parameters.AddWithValue("@n", p.Name);
            cmd.Parameters.AddWithValue("@p", p.Price);
            cmd.Parameters.AddWithValue("@c", p.Category);
            cmd.Parameters.AddWithValue("@s", p.Stock);
            cmd.Parameters.AddWithValue("@id", p.ProductID);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = new SqliteConnection($"Data Source={DbFile}");
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Products WHERE ProductID=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public List<Product> GetAll()
        {
            var list = new List<Product>();
            using var connection = new SqliteConnection($"Data Source={DbFile}");
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Products";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Product
                {
                    ProductID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    Category = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Stock = reader.GetInt32(4)
                });
            }
            return list;
        }
    }
}