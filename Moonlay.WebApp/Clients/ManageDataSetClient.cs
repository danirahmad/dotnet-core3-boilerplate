using Grpc.Core;
using Moonlay.MasterData.Protos;
using System;
using System.Threading;

namespace Moonlay.WebApp.Clients
{
    internal class ManageDataSetClient : MasterData.Protos.ManageDataSet.ManageDataSetClient, IManageDataSetClient
    {
        public ManageDataSetClient(ChannelBase channel) : base(channel)
        {

        }

        public override AsyncUnaryCall<Reply> NewDatasetAsync(NewDatasetReq request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return base.NewDatasetAsync(request, headers, deadline, cancellationToken);
        }

        public override AsyncUnaryCall<Reply> RemoveDataSetAsync(RemoveDataSetReq request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default)
        {
            return base.RemoveDataSetAsync(request, headers, deadline, cancellationToken);
        }
    }

    public interface IManageDataSetClient
    {
        AsyncUnaryCall<Reply> NewDatasetAsync(NewDatasetReq request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
        AsyncUnaryCall<Reply> RemoveDataSetAsync(RemoveDataSetReq request, Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default);
    }
}
