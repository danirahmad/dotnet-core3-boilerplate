using Grpc.Core;
using Microsoft.Extensions.Logging;
using Moonlay.MasterData.Protos;
using System.Threading.Tasks;

namespace Moonlay.MasterData.WebApi.Services
{
    public class ManageDataSetService : Protos.ManageDataSet.ManageDataSetBase
    {
        private readonly ILogger<ManageDataSetService> _logger;
        public ManageDataSetService(ILogger<ManageDataSetService> logger)
        {
            _logger = logger;
        }

        public override Task<Reply> NewDataset(NewDatasetReq request, ServerCallContext context)
        {
            return Task.FromResult(new Reply { 
                Success = true,
                Message = "Successfully"
            });
        }
    }
}
