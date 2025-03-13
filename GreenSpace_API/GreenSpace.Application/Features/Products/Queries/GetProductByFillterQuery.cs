using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Products;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Products.Queries
{
    public class GetProductByFillterQuery : IRequest<PaginatedList<ProductViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Category { get; set; }
        public string? Name { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public class QueryHandler : IRequestHandler<GetProductByFillterQuery, PaginatedList<ProductViewModel>>
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

            public async Task<PaginatedList<ProductViewModel>> Handle(GetProductByFillterQuery request, CancellationToken cancellationToken)
            {



                var products = await _unitOfWork.ProductRepository.Search(request.Category, request.Name, request.MinPrice, request.MaxPrice );
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
