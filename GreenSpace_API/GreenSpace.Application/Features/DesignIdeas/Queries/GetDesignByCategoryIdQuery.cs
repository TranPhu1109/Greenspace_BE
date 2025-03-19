using AutoMapper;
using FluentValidation;
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
    public class GetDesignByCategoryIdQuery : IRequest<PaginatedList<DesignIdeaViewModel>>
    {
        public Guid CategoryId { get; set; } = default!;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class QueryValidation : AbstractValidator<GetDesignByCategoryIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.CategoryId)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Category ID must not be null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetDesignByCategoryIdQuery, PaginatedList<DesignIdeaViewModel>>
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

            public async Task<PaginatedList<DesignIdeaViewModel>> Handle(GetDesignByCategoryIdQuery request, CancellationToken cancellationToken)
            {
                var designs = await _unitOfWork.DesignIdeaRepository.WhereAsync(x => x.DesignIdeasCategoryId == request.CategoryId, x => x.DesignIdeasCategory, x => x.Image,x => x.ProductDetails);
                if (designs == null || !designs.Any())
                {
                    throw new NotFoundException($"No designideas found for Category ID {request.CategoryId}.");
                }
                var viewModels = _mapper.Map<List<DesignIdeaViewModel>>(designs);
                return PaginatedList<DesignIdeaViewModel>.Create(
                    source: viewModels.AsQueryable(),
                    pageIndex: request.PageNumber,
                    pageSize: request.PageSize
                );
            }
        }
    }
}

