using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GreenSpace.Domain.Entities;

namespace GreenSpace.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region DbSets


        public virtual DbSet<Bill> Bills { get; set; }

        public virtual DbSet<Blog> Blogs { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Contract> Contracts { get; set; }

        public virtual DbSet<DesignIdea> DesignIdeas { get; set; }

        public virtual DbSet<Document> Documents { get; set; }

        public virtual DbSet<Image> Images { get; set; }

        public virtual DbSet<Material> Materials { get; set; }

        public virtual DbSet<MaterialFeedback> MaterialFeedbacks { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<Payment> Payments { get; set; }

        public virtual DbSet<ProductDetail> ProductDetails { get; set; }

        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<ServiceFeedback> ServiceFeedbacks { get; set; }

        public virtual DbSet<ServiceOrder> ServiceOrders { get; set; }

        public virtual DbSet<ServiceOrderDetail> ServiceOrderDetails { get; set; }

        public virtual DbSet<ServiceType> ServiceTypes { get; set; }

        public virtual DbSet<Tasks> Tasks { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UsersWallet> UsersWallets { get; set; }

        public virtual DbSet<WebManager> WebManagers { get; set; }

        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(assembly: Assembly.GetExecutingAssembly());
        }
    }
}
