using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
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
    public class GetOrderProductByIdQuery : IRequest<OrderProductViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetOrderProductByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetOrderProductByIdQuery, OrderProductViewModel>
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
            public async Task<OrderProductViewModel> Handle(GetOrderProductByIdQuery request, CancellationToken cancellationToken)
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(request.Id, x => x.User ,x => x.OrderDetails);
                if (order is null) throw new NotFoundException($"Order with ID-{request.Id} is not exist!");
                var result = _mapper.Map<OrderProductViewModel>(order);
                return result;
            }
        }
    }
}
