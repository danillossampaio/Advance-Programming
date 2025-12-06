using GymProjectApp.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace GymProjectApp.DAL
{
    // Repository class responsible for handling database operations related to Members
    public class MemberRepository
    {
        // SQLite database file name
        private const string DbFile = "gym.db";

        // Constructor initializes the database when the repository is created
        public MemberRepository()
        {
            InitializeDatabase();
        }

        // Creates the Members table if it does not already exist
        private void InitializeDatabase()
        {
            try
            {
                using var connection = new SqliteConnection($"Data Source={DbFile}");
                connection.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Members (
                        MemberID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Age INTEGER NOT NULL,
                        MembershipType TEXT NOT NULL,
                        JoinDate TEXT NOT NULL
                    );";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
            }
        }

        // Inserts a new Member into the database
        public void Insert(Member m)
        {
            // Basic validation before inserting
            if (string.IsNullOrWhiteSpace(m.Name) || m.Age <= 0)
            {
                Console.WriteLine("Invalid member data.");
                return;
            }

            try
            {
                using var connection = new SqliteConnection($"Data Source={DbFile}");
                connection.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO Members (Name, Age, MembershipType, JoinDate)
                    VALUES (@n, @a, @mt, @jd);";
                cmd.Parameters.AddWithValue("@n", m.Name);
                cmd.Parameters.AddWithValue("@a", m.Age);
                cmd.Parameters.AddWithValue("@mt", m.MembershipType.ToString());
                cmd.Parameters.AddWithValue("@jd", m.JoinDate.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Insert error: {ex.Message}");
            }
        }

        // Updates an existing Member in the database
        public void Update(Member m)
        {
            try
            {
                using var connection = new SqliteConnection($"Data Source={DbFile}");
                connection.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    UPDATE Members
                    SET Name=@n, Age=@a, MembershipType=@mt, JoinDate=@jd
                    WHERE MemberID=@id;";
                cmd.Parameters.AddWithValue("@n", m.Name);
                cmd.Parameters.AddWithValue("@a", m.Age);
                cmd.Parameters.AddWithValue("@mt", m.MembershipType.ToString());
                cmd.Parameters.AddWithValue("@jd", m.JoinDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@id", m.MemberID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update error: {ex.Message}");
            }
        }

        // Deletes a Member from the database by ID
        public void Delete(int id)
        {
            try
            {
                using var connection = new SqliteConnection($"Data Source={DbFile}");
                connection.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Members WHERE MemberID=@id;";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete error: {ex.Message}");
            }
        }

        // Retrieves all Members from the database
        public List<Member> GetAll()
        {
            var list = new List<Member>();

            try
            {
                using var connection = new SqliteConnection($"Data Source={DbFile}");
                connection.Open();
                using var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Members;";
                using var reader = cmd.ExecuteReader();

                // Read each row and map it to a Member object
                while (reader.Read())
                {
                    list.Add(new Member
                    {
                        MemberID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Age = reader.GetInt32(2),
                        MembershipType = Enum.TryParse(reader.GetString(3), out MembershipType type) ? type : MembershipType.Monthly,
                        JoinDate = DateTime.Parse(reader.GetString(4))
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Retrieval error: {ex.Message}");
            }

            return list;
        }
    }
}