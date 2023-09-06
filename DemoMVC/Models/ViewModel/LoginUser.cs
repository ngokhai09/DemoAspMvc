using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models.ViewModel
{
    public class LoginUser
    {
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set;}
        public string ReturnUrl { get; set; }

    }
}
