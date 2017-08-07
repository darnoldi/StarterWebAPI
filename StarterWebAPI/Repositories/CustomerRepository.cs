using StarterAPI.Entities;
using StarterAPI.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace StarterAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private PacktDbContext _context;

        public CustomerRepository(PacktDbContext context)
        {
            _context = context;
        }

        public IQueryable<Customer> GetAll(CustomerQueryParameters customerQueryParameters)
        {
            IQueryable<Customer> _allCustomers;

          
                 _allCustomers = _context.Customers.OrderBy(customerQueryParameters.OrderBy, customerQueryParameters.OrderBy);
            

            if (customerQueryParameters.HasQuery)
            {
                _allCustomers = _allCustomers
                    .Where(x => x.Firstname.ToLowerInvariant().Contains( customerQueryParameters.Query.ToLowerInvariant())
                    || x.Lastname.ToLowerInvariant().Contains( customerQueryParameters.Query.ToLowerInvariant()));
            }

            
            return _allCustomers
                .Skip(customerQueryParameters.PageCount * (customerQueryParameters.Page - 1))
                .Take(customerQueryParameters.PageCount);
        }

        public Customer GetSingle(Guid id)
        {
            return _context.Customers.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Customer item)
        {
            _context.Customers.Add(item);
        }

        public void Delete(Guid id)
        {
            Customer Customer = GetSingle(id);
            _context.Customers.Remove(Customer);
        }

        public void Update(Customer item)
        {
            _context.Customers.Update(item);
        }

        public int Count()
        {
            return _context.Customers.Count();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
