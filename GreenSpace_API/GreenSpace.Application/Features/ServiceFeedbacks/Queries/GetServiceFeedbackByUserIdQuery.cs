using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.ProductFeedback;
using GreenSpace.Application.ViewModels.ServiceFeedbacks;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ServiceFeedbacks.Queries
{
    public class GetServiceFeedbackByUserIdQuery : IRequest<PaginatedList<ServiceFeedbackViewModel>>
    {
        public Guid UserId { get; set; } = default!;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class QueryValidation : AbstractValidator<GetServiceFeedbackByUserIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.UserId).NotNull().NotEmpty().WithMessage("User ID must not be null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetServiceFeedbackByUserIdQuery, PaginatedList<ServiceFeedbackViewModel>>
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

            public async Task<PaginatedList<ServiceFeedbackViewModel>> Handle(GetServiceFeedbackByUserIdQuery request, CancellationToken cancellationToken)
            {
                var serviceFeedbacks = await _unitOfWork.ServiceFeedbackRepositoy.WhereAsync(x => x.UserId == request.UserId, x => x.DesignIdeaId ,x => x.User);
                if (serviceFeedbacks == null || !serviceFeedbacks.Any())
                {
                    throw new NotFoundException($"No serviceFeedback found for User ID {request.UserId}.");
                }
                var viewModels = _mapper.Map<List<ServiceFeedbackViewModel>>(serviceFeedbacks);
                return PaginatedList<ServiceFeedbackViewModel>.Create(
                    source: viewModels.AsQueryable(),
                    pageIndex: request.PageNumber,
                    pageSize: request.PageSize
                );
            }
        }
    }
}
