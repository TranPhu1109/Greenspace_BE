using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.ProductFeedback;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ProductFeedbacks.Queries
{
    public class GetFeedbackProductByProductIdQuery : IRequest<PaginatedList<ProductFeedbackViewModel>>
    {
        public Guid ProductId { get; set; } = default!;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class QueryValidation : AbstractValidator<GetFeedbackProductByProductIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.ProductId).NotNull().NotEmpty().WithMessage("Product ID must not be null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetFeedbackProductByProductIdQuery, PaginatedList<ProductFeedbackViewModel>>
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

            public async Task<PaginatedList<ProductFeedbackViewModel>> Handle(GetFeedbackProductByProductIdQuery request, CancellationToken cancellationToken)
            {
                var productFeedbacks = await _unitOfWork.ProductFeedbackRepository.WhereAsync(x => x.ProductId == request.ProductId);
                if (productFeedbacks == null || !productFeedbacks.Any())
                {
                    throw new NotFoundException($"No productFeedback found for product ID {request.ProductId}.");
                }
                var viewModels = _mapper.Map<List<ProductFeedbackViewModel>>(productFeedbacks);
                return PaginatedList<ProductFeedbackViewModel>.Create(
                    source: viewModels.AsQueryable(),
                    pageIndex: request.PageNumber,
                    pageSize: request.PageSize
                );
            }
        }
    }
}
