using Moonlay.MasterData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moonlay.MasterData.WebApi.Domain.DataSets
{
    public interface IDataSetService
    {
        Task NewDataSet(string name, string domainName, string orgName, IEnumerable<DataSetAttribute> attributes, Action<DataSet> beforeSave = null);
        Task Remove(string name);
    }

    public class DataSetService : IDataSetService
    {
        private readonly IDataSetRepository _dataSetRepository;

        public DataSetService(IDataSetRepository dataSetRepository)
        {
            _dataSetRepository = dataSetRepository;
        }

        public async Task NewDataSet(string name, string domainName, string orgName, IEnumerable<DataSetAttribute> attributes, Action<DataSet> beforeSave = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("message", nameof(name));
            }

            if (string.IsNullOrEmpty(domainName))
            {
                throw new ArgumentException("message", nameof(domainName));
            }

            if (string.IsNullOrEmpty(orgName))
            {
                throw new ArgumentException("message", nameof(orgName));
            }

            await _dataSetRepository.Create(new DataSet { Description = "", Name = name, DomainName = domainName });
        }

        public Task Remove(string name)
        {
            throw new NotImplementedException();
        }
    }
}
