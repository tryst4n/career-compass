using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CareerCompass.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CareerCompass.Data
{
    public class CareerCompassContext : IdentityDbContext<User>
    {
        public CareerCompassContext (DbContextOptions<CareerCompassContext> options)
            : base(options)
        {
        }

        public DbSet<CareerCompass.Models.CV> CV { get; set; } = default!;
    }
}
