using GreenSpace.Application.Repositories;
using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Domain.Entities;

namespace GreenSpace.Infrastructure.Repositories
{
    public class WebManagerRepository : GenericRepository<WebManager>, IWebManagerRepository
    {
        public WebManagerRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
        }
    }   
}
