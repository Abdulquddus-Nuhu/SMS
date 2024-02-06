using Access.Core.Entities;
using Access.Core.Entities.Users;
using Access.Data.Config;
using Access.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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

        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<ParentStudent> ParentStudent { get; set; }
        public DbSet<Busdriver> Busdrivers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<QrCode> QrCodes { get; set; }

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


            builder.Entity<Parent>()
                    .HasMany(e => e.Students)
                    .WithMany(e => e.Parents)
                    .UsingEntity<ParentStudent>();

        }

        public async Task<bool> TrySaveChangesAsync()
        {
            try
            {
                await SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

    }
}
