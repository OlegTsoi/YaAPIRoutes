using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YaAPIRoutes.Models;

namespace YaAPIRoutes.Data
{
    public class YaAPIRoutesContext : DbContext
    {
        public YaAPIRoutesContext (DbContextOptions<YaAPIRoutesContext> options)
            : base(options)
        {
        }

        public DbSet<YaAPIRoutes.Models.Courier> Courier { get; set; }

        
    }
}
