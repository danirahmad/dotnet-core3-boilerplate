using Grpc.Core;
using Microsoft.Extensions.Logging;
using Moonlay.MasterData.Protos;
using Moonlay.MasterData.WebApi.Domain.DataSets;
using System.Threading.Tasks;

namespace Moonlay.MasterData.WebApi.Services
{
    public class ManageDataSetService : Protos.ManageDataSet.ManageDataSetBase
    {
        private readonly ILogger<ManageDataSetService> _logger;
        private readonly IDataSetService _dataSetService;

        public ManageDataSetService(ILogger<ManageDataSetService> logger, IDataSetService dataSetService)
        {
            _logger = logger;
            _dataSetService = dataSetService;
        }

        public override Task<Reply> NewDataset(NewDatasetReq request, ServerCallContext context)
        {
            _dataSetService.NewDataSet(request.Name, request.DomainName, request.OrganizationName, null);

            return Task.FromResult(new Reply { 
                Success = true,
                Message = "Successfully"
            });
        }
    }
}
