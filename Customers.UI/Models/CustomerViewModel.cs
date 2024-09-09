using Customers.Classes;

namespace Customers.UI.Models
{
    public class CustomerViewModel : ViewModelBase
    {
        public CustomerViewModel(string method = "")
        {
            Customer = new();
            Customers = new List<Customer>();
            Method = method;
        }

        public string? Action { get; set; } = "Save";

        public string? Method { get; set; }

        public string? MethodVerb
        {
            get
            {
                switch (Method)
                {
                    case "Delete":
                        return "Deleted";
                    case "Create":
                        return "Created";
                    case "Edit":
                        return "Updated";
                    default:
                        return string.Empty;
                }

            }
        }

        public Customer Customer { get; set; }

        public IList<Customer> Customers { get; set; }
    }
}
