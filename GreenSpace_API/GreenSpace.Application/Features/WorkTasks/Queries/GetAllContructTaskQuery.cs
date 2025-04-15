using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.WorkTasks;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.WorkTasks.Queries
{
    public class GetAllContructTaskQuery : IRequest<List<WorkTaskViewModel>>
    {
        public class QueryHandler : IRequestHandler<GetAllContructTaskQuery, List<WorkTaskViewModel>>
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
            public async Task<List<WorkTaskViewModel>> Handle(GetAllContructTaskQuery request, CancellationToken cancellationToken)
            {
                var tasks = await _unitOfWork.WorkTaskRepository.WhereAsync(x => x.User.Role.RoleName == nameof(RoleEnum.Contructor)); ;
                if (tasks.Count == 0) throw new NotFoundException("There are no task in the database!");
                return _mapper.Map<List<WorkTaskViewModel>>(tasks);
            }
        }
    }
}
