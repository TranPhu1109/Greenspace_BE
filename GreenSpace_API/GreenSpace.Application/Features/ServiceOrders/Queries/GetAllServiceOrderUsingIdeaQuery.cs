using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Application.ViewModels.ServiceOrder;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ServiceOrders.Queries
{
    public class GetAllServiceOrderUsingIdeaQuery : IRequest<PaginatedList<ServiceOrderViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class QueryValidation : AbstractValidator<GetAllServiceOrderUsingIdeaQuery>
        {
            public QueryValidation()
            {

            }
        }

        public class QueryHandler : IRequestHandler<GetAllServiceOrderUsingIdeaQuery, PaginatedList<ServiceOrderViewModel>>
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

            public async Task<PaginatedList<ServiceOrderViewModel>> Handle(GetAllServiceOrderUsingIdeaQuery request, CancellationToken cancellationToken)
            {
                var ordersService = await _unitOfWork.ServiceOrderRepository.WhereAsync(x => x.ServiceType == ServiceTypeEnum.UsingDesignIdea.ToString(), x => x.User, x => x.Image,x => x.ServiceOrderDetails, x => x.WorkTask);
                if (ordersService == null || !ordersService.Any())
                {
                    throw new NotFoundException($"There are no ordersService in DB.");
                }
                var viewModels = _mapper.Map<List<ServiceOrderViewModel>>(ordersService);
                return PaginatedList<ServiceOrderViewModel>.Create(
                    source: viewModels.AsQueryable(),
                    pageIndex: request.PageNumber,
                    pageSize: request.PageSize
                );
            }
        }
    }
}

