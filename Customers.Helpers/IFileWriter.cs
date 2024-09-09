using Customers.Classes;

namespace Customers.Helpers
{
    public interface IFileWriter
    {
        public IList<Customer> ReadCustomers();

        public bool WriteCustomers(IList<Customer> customers);
    }
}
