using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Customers.Classes
{
    public class Customer
    {
        public int? Id { get; set; }

        [DisplayName("First Name")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(20, ErrorMessage = "First name must be less than 20 characters")]
        [RegularExpression("^([a-zA-Z .&'-]+)$", ErrorMessage = "First name contains invalid characters")]
        public string? FirstName { get; set; }

        [DisplayName("Last Name")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(20, ErrorMessage = "Last name must be less than 20 characters")]
        [RegularExpression("^([a-zA-Z .&'-]+)$", ErrorMessage = "Last name contains invalid characters")]
        public string? LastName { get; set; }
    }
}
