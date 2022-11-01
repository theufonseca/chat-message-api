using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Blockeds
    {
        public IList<Profile> BlockedList { get; set; } = new List<Profile>();
    }
}
