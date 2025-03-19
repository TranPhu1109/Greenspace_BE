using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.ProductFeedback;
using GreenSpace.Application.ViewModels.ServiceFeedbacks;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ServiceFeedbacks.Queries
{
    public class GetAllServiceFeedbackQuery : IRequest<PaginatedList<ServiceFeedbackViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetAllServiceFeedbackQuery, PaginatedList<ServiceFeedbackViewModel>>
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
            public async Task<PaginatedList<ServiceFeedbackViewModel>> Handle(GetAllServiceFeedbackQuery request, CancellationToken cancellationToken)
            {
                var feedbacks = await _unitOfWork.ServiceFeedbackRepositoy.GetAllAsync(p => p.DesignIdea, p => p.User);
                if (feedbacks.Count == 0) throw new NotFoundException("There are no Feedback in the database!");
                var viewModels = _mapper.Map<List<ServiceFeedbackViewModel>>(feedbacks);

                return PaginatedList<ServiceFeedbackViewModel>.Create(
                            source: viewModels.AsQueryable(),
                            pageIndex: request.PageNumber,
                            pageSize: request.PageSize
                    );

            }
        }
    }
}
