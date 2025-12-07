How to Run the GymProjectApp C# Repository
1. Pre-requisites
- .NET SDK installed
- GitHub account
- Visual Studio Code or Visual Studio
- SQLite Viewer extension (for VS Code) to inspect the products.db file

2. Clone the Repository
git clone https://github.com/danillosampaio/Advance-Programming.git
cd Advance-Programming/GymProjectApp


3. Run the C# Project
dotnet run

- The project uses a local SQLite database (products.db) located in the Database folder.
- Business logic is handled in the GymProjectApp.BLL layer.
- Data access is managed via the GymProjectApp.DAL layer.
- Models are defined in the Models folder.
- The console interface is launched via Program.cs.
