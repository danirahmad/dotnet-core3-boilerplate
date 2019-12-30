using System;
using Microsoft.EntityFrameworkCore;
using Moonlay.MCService.Db;
using Moonlay.MCService.Models;
using System.Linq;

namespace Moonlay.MCService.Customers
{
    internal class Repository : ICustomerRepository
    {
        private readonly DbSet<Customer> _dbSet;
        private readonly DbSet<CustomerTrail> _dbSetTrail;

        public DbSet<Customer> DbSet => _dbSet;
        public DbSet<CustomerTrail> DbSetTrail => _dbSetTrail;

        public Repository(IDbContext dbContext, IDbTrailContext dbTrailContext)
        {
            _dbSet = dbContext.Set<Customer>();
            _dbSetTrail = dbTrailContext.Set<CustomerTrail>();
        }

        public Customer With(Guid id)
        {
            var r = _dbSet.FirstOrDefault(o => o.Id == id);
            if (r == null) throw new RecordNotFoundException($"{nameof(Customer)} {id} not found");

            return r;
        }

        public CustomerTrail WithTrail(long id)
        {
            var r = _dbSetTrail.FirstOrDefault(o => o.Id == id);
            if (r == null) throw new RecordNotFoundException($"{nameof(CustomerTrail)} {id} not found");

            return r;
        }
    }

    internal static class RepositoryHelpers
    {
        public static Customer Store(this ICustomerRepository r, Guid id, string firstName, string lastName, string actionBy)
        {
            var record = new Customer(id)
            {
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.Now,
                CreatedBy = actionBy,
                UpdatedAt = DateTime.Now,
                UpdatedBy = actionBy
            };

            r.DbSet.Add(record);

            return record;
        }

        public static void Remove(this ICustomerRepository r, Guid id)
        {
            r.DbSet.Remove(r.With(id).MarkDeleted());
        }
    }
}