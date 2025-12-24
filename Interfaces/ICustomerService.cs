using 水水水果API.Models.DTO;

namespace 水水水果API.Interfaces
{
    public interface ICustomerService
    {
        Customer GetCustomerById(int id);

        /// <summary>
        /// 新增 Customer
        /// </summary>
        int CreateCustomer(CustomerCreate customerCreate);

        /// <summary>
        /// 更新 Customer
        /// </summary>
        int UpdateCustomer(CustomerUpdate customerUpdate);
    }
}
