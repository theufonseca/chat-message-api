using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Profile
    {
        public string Id { get; init; } = default!;
        public string Name { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string Nick { get; init; } = default!;
    }
}
