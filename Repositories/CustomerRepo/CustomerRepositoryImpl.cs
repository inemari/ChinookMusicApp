using ChinookMusicApp.Models;
using System.Data.SqlClient;

namespace ChinookMusicApp.Repositories.CustomerRepo
{
    public class CustomerRepositoryImpl : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepositoryImpl()
        {
            // Connection string for database connection.
            _connectionString = "Server=N-NO-01-05-6689\\SQLEXPRESS;Database=Chinook;Integrated Security=True;";
        }

        // Retrieves all customers from the database.
        public List<Customer> GetAll()
        {
            string sqlQuery = "SELECT * FROM Customer";
            return GetCustomersByQuery(sqlQuery);
        }

        // Retrieves a customer by their ID.
        public Customer GetById(int id)
        {
            string sqlQuery = "SELECT * FROM Customer WHERE CustomerId = @CustomerId";
            var parameters = new SqlParameter[] { new SqlParameter("@CustomerId", id) };
            return GetCustomersByQuery(sqlQuery, parameters).FirstOrDefault();
        }

        // Retrieves a customer by their name.
        public Customer GetByName(string name)
        {
            string sqlQuery = "SELECT * FROM Customer WHERE FirstName LIKE @Name";
            var parameters = new SqlParameter("@Name", $"%{name}%");
            return GetCustomersByQuery(sqlQuery, parameters).FirstOrDefault();
        }

        // Retrieves a paginated list of customers.
        public List<Customer> GetPage(int limit, int offset)
        {
            int startIndex = offset * limit;
            string sqlQuery = $"SELECT * FROM Customer ORDER BY CustomerId OFFSET {startIndex} ROWS FETCH NEXT {limit} ROWS ONLY";
            return GetCustomersByQuery(sqlQuery);
        }

        // Adds a new customer to the database.
        public void Add(Customer customer)
        {
            string sqlQuery = "INSERT INTO Customer (FirstName, LastName, Country, PostalCode, Phone, Email) " +
                             "VALUES (@FirstName, @LastName, @Country, @PostalCode, @Phone, @Email)";
            var parameters = CreateCustomerParameters(customer);
            ExecuteNonQuery(sqlQuery, parameters);
        }

        // Updates an existing customer in the database.
        public void Update(Customer customer)
        {
            string sqlQuery = "UPDATE Customer " +
                             "SET FirstName = @FirstName, " +
                             "    LastName = @LastName, " +
                             "    Country = @Country, " +
                             "    PostalCode = @PostalCode, " +
                             "    Phone = @Phone, " +
                             "    Email = @Email " +
                             "WHERE CustomerId = @CustomerId";
            var parameters = CreateCustomerParameters(customer);
            ExecuteNonQuery(sqlQuery, parameters);
        }

        // Retrieves the count of customers in each country.
        public List<CustomerCountry> GetCustomerCountByCountry()
        {
            string sqlQuery = "SELECT Country, COUNT(*) AS CustomerCount\r\nFROM Customer\r\nGROUP BY Country\r\nORDER BY CustomerCount DESC;";
            List<CustomerCountry> customersByCountry = new List<CustomerCountry>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string country = reader["Country"].ToString();
                            int customerCount = Convert.ToInt32(reader["CustomerCount"]);

                            customersByCountry.Add(new CustomerCountry
                            {
                                Country = country,
                                CustomerCount = customerCount
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return customersByCountry;
        }

        // Retrieves the highest spending customers.
        public List<CustomerSpendings> GetHighestSpendingCustomers()
        {
            string sqlQuery = "SELECT c.CustomerId, c.FirstName, c.LastName," +
                "SUM(i.Total) AS TotalSpending " +
                "FROM Customer c " +
                "JOIN Invoice i ON c.CustomerId = i.CustomerId " +
                "GROUP BY c.CustomerId, c.FirstName, c.LastName " +
                "ORDER BY TotalSpending DESC;";

            // Call the existing method to execute the query and get the highest spending customers
            return GetCustomerSpendingsByQuery(sqlQuery);
        }

        // Retrieves the most popular genres for a given customer.
        public List<CustomerGenre> GetMostPopularGenresByCustomer(int customerId)
        {
            string sqlQuery =
                "WITH GenreCounts AS (SELECT c.CustomerId, c.FirstName, c.LastName, g.Name AS GenreName," +
                "COUNT(t.TrackId) AS TrackCount" +
                "FROM Customer c" +
                "JOIN Invoice i ON c.CustomerId = i.CustomerId" +
                "JOIN InvoiceLine il ON i.InvoiceId = il.InvoiceId " +
                "JOIN Track t ON il.TrackId = t.TrackId" +
                "JOIN Genre g ON t.GenreId = g.GenreId " +
                "WHERE c.CustomerId = @CustomerId " +
                "GROUP BY c.CustomerId, c.FirstName, c.LastName, g.Name)" +
                "SELECT CustomerId,  FirstName, LastName,  STRING_AGG(GenreName, ', ') " +
                "WITHIN GROUP (ORDER BY TrackCount DESC) " +
                "AS MostPopularGenre" +
                "FROM GenreCounts" +
                "WHERE TrackCount = (SELECT MAX(TrackCount) " +
                "FROM GenreCounts)" +
                "GROUP BY CustomerId, FirstName, LastName;";

            var parameters = new SqlParameter("@CustomerId", customerId);

            return GetCustomerGenresByQuery(sqlQuery, parameters);
        }

        // Method to execute a query and retrieve customer data.
        private List<Customer> GetCustomersByQuery(string sqlQuery, params SqlParameter[] parameters)
        {
            List<Customer> customerList = new List<Customer>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddRange(parameters);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Customer customer = ReadCustomerFromReader(reader);
                                customerList.Add(customer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return customerList;
        }

        // Method to read customer data from the database.
        private Customer ReadCustomerFromReader(SqlDataReader reader)
        {
            return new Customer(
                Convert.ToInt32(reader["CustomerId"]),
                reader["FirstName"].ToString(),
                reader["LastName"].ToString(),
                reader["Country"].ToString(),
                reader["PostalCode"].ToString(),
                reader["Phone"].ToString(),
                reader["Email"].ToString()
            );
        }

        // Method to execute a query and retrieve customer spendings data.
        private List<CustomerSpendings> GetCustomerSpendingsByQuery(string sqlQuery, params SqlParameter[] parameters)
        {
            List<CustomerSpendings> customerSpendingsList = new List<CustomerSpendings>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddRange(parameters);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomerSpendings customerSpendings = ReadCustomerSpendingsFromReader(reader);
                                customerSpendingsList.Add(customerSpendings);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return customerSpendingsList;
        }

        // Method to read customer spendings data from the database.
        private CustomerSpendings ReadCustomerSpendingsFromReader(SqlDataReader reader)
        {
            return new CustomerSpendings
            {
                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                TotalSpending = Convert.ToDecimal(reader["TotalSpending"])
            };
        }

        // Method to execute a query and retrieve customer genre data.
        private List<CustomerGenre> GetCustomerGenresByQuery(string sqlQuery, params SqlParameter[] parameters)
        {
            List<CustomerGenre> customerGenresList = new List<CustomerGenre>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddRange(parameters);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomerGenre customerGenre = ReadCustomerGenreFromReader(reader);
                                customerGenresList.Add(customerGenre);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return customerGenresList;
        }

        // Method to read customer genre data from the database.
        private CustomerGenre ReadCustomerGenreFromReader(SqlDataReader reader)
        {
            return new CustomerGenre
            {
                CustomerId = Convert.ToInt32(reader["CustomerId"]),
                FirstName = reader["FirstName"].ToString(),
                LastName = reader["LastName"].ToString(),
                MostPopularGenre = reader["MostPopularGenre"].ToString()
            };
        }

        // Method to execute a non-query SQL statement.
        private void ExecuteNonQuery(string sqlQuery, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddRange(parameters);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Method to create SqlParameter array for customer data.
        private SqlParameter[] CreateCustomerParameters(Customer customer)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@FirstName", customer.FirstName),
                new SqlParameter("@LastName", customer.LastName),
                new SqlParameter("@Country", customer.Country),
                new SqlParameter("@PostalCode", customer.PostalCode),
                new SqlParameter("@Phone", customer.Phone),
                new SqlParameter("@Email", customer.Email)
            };

            return parameters;
        }

        // Method to display one customer.
        public void DisplayCustomer(Customer customer)
        {
            Console.WriteLine($"Customer ID: {customer.CustomerId}");
            Console.WriteLine($"First Name: {customer.FirstName}");
            Console.WriteLine($"Last Name: {customer.LastName}");
            Console.WriteLine($"Country: {customer.Country}");
            Console.WriteLine($"Postal Code: {customer.PostalCode}");
            Console.WriteLine($"Phone: {customer.Phone}");
            Console.WriteLine($"Email: {customer.Email}");
            Console.WriteLine();
        }

        // Method to display a list of customers.
        public void DisplayCustomers(List<Customer> customers)
        {
            foreach (var customer in customers)
            {
                DisplayCustomer(customer);
            }
        }
    }
}
