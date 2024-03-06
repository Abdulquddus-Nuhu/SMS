using Core.Entities;
using Core.Entities.Users;
using Infrastructure.Data.Config;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Reflection.Emit;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<Persona, Role, Guid>
    {
        private readonly DbContextOptions _options;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
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
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripStudent> TripStudents { get; set; }

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

            builder.Entity<TripStudent>()
                .HasKey(ts => new { ts.TripId, ts.StudentId }); // Composite key

            builder.Entity<TripStudent>()
                .HasOne(ts => ts.Trip)
                .WithMany(t => t.TripStudents)
                .HasForeignKey(ts => ts.TripId);

            builder.Entity<TripStudent>()
                .HasOne(ts => ts.Student)
                .WithMany(s => s.TripStudents)
                .HasForeignKey(ts => ts.StudentId);

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
