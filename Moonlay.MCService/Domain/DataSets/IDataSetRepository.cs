using Moonlay.MasterData.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moonlay.MasterData.WebApi.Domain.DataSets
{
    public interface IDataSetRepository
    {
        Task<List<DataSet>> AllAsync(string domainName);

        Task<List<DataSetAttribute>> GetAttributesAsync(string datasetName, string domainName);

        Task Create(DataSet model);

        Task Delete(string name);
    }
}
