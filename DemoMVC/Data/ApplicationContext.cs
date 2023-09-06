using DemoMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Data
{

    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<CodeType> CodeTypes { get; set; } = default!;
        public DbSet<Property> Property { get; set; } = default!;
        public DbSet<ApplicationUser> User { get; set; } = default!;
    }
}
