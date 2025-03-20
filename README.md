# Personal Information Management System

A simple ASP.NET MVC application for managing personal information records.

## Setup Instructions

### Prerequisites
- Visual Studio 2019 or newer
- SQL Server (Express or higher)
- .NET Framework 4.7.2 or higher

### Database Setup
1. Open SQL Server Management Studio
2. Connect to your SQL Server instance
3. Create a new database named "PersonalInfoDB"
4. Open the `PersonalInfoManagement/Database/DatabaseSetup.sql` file from this project
5. Execute the script to create tables and add sample data

### Project Setup
1. Open the solution in Visual Studio by double-clicking `PersonalInfoManagement.sln`
2. Right-click on the solution in Solution Explorer and select "Restore NuGet Packages"
3. Update the connection string in `Web.config` with your SQL Server name:
   ```xml
   <connectionStrings>
     <add name="PersonalInfoDbContext" connectionString="Data Source=YourServerName\SQLEXPRESS;Initial Catalog=PersonalInfoDB;Integrated Security=True" providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```
   Replace `YourServerName\SQLEXPRESS` with your SQL Server instance name.

4. Build the solution (Ctrl+Shift+B)
5. Run the project (F5)

## Features
- Create, view, edit and delete personal information records
- Validation for required fields and data formats
- Simple, easy-to-use interface
- Responsive design using Bootstrap 