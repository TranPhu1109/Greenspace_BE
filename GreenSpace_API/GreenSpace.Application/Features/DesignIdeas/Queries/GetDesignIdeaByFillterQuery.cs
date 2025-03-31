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
    public class GetDesignIdeaByFillterQuery : IRequest<PaginatedList<DesignIdeaViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Category { get; set; }
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public class QueryHandler : IRequestHandler<GetDesignIdeaByFillterQuery, PaginatedList<DesignIdeaViewModel>>
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

            public async Task<PaginatedList<DesignIdeaViewModel>> Handle(GetDesignIdeaByFillterQuery request, CancellationToken cancellationToken)
            {



                var designIdeas = await _unitOfWork.DesignIdeaRepository.Search(request.Category, request.Name, request.MinPrice, request.MaxPrice);
                if (designIdeas.Count == 0) throw new NotFoundException("There are no DesignIdea in DB!");
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
