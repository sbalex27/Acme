using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Acme.Models;

namespace Acme.Data
{
    public class AcmeContext : DbContext
    {
        public AcmeContext (DbContextOptions<AcmeContext> options)
            : base(options)
        {
        }

        public DbSet<Acme.Models.Form> Form { get; set; } = default!;
        public DbSet<Acme.Models.Field> Field { get; set; } = default!;
    }
}
