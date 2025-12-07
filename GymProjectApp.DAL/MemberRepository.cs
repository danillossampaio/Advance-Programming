using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using GymProjectApp.Models;

namespace GymProjectApp.DAL
{
    public class MemberRepository
    {
        private const string DbFile = @"C:\Users\danil\Documents\DBS Modules\ADVANCED PROGRAMMING\CA\GymProjectApp\Database\gym.db";

        public MemberRepository()
        {
            Console.WriteLine($"[MemberRepository] Usando banco: {DbFile}");
            EnsureTablesExist();
        }

        public void Insert(Member m)
        {
            if (m == null || string.IsNullOrWhiteSpace(m.Name) || m.Age <= 0)
            {
                Console.WriteLine("Dados inválidos para membro.");
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
                Console.WriteLine("Membro inserido com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inserir: {ex.Message}");
            }
        }

        public void Update(Member m)
        {
            if (m == null || m.MemberID <= 0)
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
                    UPDATE Members
                    SET Name=@n, Age=@a, MembershipType=@mt, JoinDate=@jd
                    WHERE MemberID=@id;";
                cmd.Parameters.AddWithValue("@n", m.Name);
                cmd.Parameters.AddWithValue("@a", m.Age);
                cmd.Parameters.AddWithValue("@mt", m.MembershipType.ToString());
                cmd.Parameters.AddWithValue("@jd", m.JoinDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@id", m.MemberID);

                var rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Membro atualizado." : "Nenhuma linha afetada (verifique o ID).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar: {ex.Message}");
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
                cmd.CommandText = "DELETE FROM Members WHERE MemberID=@id;";
                cmd.Parameters.AddWithValue("@id", id);

                var rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Membro excluído." : "Nenhuma linha afetada (verifique o ID).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir: {ex.Message}");
            }
        }

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
                Console.WriteLine($"Erro ao recuperar: {ex.Message}");
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
                    CREATE TABLE IF NOT EXISTS Members (
                        MemberID INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Age INTEGER NOT NULL,
                        MembershipType TEXT NOT NULL,
                        JoinDate TEXT NOT NULL
                    );";
                cmd.ExecuteNonQuery();

                Console.WriteLine("Tabela Members garantida.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao garantir schema: {ex.Message}");
            }
        }
    }
}