using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Complaints;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Complaints.Queries
{
    public class GetAllComplainStatusRefundQuery : IRequest<PaginatedList<ComplaintViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public class QueryValidation : AbstractValidator<GetAllComplainStatusRefundQuery>
        {
            public QueryValidation()
            {

            }
        }

        public class QueryHandler : IRequestHandler<GetAllComplainStatusRefundQuery, PaginatedList<ComplaintViewModel>>
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

            public async Task<PaginatedList<ComplaintViewModel>> Handle(GetAllComplainStatusRefundQuery request, CancellationToken cancellationToken)
            {
                var complaint = await _unitOfWork.ComplaintRepository.WhereAsync(x => x.Status == (int)ComplaintStatusEnum.refund, x => x.User, x => x.Image);
                if (complaint == null || !complaint.Any())
                {
                    throw new NotFoundException($"There are no complaint in DB.");
                }
                var viewModels = _mapper.Map<List<ComplaintViewModel>>(complaint);
                return PaginatedList<ComplaintViewModel>.Create(
                    source: viewModels.AsQueryable(),
                    pageIndex: request.PageNumber,
                    pageSize: request.PageSize
                );
            }
        }
    }
}
