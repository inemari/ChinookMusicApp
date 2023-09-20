using ChinookMusicApp.Models;

namespace ChinookMusicApp.Repositories.CustomerRepo
{
    public interface ICustomerRepository : ICrudRepository<Customer, int>
    {
        void DisplayCustomer(Customer customer);
        void DisplayCustomers(List<Customer> customers);
        List<Customer> GetPage(int limit, int offset);

        List<CustomerCountry> GetCustomerCountByCountry();
        List<CustomerSpendings> GetHighestSpendingCustomers();
        List<CustomerGenre> GetMostPopularGenresByCustomer(int customerId);
    }
}
