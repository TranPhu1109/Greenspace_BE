using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
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
    public class GetLogoQuery : IRequest<LogoViewModel>
    {

        public class QueryHandler : IRequestHandler<GetLogoQuery, LogoViewModel>
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

            public async Task<LogoViewModel> Handle(GetLogoQuery request, CancellationToken cancellationToken)
            {
                var logos = await _unitOfWork.WebManagerRepository.WhereAsync(x => x.ImageLogo != null);

                if (logos.Count == 0)
                    throw new NotFoundException("There are no logo in the database!");

                var logo = logos.First();
                var viewModel = _mapper.Map<LogoViewModel>(logo); 

                return viewModel;
            }
        }
    }
}
