using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.RecordDesign;
using GreenSpace.Application.ViewModels.RecordSketch;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.RecordDesigns.Queries
{
    public class GetRecordDesignByServiceOrderIdQuery : IRequest<PaginatedList<RecordDesignViewModel>>
    {
        public Guid ServiceOrderId { get; set; } = default!;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class QueryValidation : AbstractValidator<GetRecordDesignByServiceOrderIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.ServiceOrderId)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("ServiceOrderId must not be null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetRecordDesignByServiceOrderIdQuery, PaginatedList<RecordDesignViewModel>>
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

            public async Task<PaginatedList<RecordDesignViewModel>> Handle(GetRecordDesignByServiceOrderIdQuery request, CancellationToken cancellationToken)
            {
                var record = await _unitOfWork.RecordDesignRepository.WhereAsync(x => x.ServiceOrderId == request.ServiceOrderId, x => x.Image);
                if (record == null || !record.Any())
                {
                    throw new NotFoundException($"No record Desing found for ServiceOrderId{request.ServiceOrderId}.");
                }
                var viewModels = _mapper.Map<List<RecordDesignViewModel>>(record);
                return PaginatedList<RecordDesignViewModel>.Create(
                    source: viewModels.AsQueryable(),
                    pageIndex: request.PageNumber,
                    pageSize: request.PageSize
                );
            }
        }
    }
}
