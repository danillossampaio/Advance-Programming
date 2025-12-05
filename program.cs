using GymProjectApp.BLL;
using System;

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
                default: Console.WriteLine("Invalid choice!"); break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    static void Add(MemberManager manager)
    {
        Console.Write("Name: ");
        var name = Console.ReadLine();
        Console.Write("Age: ");
        var age = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Membership Type: ");
        var type = Console.ReadLine();
        var joinDate = DateTime.Now;

        manager.Add(new Member { Name = name, Age = age, MembershipType = type, JoinDate = joinDate });
        Console.WriteLine("Member added!");
    }

    static void Update(MemberManager manager)
    {
        Console.Write("Enter Member ID to update: ");
        var id = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("New Name: ");
        var name = Console.ReadLine();
        Console.Write("New Age: ");
        var age = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("New Membership Type: ");
        var type = Console.ReadLine();
        var joinDate = DateTime.Now;

        manager.Update(new Member { MemberID = id, Name = name, Age = age, MembershipType = type, JoinDate = joinDate });
        Console.WriteLine("Member updated!");
    }

    static void Delete(MemberManager manager)
    {
        Console.Write("Enter Member ID to delete: ");
        var id = int.Parse(Console.ReadLine() ?? "0");
        manager.Delete(id);
        Console.WriteLine("Member deleted!");
    }

    static void Show(MemberManager manager)
    {
        var members = manager.GetAll();
        if (members.Count == 0)
        {
            Console.WriteLine("No members found.");
            return;
        }

        Console.WriteLine("\nID\tName\tAge\tMembership\tJoinDate");
        foreach (var m in members)
            Console.WriteLine($"{m.MemberID}\t{m.Name}\t{m.Age}\t{m.MembershipType}\t{m.JoinDate.ToShortDateString()}");
    }
}
