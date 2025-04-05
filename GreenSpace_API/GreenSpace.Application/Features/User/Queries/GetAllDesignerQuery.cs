using AutoMapper;
using FluentValidation;
using GreenSpace.Application.Features.ServiceOrders.Queries;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.ServiceOrder;
using GreenSpace.Application.ViewModels.Users;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.User.Queries
{
    public class GetAllDesignerQuery : IRequest<PaginatedList<UserViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class QueryValidation : AbstractValidator<GetAllDesignerQuery>
        {
            public QueryValidation()
            {

            }
        }

        public class QueryHandler : IRequestHandler<GetAllDesignerQuery, PaginatedList<UserViewModel>>
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

            public async Task<PaginatedList<UserViewModel>> Handle(GetAllDesignerQuery request, CancellationToken cancellationToken)
            {
                var designer = await _unitOfWork.UserRepository.WhereAsync(x => x.Role.RoleName == RoleEnum.Designer.ToString() , x => x.Role);
                if (designer == null || !designer.Any())
                {
                    throw new NotFoundException($"There are no Designer in DB.");
                }
                var viewModels = _mapper.Map<List<UserViewModel>>(designer);
                return PaginatedList<UserViewModel>.Create(
                    source: viewModels.AsQueryable(),
                    pageIndex: request.PageNumber,
                    pageSize: request.PageSize
                );
            }
        }
    }
}
