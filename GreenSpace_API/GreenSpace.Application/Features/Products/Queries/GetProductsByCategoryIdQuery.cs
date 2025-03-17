using AutoMapper;
using FluentValidation;
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
    namespace GreenSpace.Application.Features.Products.Queries
    {
        public class GetProductsByCategoryIdQuery : IRequest<PaginatedList<ProductViewModel>>
        {
            public Guid CategoryId { get; set; } = default!;
            public int PageNumber { get; set; }
            public int PageSize { get; set; }

            public class QueryValidation : AbstractValidator<GetProductsByCategoryIdQuery>
            {
                public QueryValidation()
                {
                    RuleFor(x => x.CategoryId)
                        .NotNull()
                        .NotEmpty()
                        .WithMessage("Category ID must not be null or empty");
                }
            }

            public class QueryHandler : IRequestHandler<GetProductsByCategoryIdQuery, PaginatedList<ProductViewModel>>
            {
                private readonly IUnitOfWork _unitOfWork;
                private readonly IMapper _mapper;
                private readonly ILogger<QueryHandler> _logger;

                public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<QueryHandler> logger)
                {
                    _unitOfWork = unitOfWork;
                    _mapper = mapper;
                    _logger = logger;
                }

                public async Task<PaginatedList<ProductViewModel>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
                {
                    var products = await _unitOfWork.ProductRepository.WhereAsync(x => x.CategoryId == request.CategoryId, x => x.Category,x => x.Image);
                    if (products == null || !products.Any())
                    {
                        throw new NotFoundException($"No products found for Category ID {request.CategoryId}.");
                    }
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
}
