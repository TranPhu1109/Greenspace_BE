using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Entities
{
    public class User : BaseEntity
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public char gentle { get; set; }
        public bool status { get; set; }

    }
}
