using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using GymProjectApp.Models;

namespace GymProjectApp.DAL
{
    public class ProductRepository
    {
        private const string DbFile = @"C:\Users\danil\Documents\DBS Modules\ADVANCED PROGRAMMING\CA\GymProjectApp\Database\gym.db";

        public ProductRepository()
        {
            Console.WriteLine($"[ProductRepository] Usando banco: {DbFile}");
            EnsureTablesExist();
        }

        public void Insert(Product p)
        {
            if (p == null || string.IsNullOrWhiteSpace(p.Name) || p.Price <= 0 || p.Stock < 0)
            {
                Console.WriteLine("Dados inválidos para produto.");
                return;
            }

            try
            {
                using var connection = new SqliteConnection($"Data Source={DbFile}");
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO Products (Name, Price, Category, Stock)
                    VALUES (@n, @p, @c, @s);";
                cmd.Parameters.AddWithValue("@n", p.Name);
                cmd.Parameters.AddWithValue("@p", p.Price);
                cmd.Parameters.AddWithValue("@c", p.Category ?? "");
                cmd.Parameters.AddWithValue("@s", p.Stock);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Produto inserido com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir produto: {ex.Message}");
            }
        }

        public void Update(Product p)
        {
            if (p == null || p.ProductID <= 0)
            {
                Console.WriteLine("Atualização inválida (ID ausente).");
                return;
            }

            try
            {
                using var connection = new SqliteConnection($"Data Source={DbFile}");
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    UPDATE Products
                    SET Name=@n, Price=@p, Category=@c, Stock=@s
                    WHERE ProductID=@id;";
                cmd.Parameters.AddWithValue("@n", p.Name);
                cmd.Parameters.AddWithValue("@p", p.Price);
                cmd.Parameters.AddWithValue("@c", p.Category ?? "");
                cmd.Parameters.AddWithValue("@s", p.Stock);
                cmd.Parameters.AddWithValue("@id", p.ProductID);

                var rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Produto atualizado." : "Nenhuma linha afetada (verifique o ID).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar produto: {ex.Message}");
            }
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine("ID inválido para exclusão.");
                return;
            }

            try
            {
                using var connection = new SqliteConnection($"Data Source={DbFile}");
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Products WHERE ProductID=@id;";
                cmd.Parameters.AddWithValue("@id", id);

                var rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Produto excluído." : "Nenhuma linha afetada (verifique o ID).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir produto: {ex.Message}");
            }
        }

        public List<Product> GetAll()
        {
            var list = new List<Product>();

            try
            {
                using var connection = new SqliteConnection($"Data Source={DbFile}");
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT ProductID, Name, Price, Category, Stock FROM Products;";

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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao recuperar produtos: {ex.Message}");
            }

            return list;
        }

        private void EnsureTablesExist()
        {
            try
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

                Console.WriteLine("Tabela Products garantida.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao garantir schema: {ex.Message}");
            }
        }
    }
}