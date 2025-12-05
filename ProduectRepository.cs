using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using GymProjectApp.BLL;
using System;

namespace GymProjectApp.DAL
{
    public class MemberRepository
    {
        private const string DbFile = "gym.db";

        public MemberRepository()
        {
            using var connection = new SqliteConnection($"Data Source={DbFile}");
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Members (
                MemberID INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Age INTEGER,
                MembershipType TEXT,
                JoinDate TEXT
            );";
            cmd.ExecuteNonQuery();
        }

        public void Insert(Member m)
        {
            using var connection = new SqliteConnection($"Data Source={DbFile}");
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Members (Name, Age, MembershipType, JoinDate) VALUES (@n, @a, @mt, @jd)";
            cmd.Parameters.AddWithValue("@n", m.Name);
            cmd.Parameters.AddWithValue("@a", m.Age);
            cmd.Parameters.AddWithValue("@mt", m.MembershipType);
            cmd.Parameters.AddWithValue("@jd", m.JoinDate.ToString("yyyy-MM-dd"));
            cmd.ExecuteNonQuery();
        }

        public void Update(Member m)
        {
            using var connection = new SqliteConnection($"Data Source={DbFile}");
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Members SET Name=@n, Age=@a, MembershipType=@mt, JoinDate=@jd WHERE MemberID=@id";
            cmd.Parameters.AddWithValue("@n", m.Name);
            cmd.Parameters.AddWithValue("@a", m.Age);
            cmd.Parameters.AddWithValue("@mt", m.MembershipType);
            cmd.Parameters.AddWithValue("@jd", m.JoinDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@id", m.MemberID);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = new SqliteConnection($"Data Source={DbFile}");
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Members WHERE MemberID=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public List<Member> GetAll()
        {
            var list = new List<Member>();
            using var connection = new SqliteConnection($"Data Source={DbFile}");
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Members";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Member
                {
                    MemberID = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Age = reader.GetInt32(2),
                    MembershipType = reader.GetString(3),
                    JoinDate = DateTime.Parse(reader.GetString(4))
                });
            }
            return list;
        }
    }
}
