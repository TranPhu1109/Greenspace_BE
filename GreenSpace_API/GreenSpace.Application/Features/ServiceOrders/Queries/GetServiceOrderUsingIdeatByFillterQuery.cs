using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Application.ViewModels.ServiceOrder;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ServiceOrders.Queries
{
    public class GetServiceOrderUsingIdeatByFillterQuery : IRequest<PaginatedList<ServiceOrderViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Phone { get; set; }   
        public string? Username { get; set; }
        public int? Status { get; set; }      
        public class QueryHandler : IRequestHandler<GetServiceOrderUsingIdeatByFillterQuery, PaginatedList<ServiceOrderViewModel>>
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<QueryHandler> logger;

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<QueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                this.logger = logger;
            }

            public async Task<PaginatedList<ServiceOrderViewModel>> Handle(GetServiceOrderUsingIdeatByFillterQuery request, CancellationToken cancellationToken)
            {
                var serviceOrders = await _unitOfWork.ServiceOrderRepository.SearchUsingIdea(request.Phone, request.Username, request.Status);
                if (serviceOrders.Count == 0) throw new NotFoundException("There are no OrderService in DB!");
                var viewModels = _mapper.Map<List<ServiceOrderViewModel>>(serviceOrders);

                return PaginatedList<ServiceOrderViewModel>.Create(
                            source: viewModels.AsQueryable(),
                            pageIndex: request.PageNumber,
                            pageSize: request.PageSize
                    );
            }
        }
    }
}
