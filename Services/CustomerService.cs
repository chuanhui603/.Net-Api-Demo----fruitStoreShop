using 水水水果API.Models.DTO;

namespace 水水水果API.Services
{
    /// <summary>
    /// CustomerService 負責處理 Customer 相關的業務邏輯
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ILogger<CustomerService> logger, ICustomerRepository customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }

        public Customer GetCustomerById(int id)
        {
            return _customerRepository.GetCustomerById(id);
        }

        public int CreateCustomer(CustomerCreate customerCreate)
        {
            _logger.LogInformation("Creating new customer");
            return _customerRepository.CreateCustomer(new Customer()
            {
                BrandId = customerCreate.BrandId,
                FirstName = customerCreate.FirstName,
                LastName = customerCreate.LastName,
                BirthDay = customerCreate.BirthDay,
                Gender = customerCreate.Gender,
                Phone = customerCreate.Phone,
            });
        }

        public int UpdateCustomer(CustomerUpdate customerUpdate)
        {
            var existingCustomer = _customerRepository.GetCustomerById(customerUpdate.Id)
                ?? throw new ArgumentException($"Customer with ID {customerUpdate.Id} not found.");

            _logger.LogInformation("Updating existing customer with ID: {CustomerId}", customerUpdate.Id);
            return _customerRepository.UpdateCustomer(new Customer()
            {
                Id = customerUpdate.Id,
                BrandId = customerUpdate.BrandId,
                FirstName = customerUpdate.FirstName,
                LastName = customerUpdate.LastName,
                BirthDay = customerUpdate.BirthDay,
                Gender = customerUpdate.Gender,
                Phone = customerUpdate.Phone,
            });
        }
    }
}
