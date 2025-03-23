using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels._3PartyShip
{
    public class ReturnOrderRequest
    {
        public required string[] OrderCodes { get; set; }
        public  required string Reason { get; set; }
    }
}
