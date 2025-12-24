using System.Data.Common;
using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface ICustomerRepository
    {
        Customer GetCustomerById(int custId);

        int CreateCustomer(Customer customerCreate);

        int UpdateCustomer(Customer customerUpdate);
    }
}
