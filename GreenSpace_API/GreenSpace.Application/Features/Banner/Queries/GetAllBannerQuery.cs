using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Banner;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Banner.Queries
{
    public class GetAllBannerQuery : IRequest<List<BannerViewModel>>
    {
     
        public class QueryHandler : IRequestHandler<GetAllBannerQuery, List<BannerViewModel>>
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<QueryHandler> logger;

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<QueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                this.logger = logger;
            }

            public async Task<List<BannerViewModel>> Handle(GetAllBannerQuery request, CancellationToken cancellationToken)
            {
                var banner = await _unitOfWork.WebManagerRepository.GetAllAsync();
                if (banner.Count == 0) throw new NotFoundException("There are no banners in the database!");

                var viewModels = _mapper.Map<List<BannerViewModel>>(banner);

                return viewModels; 
            }
        }
    }
}
