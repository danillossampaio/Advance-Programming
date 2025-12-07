using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using GymProjectApp.Models;

namespace GymProjectApp.DAL
{
    /// <summary>
    /// Repository class for managing Member entities in the SQLite database.
    /// Provides CRUD operations and ensures the Members table exists.
    /// </summary>
    public class MemberRepository
    {
        private const string DbFile = @"C:\Users\danil\Documents\DBS Modules\ADVANCED PROGRAMMING\CA\GymProjectApp\Database\gym.db";

        /// <summary>
        /// Initializes a new instance of the MemberRepository class.
        /// Ensures the Members table exists in the database.
        /// </summary>
        public MemberRepository()
        {
            Console.WriteLine($"[MemberRepository] Using database: {DbFile}");
            EnsureTablesExist();
        }

        /// <summary>
        /// Inserts a new member into the Members table.
        /// </summary>
        /// <param name="m">The Member object to insert.</param>
        public void Insert(Member m)
        {
            if (m == null || string.IsNullOrWhiteSpace(m.Name) || m.Age <= 0)
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
                Console.WriteLine("Member inserted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting member: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing member in the Members table.
        /// </summary>
        /// <param name="m">The Member object with updated values.</param>
        public void Update(Member m)
        {
            if (m == null || m.MemberID <= 0)
            {
                Console.WriteLine("Invalid update (missing ID).");
                return;
            }

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

                var rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Member updated." : "No rows affected (check the ID).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating member: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a member from the Members table by ID.
        /// </summary>
        /// <param name="id">The ID of the member to delete.</param>
        public void Delete(int id)
        {
            if (id <= 0)
            {
                Console.WriteLine("Invalid ID for deletion.");
                return;
            }

            try
            {
                using var connection = new SqliteConnection($"Data Source={DbFile}");
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM Members WHERE MemberID=@id;";
                cmd.Parameters.AddWithValue("@id", id);

                var rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Member deleted." : "No rows affected (check the ID).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting member: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all members from the Members table.
        /// </summary>
        /// <returns>A list of Member objects.</returns>
        public List<Member> GetAll()
        {
            var list = new List<Member>();

            try
            {
                using var connection = new SqliteConnection($"Data Source={DbFile}");
                connection.Open();

                using var cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT MemberID, Name, Age, MembershipType, JoinDate FROM Members;";

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var membershipText = reader.GetString(3);
                    var joinDateText = reader.GetString(4);

                    var parsedType = Enum.TryParse(membershipText, out MembershipType type) ? type : MembershipType.Monthly;
                    var parsedDate = DateTime.TryParse(joinDateText, out var dt) ? dt : DateTime.MinValue;

                    list.Add(new Member
                    {
                        MemberID = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Age = reader.GetInt32(2),
                        MembershipType = parsedType,
                        JoinDate = parsedDate
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving members: {ex.Message}");
            }

            return list;
        }

        /// <summary>
        /// Ensures the Members table exists in the database.
        /// Creates the table if it does not already exist.
        /// </summary>
        private void EnsureTablesExist()
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

                Console.WriteLine("Members table ensured.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ensuring schema: {ex.Message}");
            }
        }
    }
}