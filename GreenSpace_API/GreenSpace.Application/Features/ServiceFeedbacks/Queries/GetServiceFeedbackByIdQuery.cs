using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
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
    public class GetServiceFeedbackByIdQuery : IRequest<ServiceFeedbackViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetServiceFeedbackByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }
        public class QueryHandler : IRequestHandler<GetServiceFeedbackByIdQuery, ServiceFeedbackViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<QueryHandler> _logger;

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<QueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
            }
            public async Task<ServiceFeedbackViewModel> Handle(GetServiceFeedbackByIdQuery request, CancellationToken cancellationToken)
            {
                var feed = await _unitOfWork.ServiceFeedbackRepositoy.GetByIdAsync(request.Id, p => p.DesignIdea, p => p.User);
                if (feed is null) throw new NotFoundException($"FeedBack with ID-{request.Id} is not exist!");
                return _mapper.Map<ServiceFeedbackViewModel>(feed);
            }
        }
    }
}
