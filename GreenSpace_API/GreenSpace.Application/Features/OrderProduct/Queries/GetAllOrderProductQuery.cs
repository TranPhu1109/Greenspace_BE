using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Blogs;
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
    public class GetAllOrderProductQuery : IRequest<PaginatedList<OrderProductViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetAllOrderProductQuery, PaginatedList<OrderProductViewModel>>
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

            public async Task<PaginatedList<OrderProductViewModel>> Handle(GetAllOrderProductQuery request, CancellationToken cancellationToken)
            {



                var orders = await _unitOfWork.OrderRepository.GetAllAsync(x => x.User);
                if (orders.Count == 0) throw new NotFoundException("There are no Order in DB!");
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
