using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.DesignIdeasCategory
{
    public class DesignIdeasCategoryUpdateModel
    {

        public string Name { get; set; } = default!;
        public string Description { get; set; } = string.Empty;
    }
}
