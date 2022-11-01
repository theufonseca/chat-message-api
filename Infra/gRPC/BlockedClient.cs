using chat_contacts_grpc;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.gRPC
{
    public class BlockedClient : IDisposable
    {
        public readonly Blocked.BlockedClient blockedClient;
        private readonly GrpcChannel channel;

        public BlockedClient(IConfiguration configuration)
        {
            var grpcServer = configuration.GetSection("Grpc:ContactServer").Value;
            channel = GrpcChannel.ForAddress(grpcServer);
            blockedClient = new Blocked.BlockedClient(channel);
        }

        public void Dispose()
        {
            channel.Dispose();
        }
    }
}
