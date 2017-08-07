using System;
using System.Linq;
using StarterAPI.Entities;
using StarterAPI.QueryParameters;

namespace StarterAPI.Repositories
{
    public interface ICustomerRepository
    {
        void Add(Customer item);
        void Delete(Guid id);
        IQueryable<Customer> GetAll(CustomerQueryParameters customerQueryParameters);
        Customer GetSingle(Guid id);
        bool Save();
        int Count();
        void Update(Customer item);
    }
}