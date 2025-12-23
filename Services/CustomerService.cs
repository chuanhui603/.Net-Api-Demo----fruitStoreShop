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

        public int UpsertCustomer(CustomerUpsert customerDto)
        {
            if (customerDto.Id > 0)
            {
                var existingCustomer = _customerRepository.GetCustomerById(customerDto.Id);
                if (existingCustomer != null)
                {
                    _logger.LogInformation("Updating existing customer with ID: {CustomerId}", customerDto.Id);
                    return _customerRepository.UpsertCustomer(customerDto);
                }
            }

            _logger.LogInformation("Creating new customer");
            return _customerRepository.UpsertCustomer(customerDto);
        }
    }
}
