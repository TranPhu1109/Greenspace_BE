using Microsoft.EntityFrameworkCore;
using System.Reflection;
using GreenSpace.Domain.Entities;

namespace GreenSpace.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        #region DbSets


        public  DbSet<Bill> Bills { get; set; }

        public  DbSet<Blog> Blogs { get; set; }

        public  DbSet<Category> Categories { get; set; }
        public DbSet<DesignIdeasCategory> DesignIdeasCategories { get; set; }

        public  DbSet<Contract> Contracts { get; set; }

        public  DbSet<DesignIdea> DesignIdeas { get; set; }

        public  DbSet<Document> Documents { get; set; }

        public  DbSet<Image> Images { get; set; }

        public  DbSet<Product> Materials { get; set; }

        public  DbSet<ProductFeedback> MaterialFeedbacks { get; set; }

        public  DbSet<Notification> Notifications { get; set; }

        public  DbSet<Order> Orders { get; set; }

        public  DbSet<OrderDetail> OrderDetails { get; set; }

        public  DbSet<Payment> Payments { get; set; }

        public  DbSet<ProductDetail> ProductDetails { get; set; }

        public  DbSet<RefreshToken> RefreshTokens { get; set; }

        public  DbSet<Role> Roles { get; set; }

        public  DbSet<ServiceFeedback> ServiceFeedbacks { get; set; }

        public  DbSet<ServiceOrder> ServiceOrders { get; set; }

        public  DbSet<ServiceOrderDetail> ServiceOrderDetails { get; set; }

        public  DbSet<ServiceType> ServiceTypes { get; set; }

        public  DbSet<WorkTask> Tasks { get; set; }

        public  DbSet<User> Users { get; set; }

        public  DbSet<UsersWallet> UsersWallets { get; set; }

        public  DbSet<WebManager> WebManagers { get; set; }
        public DbSet<RecordDesign> RecordDesigns { get; set; }
        public DbSet<RecordSketch> RecordSketches { get; set; }


        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(assembly: Assembly.GetExecutingAssembly());
        }
    }
}
