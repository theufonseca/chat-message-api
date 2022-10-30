using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.gRPC
{
    public class ProfileClient : IDisposable
    {
        public readonly chat_profile_grpc_client.Profile.ProfileClient profileClient;
        private readonly GrpcChannel channel;
        public ProfileClient(IConfiguration configuration)
        {
            var grpcServer = configuration.GetSection("Grpc:Server").Value;
            channel = GrpcChannel.ForAddress(grpcServer);
            profileClient = new chat_profile_grpc_client.Profile.ProfileClient(channel);
            
        }

        public void Dispose()
        {
            channel.Dispose();
        }
    }
}
