using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Entities
{
    public class Address : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string UserAddress { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
