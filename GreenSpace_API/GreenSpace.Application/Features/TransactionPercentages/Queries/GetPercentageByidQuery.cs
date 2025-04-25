using AutoMapper;
using FluentValidation;
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
    public class GetPercentageByidQuery : IRequest<TransactionPercentageViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetPercentageByidQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetPercentageByidQuery, TransactionPercentageViewModel>
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
            public async Task<TransactionPercentageViewModel> Handle(GetPercentageByidQuery request, CancellationToken cancellationToken)
            {
                var tage = await _unitOfWork.TransactionPercentageRepository.GetByIdAsync(request.Id);
                if (tage is null) throw new NotFoundException($"percentages with ID-{request.Id} is not exist!");
                var result = _mapper.Map<TransactionPercentageViewModel>(tage);
                return result;
            }
        }
    }
}
