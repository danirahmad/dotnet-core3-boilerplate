using System;
using Moonlay.MCService.Customers;
using Moonlay.MCService.Db;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Moonlay.MCService.UnitTests.Domain.Customers
{
    internal class DbTestConnection : IDisposable
    {
        public DbTestConnection()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: "moonlaydev")
                .Options;

            var optionsTrail = new DbContextOptionsBuilder<MyDbTrailContext>()
                .UseInMemoryDatabase(databaseName: "moonlaydev_trail")
                .Options;

            Db = new MyDbContext(options);
            DbTrail = new MyDbTrailContext(optionsTrail);
        }

        public MyDbContext Db { get; }
        public MyDbTrailContext DbTrail { get; }

        public void Dispose()
        {
            Db.Dispose();
            DbTrail.Dispose();
        }
    }

    public class ServiceTest : IDisposable
    {
        private readonly MockRepository _MockRepo;
        private readonly Mock<ICustomerRepository> _CustomerRepo;
        private readonly Mock<ISignInService> _SignInService;

        public ServiceTest()
        {
            _MockRepo = new MockRepository(MockBehavior.Strict);
            _SignInService = _MockRepo.Create<ISignInService>();
            _CustomerRepo = _MockRepo.Create<ICustomerRepository>();
        }

        public void Dispose()
        {
            _MockRepo.VerifyAll();
        }

        private MCService.Customers.Service CreateService(DbTestConnection db)
        {
            _CustomerRepo.Setup(s => s.DbSet).Returns(db.Db.Set<Models.Customer>());
            _CustomerRepo.Setup(s => s.DbSetTrail).Returns(db.DbTrail.Set<Models.CustomerTrail>());

            _SignInService.Setup(x => x.Demo).Returns(true);
            _SignInService.Setup(x => x.CurrentUser).Returns("samplelogin@moonlay.com");

            return new MCService.Customers.Service(_CustomerRepo.Object, db.Db, db.DbTrail, _SignInService.Object);
        }

        [Fact(DisplayName = "CustomerService.CreateCustomer_Successfully")]
        public void CreateCustomer_Successfully()
        {
            using (var db = new DbTestConnection())
            {
                // Prepare Data
                var firstName = "Afandy";
                var lastName = "Lamusu";

                // Action
                var service = CreateService(db);
                var customer = service.NewCustomer(firstName, lastName);

                // Asserts
                customer.Should().NotBeNull();
                customer.FirstName.Should().Be(firstName);
                customer.LastName.Should().Be(lastName);
            }

        }

        [Fact(DisplayName = "CustomerService.Search_Successfully")]
        public void Search_Successfully()
        {
            using (var db = new DbTestConnection())
            {
                // prepare data
                var service = CreateService(db);
                service.NewCustomer("Andy", "Hasan");
                service.NewCustomer("Andy", "Kribo");

                var customers = service.Search(x => x.FirstName == "Andy");

                customers.Should().Contain(x => x.FirstName == "Andy");
            }
        }
    }
}