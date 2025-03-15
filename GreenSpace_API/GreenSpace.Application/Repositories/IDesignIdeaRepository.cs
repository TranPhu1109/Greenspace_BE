using GreenSpace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Repositories
{
    public interface IDesignIdeaRepository : IGenericRepository<DesignIdea>
    {
        Task<List<DesignIdea>> Search(string? cate, string? name, float? minPrice, float? maxPrice);

    }
}
