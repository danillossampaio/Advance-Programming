using GymProjectApp.BLL;
using GymProjectApp.DAL;
using GymProjectApp.Models;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Instanciar os managers
var memberManager = new MemberManager(new MemberRepository());
var productManager = new ProductManager(new ProductRepository());

while (true)
{
    Console.WriteLine("\n=== Gym Project Management ===");
    Console.WriteLine("1. Add Member");
    Console.WriteLine("2. Update Member");
    Console.WriteLine("3. Delete Member");
    Console.WriteLine("4. Show All Members");
    Console.WriteLine("5. Add Product");
    Console.WriteLine("6. Update Product");
    Console.WriteLine("7. Delete Product");
    Console.WriteLine("8. Show All Products");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1": AddMember(memberManager); break;
        case "2": UpdateMember(memberManager); break;
        case "3": DeleteMember(memberManager); break;
        case "4": ShowMembers(memberManager); break;
        case "5": AddProduct(productManager); break;
        case "6": UpdateProduct(productManager); break;
        case "7": DeleteProduct(productManager); break;
        case "8": ShowProducts(productManager); break;
        case "0": return;
        default: PrintError("Invalid choice. Please try again."); break;
    }
}

// MEMBER METHODS
static void AddMember(MemberManager manager)
{
    Console.Write("Name: ");
    var name = Console.ReadLine() ?? string.Empty;

    Console.Write("Age: ");
    if (!int.TryParse(Console.ReadLine(), out int age))
    {
        PrintError("Age must be a valid number.");
        return;
    }

    Console.WriteLine("Membership Type (Monthly, Quarterly, SemiAnnual, Annual): ");
    var typeInput = Console.ReadLine();
    if (!Enum.TryParse(typeInput, out MembershipType type))
    {
        PrintError("Invalid membership type.");
        return;
    }

    var member = new Member
    {
        Name = name,
        Age = age,
        MembershipType = type,
        JoinDate = DateTime.Now
    };

    var ok = manager.Add(member);
    if (ok) PrintSuccess("Member added successfully.");
    else PrintError("Failed to add member.");
}

static void UpdateMember(MemberManager manager)
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

    Console.WriteLine("New Membership Type (Monthly, Quarterly, SemiAnnual, Annual): ");
    var typeInput = Console.ReadLine();
    if (!Enum.TryParse(typeInput, out MembershipType type))
    {
        PrintError("Invalid membership type.");
        return;
    }

    var member = new Member
    {
        MemberID = id,
        Name = name,
        Age = age,
        MembershipType = type,
        JoinDate = DateTime.Now
    };

    var ok = manager.Update(member);
    if (ok) PrintSuccess("Member updated successfully.");
    else PrintError("Failed to update member.");
}

static void DeleteMember(MemberManager manager)
{
    Console.Write("Enter Member ID to delete: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        PrintError("ID must be a valid number.");
        return;
    }

    var ok = manager.Delete(id);
    if (ok) PrintSuccess("Member deleted successfully.");
    else PrintError("Failed to delete member.");
}

static void ShowMembers(MemberManager manager)
{
    var members = manager.GetAll();
    if (members.Count == 0)
    {
        PrintInfo("No members found.");
        return;
    }

    PrintInfo("Registered Members:");
    Console.WriteLine("\nID\tName\tAge\tType\tJoin Date");
    foreach (var m in members)
    {
        Console.WriteLine($"{m.MemberID}\t{m.Name}\t{m.Age}\t{m.MembershipType}\t{m.JoinDate:d}");
    }
}

// PRODUCT METHODS
static void AddProduct(ProductManager manager)
{
    Console.Write("Product Name: ");
    var name = Console.ReadLine() ?? string.Empty;

    Console.Write("Price: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal price))
    {
        PrintError("Price must be a valid number.");
        return;
    }

    Console.Write("Category: ");
    var category = Console.ReadLine() ?? string.Empty;

    Console.Write("Stock Quantity: ");
    if (!int.TryParse(Console.ReadLine(), out int stock))
    {
        PrintError("Stock must be a valid number.");
        return;
    }

    var ok = manager.Add(new Product
    {
        Name = name,
        Price = price,
        Category = category,
        Stock = stock
    });

    if (ok) PrintSuccess("Product added successfully.");
    else PrintError("Failed to add product.");
}

static void UpdateProduct(ProductManager manager)
{
    Console.Write("Enter Product ID to update: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        PrintError("ID must be a valid number.");
        return;
    }

    Console.Write("New Name: ");
    var name = Console.ReadLine() ?? string.Empty;

    Console.Write("New Price: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal price))
    {
        PrintError("Price must be a valid number.");
        return;
    }

    Console.Write("New Category: ");
    var category = Console.ReadLine() ?? string.Empty;

    Console.Write("New Stock Quantity: ");
    if (!int.TryParse(Console.ReadLine(), out int stock))
    {
        PrintError("Stock must be a valid number.");
        return;
    }

    var ok = manager.Update(new Product
    {
        ProductID = id,
        Name = name,
        Price = price,
        Category = category,
        Stock = stock
    });

    if (ok) PrintSuccess("Product updated successfully.");
    else PrintError("Failed to update product.");
}

static void DeleteProduct(ProductManager manager)
{
    Console.Write("Enter Product ID to delete: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        PrintError("ID must be a valid number.");
        return;
    }

    var ok = manager.Delete(id);
    if (ok) PrintSuccess("Product deleted successfully.");
    else PrintError("Failed to delete product.");
}

static void ShowProducts(ProductManager manager)
{
    var products = manager.GetAll();
    if (products.Count == 0)
    {
        PrintInfo("No products found.");
        return;
    }

    PrintInfo("Registered Products:");
    Console.WriteLine("\nID\tName\tPrice\tCategory\tStock");
    foreach (var p in products)
    {
        Console.WriteLine($"{p.ProductID}\t{p.Name}\t{p.Price:C}\t{p.Category}\t{p.Stock}");
    }
}

// VISUAL METHODS
static void PrintSuccess(string message)
{
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($" {message}");
    Console.ResetColor();
}

static void PrintError(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($" {message}");
    Console.ResetColor();
}

static void PrintInfo(string message)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($" {message}");
    Console.ResetColor();
}