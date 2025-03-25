using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Category;
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
    public class GetWorkTaskByUserIdQuery : IRequest<List<WorkTaskViewModel>>
    {
        public Guid UserId { get; set; } = default!;


        public class QueryValidation : AbstractValidator<GetWorkTaskByUserIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.UserId)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("User ID must not be null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetWorkTaskByUserIdQuery, List<WorkTaskViewModel>>
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

            public async Task<List<WorkTaskViewModel>> Handle(GetWorkTaskByUserIdQuery request, CancellationToken cancellationToken)
            {
                var task = await _unitOfWork.WorkTaskRepository.WhereAsync(x => x.UserId == request.UserId, x => x.ServiceOrder, x => x.ServiceOrder.Image, x => x.ServiceOrder.User, x => x.ServiceOrder.ServiceOrderDetails,x => x.User);
                if (task == null || !task.Any())
                {
                    throw new NotFoundException($"No Tasks found for User ID {request.UserId}.");
                }
                var result = _mapper.Map<List<WorkTaskViewModel>>(task);
                return result;

            }
        }
    }
}
