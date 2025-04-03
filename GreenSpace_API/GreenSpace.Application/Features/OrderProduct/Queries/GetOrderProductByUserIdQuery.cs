using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Contracts;
using GreenSpace.Application.ViewModels.OrderProducts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.OrderProduct.Queries
{

    public class GetOrderProductByUserIdQuery : IRequest<PaginatedList<OrderProductViewModel>>
    {

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class QueryValidation : AbstractValidator<GetOrderProductByUserIdQuery>
        {
            public QueryValidation()
            {
              
            }
        }

        public class QueryHandler : IRequestHandler<GetOrderProductByUserIdQuery, PaginatedList<OrderProductViewModel>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ILogger<QueryHandler> _logger;
            private readonly IClaimsService claimsService;

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<QueryHandler> logger, IClaimsService claimsService)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
                this.claimsService = claimsService;
            }

            public async Task<PaginatedList<OrderProductViewModel>> Handle(GetOrderProductByUserIdQuery request, CancellationToken cancellationToken)
            {
                var orders = await _unitOfWork.OrderRepository.WhereAsync(x => x.UserId == claimsService.GetCurrentUser, x => x.User , x => x.OrderDetails);
                if (orders == null || !orders.Any())
                {
                    throw new NotFoundException($"No Orders found for User ID {claimsService.GetCurrentUser}.");
                }
                var viewModels = _mapper.Map<List<OrderProductViewModel>>(orders);
                return PaginatedList<OrderProductViewModel>.Create(
                    source: viewModels.AsQueryable(),
                    pageIndex: request.PageNumber,
                    pageSize: request.PageSize
                );
            }
        }
    }
}
