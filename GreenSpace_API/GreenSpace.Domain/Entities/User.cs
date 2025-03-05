using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Entities
{
    public class User : BaseEntity
    {
        [Key]
        public Guid UserId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public int? RoleId { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public bool? Status { get; set; }

        public string? AvatarUrl { get; set; }

        public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

        public virtual ICollection<MaterialFeedback> MaterialFeedbacks { get; set; } = new List<MaterialFeedback>();

        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public virtual Role? Role { get; set; }

        public virtual ICollection<ServiceFeedback> ServiceFeedbacks { get; set; } = new List<ServiceFeedback>();

        public virtual ICollection<WorkTask> WorkTask { get; set; } = new List<WorkTask>();

        public virtual UsersWallet? UsersWallet { get; set; }

    }
}
