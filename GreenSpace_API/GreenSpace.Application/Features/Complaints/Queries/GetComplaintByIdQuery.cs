using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Blogs;
using GreenSpace.Application.ViewModels.Complaints;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Complaints.Queries
{
    public class GetComplaintByIdQuery : IRequest<ComplaintViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetComplaintByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetComplaintByIdQuery, ComplaintViewModel>
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
            public async Task<ComplaintViewModel> Handle(GetComplaintByIdQuery request, CancellationToken cancellationToken)
            {
                var complaint = await _unitOfWork.ComplaintRepository.GetByIdAsync(request.Id, x => x.Image , x =>x .User, x => x.ServiceOrder, x => x.Order);
                if (complaint is null) throw new NotFoundException($"Blog with ID-{request.Id} is not exist!");
                var result = _mapper.Map<ComplaintViewModel>(complaint);
                return result;
            }
        }
    }
}
