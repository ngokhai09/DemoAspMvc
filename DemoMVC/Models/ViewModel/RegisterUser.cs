using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models.ViewModel
{
    public class RegisterUser
    {
        [Display(Name = "Username")]
        [Required]
        public string Username { get; set; }
        [Display(Name = "Fullname")]
        public string Name { get; set; }

        [Display(Name ="Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name ="Phone Number")]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
