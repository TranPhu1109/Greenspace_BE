using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Contracts.Queries
{
    public class GetContractByServiceOrderIdQuery : IRequest<PaginatedList<ContractViewModel>>
    {
        public Guid ServiceOrderId { get; set; } = default!;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class QueryValidation : AbstractValidator<GetContractByServiceOrderIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.ServiceOrderId)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("ServiceOrder ID must not be null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetContractByServiceOrderIdQuery, PaginatedList<ContractViewModel>>
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

            public async Task<PaginatedList<ContractViewModel>> Handle(GetContractByServiceOrderIdQuery request, CancellationToken cancellationToken)
            {
                var contracts = await _unitOfWork.ContractRepository.WhereAsync(x => x.ServiceOrderId == request.ServiceOrderId, x => x.User);
                if (contracts == null || !contracts.Any())
                {
                    throw new NotFoundException($"No contracts found for ServiceOrder ID {request.ServiceOrderId}.");
                }
                var viewModels = _mapper.Map<List<ContractViewModel>>(contracts);
                return PaginatedList<ContractViewModel>.Create(
                    source: viewModels.AsQueryable(),
                    pageIndex: request.PageNumber,
                    pageSize: request.PageSize
                );
            }
        }
    }
}
