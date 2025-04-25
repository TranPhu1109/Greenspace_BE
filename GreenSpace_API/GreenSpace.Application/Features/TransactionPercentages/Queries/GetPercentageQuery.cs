using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Banner;
using GreenSpace.Application.ViewModels.TransactionPercentage;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.TransactionPercentages.Queries
{
    public class GetPercentageQuery : IRequest<TransactionPercentageViewModel>
    {

        public class QueryHandler : IRequestHandler<GetPercentageQuery, TransactionPercentageViewModel>
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

            public async Task<TransactionPercentageViewModel> Handle(GetPercentageQuery request, CancellationToken cancellationToken)
            {
                var percentages = await _unitOfWork.TransactionPercentageRepository.GetAllAsync();

                if (percentages.Count == 0)
                    throw new NotFoundException("There are no percentages in the database!");

                var tage =  percentages.First();
                var viewModel = _mapper.Map<TransactionPercentageViewModel>(tage);

                return viewModel;
            }
        }
    }
}
