using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface ICustomerService
    {
        Customer GetCustomerById(int id);
        int UpsertCustomer(CustomerUpsert customerDto);
    }
}
