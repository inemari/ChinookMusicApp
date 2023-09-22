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

- .NET SDK (version X.X.X): You can download it from<br> [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet).

- SQL Server Management Studio: You can download it from <br>[docs.microsoft.com](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms).

- Chinook Database: The application assumes that you have the Chinook database set up in your SQL Server instance. You can download the database script from<br> [github.com/lerocha/chinook-database](https://github.com/lerocha/chinook-database).

- Database setup:

  1. Create a database named SuperheroesDb using SQL Server Management Studio. You can use the script in 01_dbCreate.sql for this purpose.<br>

  2. Create the necessary tables for the Superheroes database using the script in 02_tableCreate.sql.<br>

  3. Set up the relationships between tables as described in 03_relationshipSuperheroAssistant.sql and 04_relationshipSuperheroPower.sql.<br>

  4. Insert data into the database using the provided scripts: 05_insertSuperheroes.sql, 06_insertAssistants.sql, 07_powers.sql, 08_updateSuperhero.sql, and 09_deleteAssistant.sql

### Installation

1. Clone this repository to your local machine:<br>
   ```git clone https://github.com/yourusername/chinook-music-app.git```
2. Open the solution in Visual Studio or your preferred .NET IDE.

3. Configure the database connection string in the CustomerRepositoryImpl constructor to point to your SQL Server instance and the Chinook database:
<br>```_connectionString = "Server=your-server-name; Database=Chinook;Integrated Security=True;";```

4. Build and run the application.

### Usage
#### Customer Management
Use the application to view, add, update, and delete customer records.<br>
#### Customer Queries
Perform various queries on customer data, such as retrieving customer information by ID, name, and country.<br>
#### Statistics
Retrieve statistics about customer data, including the number of customers in each country and the highest-spending customers.<br>
#### Genre Analysis
Find the most popular music genres among customers and identify customers with the same most popular genres.<br>

### Contributers
- [Noah Høgstøl](https://github.com/Nuuah)<br>
- [Ine Mari Bredesen](https://github.com/inemari)
