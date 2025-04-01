using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.MongoDbs.Carts;

public class CartCreateModel
{
    public Guid UserId { get; set; }
    public List<CartItemCreateModel>? Items { get; set; }
}
