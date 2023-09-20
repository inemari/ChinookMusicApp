# Chinook Music App

The Chinook Music App is a .NET application that provides functionality for managing a music store's customer data and performing various queries on the Chinook database.

## Features

- **Customer Management:** View, add, update, and delete customer records.

- **Customer Queries:** Perform various queries on the customer data, including retrieving customer information by ID, name, and country, and paginating through the customer list.

- **Statistics:** Retrieve statistics about the customer data, such as the number of customers in each country and the highest-spending customers.

- **Genre Analysis:** Find the most popular music genres among customers and identify customers with the same most popular genres.

## Getting Started

### Prerequisites

Before running the Chinook Music App, you need the following:

- .NET SDK (version X.X.X): You can download it from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet).

- SQL Server Management Studio: You can download it from [docs.microsoft.com](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms).

- Chinook Database: The application assumes that you have the Chinook database set up in your SQL Server instance. You can download the database script from [github.com/lerocha/chinook-database](https://github.com/lerocha/chinook-database).

### Installation

1. Clone this repository to your local machine:
   ```git clone https://github.com/yourusername/chinook-music-app.git```
2. Open the solution in Visual Studio or your preferred .NET IDE.

3. Configure the database connection string in the CustomerRepositoryImpl constructor to point to your SQL Server instance and the Chinook database:
<br>```_connectionString = "Server=your-server-name; Database=Chinook;Integrated Security=True;";```

4. Build and run the application.

### Usage
<b>Customer Management</b>
Use the application to view, add, update, and delete customer records.<br>
<b>Customer Queries</b>
Perform various queries on customer data, such as retrieving customer information by ID, name, and country.<br>
<b>Statistics</b>
Retrieve statistics about customer data, including the number of customers in each country and the highest-spending customers.<br>
<b>Genre Analysis</b>
Find the most popular music genres among customers and identify customers with the same most popular genres.<br>
<b>Contributing</b>
Contributions are welcome! If you have suggestions or improvements, please open an issue or submit a pull request.
