namespace 水水水果API.Interfaces
{
    public interface ICustomerRepository
    {
        Customer GetCustomerById(int custId);
        int CreateCustomer(Customer customer);
        void UpdateCustomer(Customer cust);
    }
}
