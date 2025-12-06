using System;
using System.Collections.Generic;
using GymProjectApp.Models;
using GymProjectApp.BLL;

namespace GymProjectApp
{
    class Program
    {
        static void Main()
        {
            var manager = new MemberManager();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Gym Project Management ===");
                Console.WriteLine("1. Add Member");
                Console.WriteLine("2. Update Member");
                Console.WriteLine("3. Delete Member");
                Console.WriteLine("4. Show All Members");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": Add(manager); break;
                    case "2": Update(manager); break;
                    case "3": Delete(manager); break;
                    case "4": Show(manager); break;
                    case "0": return;
                    default:
                        PrintError("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Adds a new member to the system with validation.
        /// </summary>
        static void Add(MemberManager manager)
        {
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? string.Empty;

            Console.Write("Age: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                PrintError("Age must be a valid number.");
                return;
            }

            Console.Write("Membership Type (Monthly, Quarterly, SemiAnnual, Annual): ");
            var typeInput = Console.ReadLine();
            if (!Enum.TryParse(typeInput, true, out MembershipType type))
            {
                PrintError("Invalid membership type. Accepted values: Monthly, Quarterly, SemiAnnual, Annual.");
                return;
            }

            var joinDate = DateTime.Now;

            var ok = manager.Add(new Member
            {
                Name = name,
                Age = age,
                MembershipType = type,
                JoinDate = joinDate
            });

            if (ok) PrintSuccess("Member was added successfully.");
            else PrintError("Member could not be added. Please check the data and try again.");
        }

        /// <summary>
        /// Updates an existing member.
        /// </summary>
        static void Update(MemberManager manager)
        {
            Console.Write("Enter Member ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                PrintError("ID must be a valid number.");
                return;
            }

            Console.Write("New Name: ");
            var name = Console.ReadLine() ?? string.Empty;

            Console.Write("New Age: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                PrintError("Age must be a valid number.");
                return;
            }

            Console.Write("New Membership Type (Monthly, Quarterly, SemiAnnual, Annual): ");
            var typeInput = Console.ReadLine();
            if (!Enum.TryParse(typeInput, true, out MembershipType type))
            {
                PrintError("Invalid membership type. Accepted values: Monthly, Quarterly, SemiAnnual, Annual.");
                return;
            }

            var joinDate = DateTime.Now;

            var ok = manager.Update(new Member
            {
                MemberID = id,
                Name = name,
                Age = age,
                MembershipType = type,
                JoinDate = joinDate
            });

            if (ok) PrintSuccess("Member was updated successfully.");
            else PrintError("Update failed. Member not found or invalid data.");
        }

        /// <summary>
        /// Deletes a member by ID.
        /// </summary>
        static void Delete(MemberManager manager)
        {
            Console.Write("Enter Member ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                PrintError("ID must be a valid number.");
                return;
            }

            var ok = manager.Delete(id);
            if (ok) PrintSuccess("Member was deleted successfully.");
            else PrintError("Delete failed. Member not found.");
        }

        /// <summary>
        /// Displays all members.
        /// </summary>
        static void Show(MemberManager manager)
        {
            var members = manager.GetAll();
            if (members.Count == 0)
            {
                PrintInfo("No members found in the database.");
                return;
            }

            PrintInfo("Here are the registered members:");
            Console.WriteLine("\nID\tName\tAge\tMembership\tJoinDate");
            foreach (var m in members)
            {
                Console.WriteLine($"{m.MemberID}\t{m.Name}\t{m.Age}\t{m.MembershipType}\t{m.JoinDate:yyyy-MM-dd}");
            }
        }

        // ===========================
        // Helper methods for messages
        // ===========================

        /// <summary>
        /// Prints a success message.
        /// </summary>
        private static void PrintSuccess(string message)
        {
            Console.WriteLine($"\n{message}\n");
        }

        /// <summary>
        /// Prints an error message.
        /// </summary>
        private static void PrintError(string message)
        {
            Console.WriteLine($"\n{message}\n");
        }

        /// <summary>
        /// Prints an informational message.
        /// </summary>
        private static void PrintInfo(string message)
        {
            Console.WriteLine($"\n{message}\n");
        }
    }
}