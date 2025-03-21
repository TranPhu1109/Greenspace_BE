using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Application.ViewModels.RecordSketch;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.RecordSketchs.Queries
{
    public class GetRecordSketchByServiceOrderIdQuery : IRequest<PaginatedList<RecordSketchViewModel>>
    {
        public Guid ServiceOrderId { get; set; } = default!;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class QueryValidation : AbstractValidator<GetRecordSketchByServiceOrderIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.ServiceOrderId)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("ServiceOrderId must not be null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetRecordSketchByServiceOrderIdQuery, PaginatedList<RecordSketchViewModel>>
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

            public async Task<PaginatedList<RecordSketchViewModel>> Handle(GetRecordSketchByServiceOrderIdQuery request, CancellationToken cancellationToken)
            {
                var recordSketch = await _unitOfWork.RecordSketchRepository.WhereAsync(x => x.ServiceOrderId == request.ServiceOrderId, x => x.Image);
                if (recordSketch == null || !recordSketch.Any())
                {
                    throw new NotFoundException($"No recordSketch found for ServiceOrderId{request.ServiceOrderId}.");
                }
                var viewModels = _mapper.Map<List<RecordSketchViewModel>>(recordSketch);
                return PaginatedList<RecordSketchViewModel>.Create(
                    source: viewModels.AsQueryable(),
                    pageIndex: request.PageNumber,
                    pageSize: request.PageSize
                );
            }
        }
    }
}
