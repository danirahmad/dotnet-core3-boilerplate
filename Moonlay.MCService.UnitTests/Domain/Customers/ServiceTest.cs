using System;
using Moonlay.MCService.Customers;
using Moonlay.MCService.Db;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
            _CustomerRepo = _MockRepo.Create<ICustomerRepository>();
        }

        public void Dispose()
        {
            _MockRepo.VerifyAll();
        }

        private MCService.Customers.Service CreateService(DbTestConnection db)
        {
            return new MCService.Customers.Service(_CustomerRepo.Object, db.Db, db.DbTrail);
        }


        [Theory(DisplayName = "CustomerService.CreateCustomer_Successfully")]
        [InlineData("Afandy", "Lamusu")]
        [InlineData("Afandy@", "Lamusu")]
        [InlineData("Afandy&77", "Lamusu")]
        public async Task CreateCustomer_Successfully(string firstName, string lastName)
        {
            using (var db = new DbTestConnection())
            {
                _CustomerRepo.Setup(s => s.DbSet).Returns(db.Db.Set<Models.Customer>());
                _CustomerRepo.Setup(s => s.DbSetTrail).Returns(db.DbTrail.Set<Models.CustomerTrail>());
                _CustomerRepo.Setup(s => s.CurrentUser).Returns("samplelogin@moonlay.com");
                _CustomerRepo.Setup(s => s.IsDemo).Returns(true);

                // Action
                var service = CreateService(db);
                Models.Customer customer = await service.NewCustomerAsync(firstName, lastName); ;
                
                // Asserts
                customer.Should().NotBeNull();
                customer.FirstName.Should().Be(firstName);
                customer.LastName.Should().Be(lastName);

            }

        }

        [Theory(DisplayName = "CustomerService.CreateCustomer_Fail_FirstName_NullOrEmpty")]
        [InlineData("", "Lamusu")]
        public async Task CreateCustomer_Fail_FirstName(string firstName, string lastName)
        {
            using (var db = new DbTestConnection())
            {
                var service = CreateService(db);

                // Action
                Func<Task> act = async () => { await service.NewCustomerAsync(firstName, lastName); };

                // Asserts
                await act.Should().ThrowAsync<ArgumentException>();
            }
        }

        [Fact(DisplayName = "CustomerService.Search_Successfully")]
        public async Task Search_Successfully()
        {
            using (var db = new DbTestConnection())
            {
                _CustomerRepo.Setup(s => s.DbSet).Returns(db.Db.Set<Models.Customer>());
                _CustomerRepo.Setup(s => s.DbSetTrail).Returns(db.DbTrail.Set<Models.CustomerTrail>());
                _CustomerRepo.Setup(s => s.CurrentUser).Returns("samplelogin@moonlay.com");
                _CustomerRepo.Setup(s => s.IsDemo).Returns(true);

                // prepare data
                var service = CreateService(db);
                await service.NewCustomerAsync("Andy", "Hasan");
                await service.NewCustomerAsync("Andy", "Kribo");

                var customers = await service.SearchAsync(x => x.FirstName == "Andy");

                customers.Should().Contain(x => x.FirstName == "Andy");
            }
        }

        [Fact(DisplayName = "CustomerService.UpdateProfile_Successfully")]
        public async Task UpdateProfile_Successfully()
        {
            using (var db = new DbTestConnection())
            {
                _CustomerRepo.Setup(s => s.DbSet).Returns(db.Db.Set<Models.Customer>());
                _CustomerRepo.Setup(s => s.DbSetTrail).Returns(db.DbTrail.Set<Models.CustomerTrail>());
                _CustomerRepo.Setup(s => s.CurrentUser).Returns("samplelogin@moonlay.com");
                _CustomerRepo.Setup(s => s.IsDemo).Returns(true);

                // prepare data
                var service = CreateService(db);

                var newCustomer = await service.NewCustomerAsync("Andy", "Kribo");
                DateTimeOffset timeOfCreated = newCustomer.UpdatedAt;

                _CustomerRepo.Setup(x => x.With(newCustomer.Id)).Returns(newCustomer);

                // Action
                var updatedCustomer = await service.UpdateProfileAsync(newCustomer.Id, "Andy", "Hasan");

                // Assert : check the time of created not equal the time when updated.
                updatedCustomer.Id.Should().Be(newCustomer.Id);
                updatedCustomer.UpdatedAt.Should().BeOnOrAfter(timeOfCreated);

                // validate the logs, should contain 2 records.
                (await service.LogsAsync(newCustomer.Id)).Should().HaveCount(2);
            }   
        }
    }
}