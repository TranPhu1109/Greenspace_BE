using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.ProductFeedback;
using GreenSpace.Application.ViewModels.Products;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ProductFeedbacks.Queries
{
    public class GetFeedbackProductByUserIdQuery : IRequest<PaginatedList<ProductFeedbackViewModel>>
    {
        public Guid UserId { get; set; } = default!;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class QueryValidation : AbstractValidator<GetFeedbackProductByUserIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("User ID must not be null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetFeedbackProductByUserIdQuery, PaginatedList<ProductFeedbackViewModel>>
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

            public async Task<PaginatedList<ProductFeedbackViewModel>> Handle(GetFeedbackProductByUserIdQuery request, CancellationToken cancellationToken)
            {
                var productFeedbacks = await _unitOfWork.ProductFeedbackRepository.WhereAsync(x => x.UserId == request.UserId, x => x.Product);
                if (productFeedbacks == null || !productFeedbacks.Any())
                {
                    throw new NotFoundException($"No productFeedback found for User ID {request.UserId}.");
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

