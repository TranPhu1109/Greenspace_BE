using GreenSpace.Application.ViewModels.ComplaintDetail;
using GreenSpace.Application.ViewModels.ServiceOrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Complaints
{
    public class ComplaintDetailsUpdateModel
    {
        public List<ComplaintDetailUpdateModel> ComplaintDetails { get; set; } = new List<ComplaintDetailUpdateModel>();
    }
}
