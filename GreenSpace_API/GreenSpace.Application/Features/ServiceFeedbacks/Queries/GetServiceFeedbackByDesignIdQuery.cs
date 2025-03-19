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
    public class GetServiceFeedbackByDesignIdQuery : IRequest<PaginatedList<ServiceFeedbackViewModel>>
    {
        public Guid DesignId { get; set; } = default!;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class QueryValidation : AbstractValidator<GetServiceFeedbackByDesignIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.DesignId).NotNull().NotEmpty().WithMessage("User ID must not be null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetServiceFeedbackByDesignIdQuery, PaginatedList<ServiceFeedbackViewModel>>
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

            public async Task<PaginatedList<ServiceFeedbackViewModel>> Handle(GetServiceFeedbackByDesignIdQuery request, CancellationToken cancellationToken)
            {
                var Feedbacks = await _unitOfWork.ServiceFeedbackRepositoy.WhereAsync(x => x.DesignIdeaId == request.DesignId);
                if (Feedbacks == null || !Feedbacks.Any())
                {
                    throw new NotFoundException($"No productFeedback found for User ID {request.DesignId}.");
                }
                var viewModels = _mapper.Map<List<ServiceFeedbackViewModel>>(Feedbacks);
                return PaginatedList<ServiceFeedbackViewModel>.Create(
                    source: viewModels.AsQueryable(),
                    pageIndex: request.PageNumber,
                    pageSize: request.PageSize
                );
            }
        }
    }
}
