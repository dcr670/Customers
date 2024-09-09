using Customers.UI.Models;
using Customers.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Customers.UI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private IHttpClientFactory _httpClientFactory;
        private IConfiguration _configuration;

        public CustomerController(ILogger<CustomerController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            CustomerViewModel customerVM = new();

            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(_configuration["Customers.WebAPI:URI"] ?? string.Empty);

                var responseTask = await httpClient.GetAsync("/Customers");

                responseTask.EnsureSuccessStatusCode();
                
                customerVM.Customers = await responseTask.Content.ReadFromJsonAsync<IList<Customer>>() ?? new List<Customer>();
            }
            catch (Exception ex)
            {
                customerVM.ErrorMessage = "There was a problem calling the Customers web service, please see logs for further information";
                _logger.LogError(ex.InnerException, ex.Message);
            }

            return View(customerVM);
        }


        [HttpGet]
        public async Task<IActionResult> Customer(int? customerId, string method)
        {
            CustomerViewModel customerVM = new(method);

            if (customerId != null)
            {
                if (method == "Delete")
                {
                    customerVM.Action = "Delete";
                }

                try
                {
                    HttpClient httpClient = _httpClientFactory.CreateClient();
                    httpClient.BaseAddress = new Uri(_configuration["Customers.WebAPI:URI"] ?? string.Empty);

                    var responseTask = await httpClient.GetAsync($"/Customers/{customerId}");

                    if (responseTask.IsSuccessStatusCode)
                    {
                        customerVM.Customer = await responseTask.Content.ReadFromJsonAsync<Customer>() ?? new Customer();
                    }
                }
                catch (Exception ex)
                {
                    customerVM.ErrorMessage = "There was a problem calling the Customers web service, please see logs for further information";
                    _logger.LogError(ex.InnerException, ex.Message);
                }
            }

            return View(customerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Customer customer)
        {
            CustomerViewModel customerVM = new(customer.Id == null ? "Create" : "Edit");

            if (ModelState.IsValid)
            {
                try
                {
                    HttpClient httpClient = _httpClientFactory.CreateClient();
                    httpClient.BaseAddress = new Uri(_configuration["Customers.WebAPI:URI"] ?? string.Empty);

                    var responseTask = await httpClient.PutAsJsonAsync<Customer>("/Customers", customer);

                    if (responseTask.IsSuccessStatusCode)
                    {
                        customerVM.Customer = await responseTask.Content.ReadFromJsonAsync<Customer>() ?? new Customer();
                        customerVM.Result = true;
                    }
                }
                catch (Exception ex)
                {
                    customerVM.ErrorMessage = "There was a problem calling the Customers web service, please see logs for further information";
                    _logger.LogError(ex.InnerException, ex.Message);
                }
                
            }

            return View("Customer", customerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Customer customer)
        {
            CustomerViewModel customerVM = new("Delete");
            customerVM.Customer = customer;
            customerVM.Action = customerVM.Method;

            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient();
                httpClient.BaseAddress = new Uri(_configuration["Customers.WebAPI:URI"] ?? string.Empty);

                var responseTask = await httpClient.DeleteAsync($"/Customers/{customer.Id}");

                if (responseTask.IsSuccessStatusCode)
                {
                    customerVM.Result = true;
                }
            }
            catch (Exception ex)
            {
                customerVM.ErrorMessage = "There was a problem calling the Customers web service, please see logs for further information";
                _logger.LogError(ex.InnerException, ex.Message);
            }

            return View("Customer", customerVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
