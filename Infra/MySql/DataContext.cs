using Infra.MySql.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.MySql
{
    public class DataContext : DbContext
    {
        public DbSet<MessageModel> Message { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    }
}
