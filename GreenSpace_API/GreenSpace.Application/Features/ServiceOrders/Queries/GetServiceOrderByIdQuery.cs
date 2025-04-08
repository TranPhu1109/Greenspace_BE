using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
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
    public class GetServiceOrderByIdQuery : IRequest<ServiceOrderViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetServiceOrderByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetServiceOrderByIdQuery, ServiceOrderViewModel>
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
            public async Task<ServiceOrderViewModel> Handle(GetServiceOrderByIdQuery request, CancellationToken cancellationToken)
            {
                var orderServices = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(request.Id, x => x.Image, x => x.ServiceOrderDetails,x=>x.User,x => x.WorkTask);
                if (orderServices is null) throw new NotFoundException($"Product with ID-{request.Id} is not exist!");
                var result = _mapper.Map<ServiceOrderViewModel>(orderServices);
                return result;
            }
        }
    }
}
