using Moonlay.MasterData.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moonlay.MasterData.WebApi.Domain.DataSets
{
    public interface IDataSetRepository
    {
        Task<List<DataSet>> AllDataSetsAsync(string domainName);

        Task<List<DataSetAttribute>> GetAttributesAsync(string datasetName, string domainName);

        Task Create(string name, string domainName, string orgName, IEnumerable<DataSetAttribute> dataSetAttributes);

        Task Delete(string name);
    }
}
