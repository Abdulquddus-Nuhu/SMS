using System;
using Module.Access.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Module.Access.Data
{

    public class AcessDbContext : IdentityDbContext<Persona, Role, Guid>
    {
        private readonly DbContextOptions _options;
        public AcessDbContext(DbContextOptions<AcessDbContext> options) : base(options)
        {
            _options = options;
        }

        //public DbSet<Student> Students { get; set; }
        //public DbSet<Parent> Parents { get; set; }
    }

}

