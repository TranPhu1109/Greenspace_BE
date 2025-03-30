using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Blogs;
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
    public class GetContractByIdQuery : IRequest<ContractViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetContractByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetContractByIdQuery, ContractViewModel>
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
            public async Task<ContractViewModel> Handle(GetContractByIdQuery request, CancellationToken cancellationToken)
            {
                var contract = await _unitOfWork.ContractRepository.GetByIdAsync(request.Id, x => x.User);
                if (contract is null) throw new NotFoundException($"contract with ID-{request.Id} is not exist!");
                var result = _mapper.Map<ContractViewModel>(contract);
                return result;
            }
        }
    }
}
