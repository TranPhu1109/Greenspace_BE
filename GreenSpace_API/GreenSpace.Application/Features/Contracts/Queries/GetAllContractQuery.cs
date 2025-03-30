using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels;
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
    public class GetAllContractQuery : IRequest<PaginatedList<ContractViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetAllContractQuery, PaginatedList<ContractViewModel>>
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

            public async Task<PaginatedList<ContractViewModel>> Handle(GetAllContractQuery request, CancellationToken cancellationToken)
            {



                var contracts = await _unitOfWork.ContractRepository.GetAllAsync(x => x.User);
                if (contracts.Count == 0) throw new NotFoundException("There are no contract in DB!");
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
