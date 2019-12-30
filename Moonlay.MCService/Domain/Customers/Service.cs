using System;
using System.Collections.Generic;
using Moonlay.MCService.Db;
using Moonlay.MCService.Models;
using System.Linq;

namespace Moonlay.MCService.Customers
{
    public class Service : ICustomerService
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IDbContext _db;
        private readonly IDbTrailContext _dbTrail;
        private readonly ISignInService _signIn;

        public Service(ICustomerRepository repository, IDbContext db, IDbTrailContext dbTrail, ISignInService signIn)
        {
            _customerRepo = repository;
            _db = db;
            _dbTrail = dbTrail;
            _signIn = signIn;
        }

        public Customer NewCustomer(string firstName, string lastName)
        {
            var customer = this._customerRepo.Store(Guid.NewGuid(), firstName, lastName, _signIn.CurrentUser);

            this._db.SaveChanges();

            this._customerRepo.DbSetTrail.Add(customer.ToTrail());

            this._dbTrail.SaveChanges();

            return customer;
        }

        public List<Customer> Search(Func<Customer, bool> criteria = null, int page = 0, int size = 25)
        {
            if (criteria == null)
                criteria = x => true;

            return _customerRepo.DbSet.Where(criteria).OrderBy(o => o.LastName).Skip(page * size).Take(size).ToList();
        }

        public List<CustomerTrail> GetCustomerLogs(Guid id)
        {
            var customer = _customerRepo.With(id);

            return _customerRepo.DbSetTrail.Where(o=>o.CustomerId == id).ToList();
        }
    }

}