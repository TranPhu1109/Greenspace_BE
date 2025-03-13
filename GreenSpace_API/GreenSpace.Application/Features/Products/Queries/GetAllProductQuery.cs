using AutoMapper;
using GreenSpace.Application.Data;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Products.Queries
{
    public class GetAllProductQuery : IRequest<PaginatedList<ProductViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetAllProductQuery, PaginatedList<ProductViewModel>>
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

            public async Task<PaginatedList<ProductViewModel>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
            {



                var products = await _unitOfWork.ProductRepository.GetAllAsync(x => x.Image, x => x.Category);
                if (products.Count == 0) throw new NotFoundException("There are no product in DB!");
                var viewModels = _mapper.Map<List<ProductViewModel>>(products);

                return PaginatedList<ProductViewModel>.Create(
                            source: viewModels.AsQueryable(),
                            pageIndex: request.PageNumber,
                            pageSize: request.PageSize
                    );
            }
        }
    }
}
