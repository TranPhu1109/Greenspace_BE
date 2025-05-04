using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Blogs;
using GreenSpace.Application.ViewModels.ExternalProduct;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ExternalProduct.Queries
{
    public class GetAllExternalProductQuery : IRequest<PaginatedList<ExternalProductsViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetAllExternalProductQuery, PaginatedList<ExternalProductsViewModel>>
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

            public async Task<PaginatedList<ExternalProductsViewModel>> Handle(GetAllExternalProductQuery request, CancellationToken cancellationToken)
            {



                var external = await _unitOfWork.ExternalProductsRepository.GetAllAsync();
                if (external.Count == 0) throw new NotFoundException("There are no ExternalProduct in DB!");
                var viewModels = _mapper.Map<List<ExternalProductsViewModel>>(external);

                return PaginatedList<ExternalProductsViewModel>.Create(
                            source: viewModels.AsQueryable(),
                            pageIndex: request.PageNumber,
                            pageSize: request.PageSize
                    );
            }
        }
    }
}
