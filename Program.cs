using ChinookMusicApp.Models;
using ChinookMusicApp.Repositories.CustomerRepo;

namespace ChinookMusicApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of your repository
            CustomerRepositoryImpl customerRepository = new CustomerRepositoryImpl();

            // Replace 'customerId' with the actual customer ID you want to test
            int customerId = 12; // Replace with the customer ID you want to test

            List<CustomerGenre> mostPopularGenres = customerRepository.GetMostPopularGenresByCustomer(customerId);

            // Iterate through the list and print each customer's most popular genre(s)
            foreach (CustomerGenre c in mostPopularGenres)
            {
                Console.WriteLine($"Customer ID: {c.CustomerId}");
                Console.WriteLine($"First Name: {c.FirstName}");
                Console.WriteLine($"Last Name: {c.LastName}");
                Console.WriteLine($"Most Popular Genre(s): {c.MostPopularGenre}");
                Console.WriteLine();
            }
        }
    }
}
