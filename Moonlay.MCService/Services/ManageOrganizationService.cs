using Grpc.Core;
using Microsoft.Extensions.Logging;
using Moonlay.MasterData.Protos;
using System.Threading.Tasks;

namespace Moonlay.MasterData.WebApi.Services
{
    public class ManageOrganizationService : Protos.ManageOrganization.ManageOrganizationBase
    {
        private readonly ILogger<ManageOrganizationService> _logger;
        public ManageOrganizationService(ILogger<ManageOrganizationService> logger)
        {
            _logger = logger;
        }

        public override Task<Reply> NewOrganization(NewOrganizationReq request, ServerCallContext context)
        {
            return Task.FromResult(new Reply
            {
                Success = true,
                Message = "Successfully"
            });
        }
    }
}
