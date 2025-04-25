using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Banner;
using GreenSpace.Application.ViewModels.ComplaintReason;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ComplaintReasons.Queries
{
    public class GetAllReasonQuery : IRequest<List<ComplaintReasonViewModel>>
    {

        public class QueryHandler : IRequestHandler<GetAllReasonQuery, List<ComplaintReasonViewModel>>
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

            public async Task<List<ComplaintReasonViewModel>> Handle(GetAllReasonQuery request, CancellationToken cancellationToken)
            {
                var reasons = await _unitOfWork.ComplaintReasonRepository.GetAllAsync();
                if (reasons.Count == 0) throw new NotFoundException("There are no ComplaintReason in the database!");

                var viewModels = _mapper.Map<List<ComplaintReasonViewModel>>(reasons);

                return viewModels;
            }
        }
    }
}
