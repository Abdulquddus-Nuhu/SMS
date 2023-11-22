using Access.Data.Config;
using Access.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            var entityTypes = builder.Model.GetEntityTypes();
            entityTypes.ToList().ForEach(entityType =>
            {
                if (typeof(IBaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            });
        }

    }
}
