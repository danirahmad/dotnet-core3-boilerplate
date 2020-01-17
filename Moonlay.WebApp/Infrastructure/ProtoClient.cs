using Grpc.Net.Client;
using System;
using System.Threading.Tasks;

namespace Moonlay.WebApp.Infrastructure
{
    public class ProtoClient
    {
        public ProtoClient()
        {

        }

        public async Task SendAsync()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new MasterData.Protos.ManageOrganization.ManageOrganizationClient(channel);

            var reply = await client.NewOrganizationAsync(
                              new MasterData.Protos.NewOrganizationReq { Name = "MyOrg" });
            //Console.WriteLine("Greeting: " + reply.Message);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
