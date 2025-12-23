namespace 水水水果API.Interfaces
{
    public interface ICustomerRepository
    {
        Customer GetCustomerById(int custId);
        int UpsertCustomer(CustomerUpsert cust);
    }
}
