using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GreenSpace.Domain.Entities;

namespace GreenSpace.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region DbSets

        public DbSet<User> User { get; set; }
       
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(assembly: Assembly.GetExecutingAssembly());
        }
    }
}
