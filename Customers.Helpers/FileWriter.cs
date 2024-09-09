using Customers.Classes;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Customers.Helpers
{
    public class FileWriter : IFileWriter
    {
        private readonly ILogger<FileWriter> _logger;

        public FileWriter(ILogger<FileWriter> logger)
        {
            _logger = logger;
        }

        public IList<Customer> ReadCustomers()
        {
            IList<Customer>? customers = new List<Customer>();

            try
            {
                using (StreamReader stream = File.OpenText("Customers.json"))
                {
                    string json = stream.ReadToEnd();
                    customers = JsonConvert.DeserializeObject<List<Customer>>(json);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        
            return customers!;
        }

        public bool WriteCustomers(IList<Customer> customers)
        {
            try
            {
                using (StreamWriter stream = System.IO.File.CreateText("Customers.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(stream, customers);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

            return true;
        }
    }
}
