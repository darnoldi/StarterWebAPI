using StarterAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterAPI.Services
{
    public class SeedDataService : ISeedDataService
    {
        private PacktDbContext _context;
        public SeedDataService(PacktDbContext context)
        {
            _context = context;
        }

        public async Task EnsureSeedData()
        {
            _context.Database.EnsureCreated();

            _context.Customers.RemoveRange(_context.Customers);
            _context.SaveChanges();

            Customer customer = new Customer();
            customer.Firstname = "Chuck";
            customer.Lastname = "Norris";
            customer.Age = 30;
            customer.Id = Guid.NewGuid();

            _context.Add(customer);

            Customer customer2 = new Customer();
            customer2.Firstname = "Dave";
            customer2.Lastname = "Arnoldi";
            customer2.Age = 47;
            customer2.Id = Guid.NewGuid();

            _context.Add(customer2);

            await _context.SaveChangesAsync();
        }

    }
}
