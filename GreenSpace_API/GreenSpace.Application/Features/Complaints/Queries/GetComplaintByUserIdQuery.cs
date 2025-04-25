using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Complaints;
using GreenSpace.Application.ViewModels.WorkTasks;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Complaints.Queries
{
    public class GetComplaintByUserIdQuery : IRequest<List<ComplaintViewModel>>
    {
        public Guid UserId { get; set; } = default!;


        public class QueryValidation : AbstractValidator<GetComplaintByUserIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.UserId)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("User ID must not be null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetComplaintByUserIdQuery, List<ComplaintViewModel>>
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

            public async Task<List<ComplaintViewModel>> Handle(GetComplaintByUserIdQuery request, CancellationToken cancellationToken)
            {
                var complaints = await _unitOfWork.ComplaintRepository.WhereAsync(x => x.UserId == request.UserId, x => x.Image, x => x.User, x => x.ServiceOrder, x => x.Order, x => x.ComplaintDetails, x => x.ComplaintReason);
                if (complaints == null || !complaints.Any())
                {
                    throw new NotFoundException($"No Tasks found for User ID {request.UserId}.");
                }
                var result = _mapper.Map<List<ComplaintViewModel>>(complaints);
                return result;

            }
        }
    }
}
