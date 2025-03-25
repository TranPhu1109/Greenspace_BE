using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Application.ViewModels.WorkTasks;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.WorkTasks.Queries
{
    public class GetWorkTaskByIdQuery : IRequest<WorkTaskViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetWorkTaskByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetWorkTaskByIdQuery, WorkTaskViewModel>
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
            public async Task<WorkTaskViewModel> Handle(GetWorkTaskByIdQuery request, CancellationToken cancellationToken)
            {
                var task = await _unitOfWork.WorkTaskRepository.GetByIdAsync(request.Id, x => x.User,x => x.ServiceOrder.User, x => x.ServiceOrder,x => x.User,x => x.ServiceOrder.Image, x => x.ServiceOrder.ServiceOrderDetails);
                if (task is null) throw new NotFoundException($"task with ID-{request.Id} is not exist!");
                var result = _mapper.Map<WorkTaskViewModel>(task);
                return result;
            }
        }
    }
}
