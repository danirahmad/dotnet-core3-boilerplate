using FluentAssertions;
using Moonlay.MasterData.WebApi.Db;
using Moonlay.MasterData.WebApi.Domain.DataSets;
using Moonlay.MasterData.WebApi.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Moonlay.MasterData.WebApi.UnitTests.Domain.DataSets
{
    public class DataSetRepositoryTests : IDisposable
    {
        private readonly MockRepository mockRepository;

        public DataSetRepositoryTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DataSetRepository CreateDataSetRepository()
        {
            return new DataSetRepository(new MyConnection(new Microsoft.Data.SqlClient.SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=moonlaydev;Integrated Security=True")));
        }

        [Fact]
        public async Task AllDataSets_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dataSetRepository = this.CreateDataSetRepository();
            string domainName = "dbo";

            // Act
            var result = await dataSetRepository.AllAsync(
                domainName);

            // Assert
            result.Should().NotBeEmpty();
            result.First().Name.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetAttributesAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dataSetRepository = this.CreateDataSetRepository();
            string domainName = "dbo";
            string datasetName = "Customers";

            // Act
            var result = await dataSetRepository.GetAttributesAsync(
                datasetName, domainName);

            // Assert
            result.Should().NotBeEmpty();
            result.First().Name.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Create_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dataSetRepository = this.CreateDataSetRepository();
            string name = "People";
            string domainName = "dbo";
            string orgName = "Customers";
            IEnumerable<DataSetAttribute> dataSetAttributes = new DataSetAttribute[] { };

            // Act
            await dataSetRepository.Create(
                new DataSet { Name = name, DomainName = domainName });

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dataSetRepository = this.CreateDataSetRepository();
            string name = null;

            // Act
            await dataSetRepository.Delete(
                name);

            // Assert
            Assert.True(false);
        }
    }
}
