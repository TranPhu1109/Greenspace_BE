using System.ComponentModel.DataAnnotations;

namespace GreenSpace.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = default!;

        public Guid RoleId { get; set; }

        public string? Phone { get; set; } = default!;

        public string? Address { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        public ICollection<ProductFeedback> MaterialFeedbacks { get; set; } = new List<ProductFeedback>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public ICollection<ServiceOrder> ServiceOrders { get; set; } = new List<ServiceOrder>();

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public Role Role { get; set; } = default!;

        public ICollection<ServiceFeedback> ServiceFeedbacks { get; set; } = new List<ServiceFeedback>();

        public ICollection<WorkTask> WorkTask { get; set; } = new List<WorkTask>();

        public Guid WalletId { get; set; }
        public UsersWallet UsersWallet { get; set; } = default!;

    }
}
