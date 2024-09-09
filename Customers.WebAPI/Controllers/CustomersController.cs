using Customers.Classes;
using Customers.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Customers.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private IFileWriter _fileWriter;

        public CustomersController(ILogger<CustomersController> logger, IFileWriter fileWriter)
        {
            _fileWriter = fileWriter;
            _logger = logger;
        }

        [HttpGet]
        public IList<Customer> Get()
        {
            try
            {
                return _fileWriter.ReadCustomers().OrderBy(customer => customer.Id).ToList();
            }
            catch
            {
                _logger.LogError("There was a problem reading the customers from file in the Customers Web Service");
                throw;
            }
        }

        [HttpGet("{customerId:int}")]
        public Customer? Get(int customerId)
        {
            try
            {
                IList<Customer> customers = _fileWriter.ReadCustomers();
                return customers.FirstOrDefault(_ => _.Id == customerId);
            }
            catch
            {
                _logger.LogError("There was a problem reading the customers from file in the Customers Web Service");
                throw;
            }
        }

        [HttpPut]
        public ActionResult<Customer> AddUpdate(Customer customer)
        {
            IList<Customer> customers = _fileWriter.ReadCustomers();

            if (customer.Id != null)
            {
                customers.Remove(customers.FirstOrDefault(c => c.Id == customer.Id, new Customer()));
            }
            else
            {
                int nextCustomerId = customers.Max(c => c.Id) ?? 0;
                customer.Id = ++nextCustomerId;
            }

            customers.Add(customer);

            try
            {
                _fileWriter.WriteCustomers(customers);
            }
            catch
            {
                _logger.LogError("There was a problem writing to the customers file in the Customers Web Service");
                throw;
            }

            return CreatedAtAction(nameof(AddUpdate), new { id = customer.Id }, customer);
        }

        [HttpDelete("{customerId:int}")]
        public ActionResult<bool> Delete(int customerId)
        {
            IList<Customer> customers = _fileWriter.ReadCustomers();
            customers.Remove(customers.FirstOrDefault(_ => _.Id == customerId, new Customer()));

            try
            {
                _fileWriter.WriteCustomers(customers);
            }
            catch
            {
                _logger.LogError("There was a problem writing to the customers file in the Customers Web Service");
                throw;
            }

            return Ok();
        }
    }
}
