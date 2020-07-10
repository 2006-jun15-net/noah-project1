using System.ComponentModel.DataAnnotations;

namespace StoreApp.WebApp.ViewModels
{
    public class CustomerViewModel
    {
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z'''-'\s]{1,40}$")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z'''-'\s]{1,40}$")]
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Username")]
        [Required]
        public string Username { get; set; }
    }
}
