using chat_contacts_grpc;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.gRPC
{
    public class BlockedService : IBlockedService
    {
        private readonly BlockedClient blockedClient;

        public BlockedService(BlockedClient blockedClient)
        {
            this.blockedClient = blockedClient;
        }

        public async Task<Blockeds> GetBlockeds(string id)
        {
            var blockeds = new Blockeds();
            var result = await blockedClient.blockedClient.getBlockedListAsync(new getBlockedListRequest
            {
                Id = id
            });

            foreach (var item in result.BlockedList)
            {
                blockeds.BlockedList.Add(new Profile
                {
                    Name = item.Name,
                    Id = item.Id,
                    Email = item.Email,
                    Nick = item.Nick,
                });
            }

            return blockeds;
        }
    }
}
