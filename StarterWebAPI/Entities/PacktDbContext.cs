using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StarterWebAPI.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterAPI.Entities
{
    public class PacktDbContext : IdentityDbContext
    {
        public PacktDbContext(DbContextOptions<PacktDbContext> options) : base(options )
        {


        }

        public DbSet<Customer > Customers { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }

    }
}
