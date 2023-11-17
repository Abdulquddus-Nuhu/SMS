using Access.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Data
{
    public class AccessDbContext : IdentityDbContext<Persona, Role, Guid>
    {
        private readonly DbContextOptions _options;
        public AccessDbContext(DbContextOptions<AccessDbContext> options) : base(options)
        {
            _options = options;
        }

        //public DbSet<Student> Students { get; set; }
        //public DbSet<Parent> Parents { get; set; }
    }
}
