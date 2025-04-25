using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Banner;
using GreenSpace.Application.ViewModels.ComplaintReason;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ComplaintReasons.Queries
{
    public class GetReasonByIdQuery : IRequest<ComplaintReasonViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetReasonByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetReasonByIdQuery, ComplaintReasonViewModel>
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
            public async Task<ComplaintReasonViewModel> Handle(GetReasonByIdQuery request, CancellationToken cancellationToken)
            {
                var reason = await _unitOfWork.ComplaintReasonRepository.GetByIdAsync(request.Id);
                if (reason is null) throw new NotFoundException($"ComplaintReason with ID-{request.Id} is not exist!");
                var result = _mapper.Map<ComplaintReasonViewModel>(reason);
                return result;
            }
        }
    }
}
