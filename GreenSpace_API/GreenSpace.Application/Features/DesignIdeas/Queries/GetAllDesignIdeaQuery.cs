using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.DesignIdea;
using GreenSpace.Application.ViewModels.Products;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.DesignIdeas.Queries
{
    public class GetAllDesignIdeaQuery : IRequest<PaginatedList<DesignIdeaViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetAllDesignIdeaQuery, PaginatedList<DesignIdeaViewModel>>
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

            public async Task<PaginatedList<DesignIdeaViewModel>> Handle(GetAllDesignIdeaQuery request, CancellationToken cancellationToken)
            {



                var designIdeas = await _unitOfWork.DesignIdeaRepository.GetAllAsync(x => x.Image, x => x.Category,x => x.ProductDetails);
                if (designIdeas.Count == 0) throw new NotFoundException("There are no designIdea in DB!");
                var viewModels = _mapper.Map<List<DesignIdeaViewModel>>(designIdeas);

                return PaginatedList<DesignIdeaViewModel>.Create(
                            source: viewModels.AsQueryable(),
                            pageIndex: request.PageNumber,
                            pageSize: request.PageSize
                    );
            }
        }
    }
}
