using System;
using Microsoft.EntityFrameworkCore;
using Moonlay.MCService.Db;
using Moonlay.MCService.Models;
using System.Linq;
using System.Threading.Tasks;
using Moonlay.Core.Models;

namespace Moonlay.MCService.Customers
{
    internal class Repository : ICustomerRepository
    {
        public DbSet<Customer> DbSet { get; }
        public DbSet<CustomerTrail> DbSetTrail { get; }
        public string CurrentUser { get; }
        public bool IsCurrentUserDemo { get; }

        public Repository(IDbContext dbContext, IDbTrailContext dbTrailContext, ISignInService signIn)
        {
            DbSet = dbContext.Set<Customer>();
            DbSetTrail = dbTrailContext.Set<CustomerTrail>();
            CurrentUser = signIn.CurrentUser;
            IsCurrentUserDemo = signIn.Demo;
        }

        public Customer With(Guid id)
        {
            var r = DbSet.FirstOrDefault(o => o.Id == id);
            if (r == null) throw new RecordNotFoundException($"{nameof(Customer)} {id} not found");

            return r;
        }

        public CustomerTrail WithTrail(long id)
        {
            var r = DbSetTrail.FirstOrDefault(o => o.Id == id);
            if (r == null) throw new RecordNotFoundException($"{nameof(CustomerTrail)} {id} not found");

            return r;
        }
    }

    internal static class RepositoryHelpers
    {
        /// <summary>
        /// Save new Customer
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="id"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public static async Task<Customer> StoreAsync(this ICustomerRepository repo, Guid id, string firstName, string lastName)
        {
            var record = new Customer(id)
            {
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.Now,
                CreatedBy = repo.CurrentUser,
                UpdatedAt = DateTime.Now,
                UpdatedBy = repo.CurrentUser,
                Deleted = false,
                Tested = repo.IsCurrentUserDemo
            };

            await repo.DbSet.AddAsync(record);

            return record;
        }

        public static Task<Customer> UpdateAsync(this ICustomerRepository repo, Customer record)
        {
            record.UpdatedAt = DateTime.Now;
            record.UpdatedBy = repo.CurrentUser;

            repo.DbSet.Update(record);

            return Task.FromResult(record);
        }

        public static Task RemoveAsync(this ICustomerRepository repo, Guid id)
        {
            var record = repo.With(id);
            record.UpdatedAt = DateTime.Now;
            record.UpdatedBy = repo.CurrentUser;

            repo.DbSet.Update(record);

            return Task.FromResult(record);
        }
    }
}