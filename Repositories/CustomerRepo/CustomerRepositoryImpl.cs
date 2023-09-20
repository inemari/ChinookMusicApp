using ChinookMusicApp.Models;
using System.Data.SqlClient;

namespace ChinookMusicApp.Repositories.CustomerRepo
{
    public class CustomerRepositoryImpl : ICustomerRepository
    {
        private readonly string _connectionString = "Server=N-NO-01-05-6689\\SQLEXPRESS;Database=Chinook;Integrated Security=True;";

        public List<Customer> GetAll()
        {
            string sqlQuery = "SELECT * FROM Customer";
            return GetCustomersByQuery(sqlQuery);
        }

        public Customer GetById(int id)
        {
            string sqlQuery = "SELECT * FROM Customer WHERE CustomerId = @CustomerId";
            var parameters = new SqlParameter[] { new SqlParameter("@CustomerId", id) };
            return GetCustomersByQuery(sqlQuery, parameters).FirstOrDefault();
        }

        public Customer GetByName(string name)
        {
            string sqlQuery = "SELECT * FROM Customer WHERE FirstName LIKE @Name";
            var parameters = new SqlParameter("@Name", $"%{name}%");
            return GetCustomersByQuery(sqlQuery, parameters).FirstOrDefault();
        }

        public List<Customer> GetPage(int limit, int offset)
        {
            int startIndex = offset * limit;
            string sqlQuery = $"SELECT * FROM Customer ORDER BY CustomerId OFFSET {startIndex} ROWS FETCH NEXT {limit} ROWS ONLY";
            return GetCustomersByQuery(sqlQuery);
        }

        public void Add(Customer customer)
        {
            string sqlQuery = "INSERT INTO Customer (FirstName, LastName, Country, PostalCode, Phone, Email) " +
                             "VALUES (@FirstName, @LastName, @Country, @PostalCode, @Phone, @Email)";
            var parameters = CreateCustomerParameters(customer);
            ExecuteNonQuery(sqlQuery, parameters);
        }

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



        public List<CustomerSpendings> GetHighestSpendingCustomers()
        {
            string sqlQuery = "SELECT c.CustomerId, c.FirstName, c.LastName," +
                "SUM(i.Total) AS TotalSpending " +
                "FROM Customer c " +
                "JOIN Invoice i ON c.CustomerId = i.CustomerId " +
                "GROUP BY c.CustomerId, c.FirstName, c.LastName " +
                "ORDER BY TotalSpending DESC;";



            // Call your existing method to execute the query and get the highest spending customers
            return GetCustomerSpendingsByQuery(sqlQuery);
        }

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

        public List<CustomerGenre> GetMostPopularGenresByCustomer(int customerId)
        {
            string sqlQuery = "WITH GenreCounts AS (\r\n    SELECT\r\n        c.CustomerId,\r\n        c.FirstName,\r\n        c.LastName,\r\n        g.Name AS GenreName,\r\n        COUNT(t.TrackId) AS TrackCount\r\n    FROM Customer c\r\n    JOIN Invoice i ON c.CustomerId = i.CustomerId\r\n    JOIN InvoiceLine il ON i.InvoiceId = il.InvoiceId\r\n    JOIN Track t ON il.TrackId = t.TrackId\r\n    JOIN Genre g ON t.GenreId = g.GenreId\r\n    WHERE c.CustomerId = @CustomerId\r\n    GROUP BY\r\n        c.CustomerId,\r\n        c.FirstName,\r\n        c.LastName,\r\n        g.Name\r\n)\r\nSELECT\r\n    CustomerId,\r\n    FirstName,\r\n    LastName,\r\n    STRING_AGG(GenreName, ', ') WITHIN GROUP (ORDER BY TrackCount DESC) AS MostPopularGenre\r\nFROM GenreCounts\r\nWHERE TrackCount = (SELECT MAX(TrackCount) FROM GenreCounts)\r\nGROUP BY\r\n    CustomerId,\r\n    FirstName,\r\n    LastName;\r\n";

            var parameters = new SqlParameter("@CustomerId", customerId);

            return GetCustomerGenresByQuery(sqlQuery, parameters);
        }
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

        // Method to display one customer
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


        // Method to display a list of customers
        public void DisplayCustomers(List<Customer> customers)
        {
            foreach (var customer in customers)
            {
                DisplayCustomer(customer);
            }
        }

    }
}
