﻿



using Microsoft.AspNetCore.Identity;

namespace DemoMVC.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        
    }
}
