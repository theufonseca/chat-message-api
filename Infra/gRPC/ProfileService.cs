using chat_profile_grpc_client;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.gRPC
{
    public class ProfileService : IProfileService
    {
        private readonly ProfileClient client;

        public ProfileService(ProfileClient client)
        {
            this.client = client;
        }
        public async Task<Domain.Entities.Profile?> Get(string id)
        {
            var result = await client.profileClient.GetAsync(new ProfileRequest { Id = id });
            
            if (result == null)
                return null;

            return new Domain.Entities.Profile
            {
                Id = result.Id,
                Email = result.Email,
                Name = result.Name,
                Nick = result.Nick
            };
        }
    }
}
