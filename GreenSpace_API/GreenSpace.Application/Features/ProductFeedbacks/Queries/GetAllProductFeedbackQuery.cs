using AutoMapper;
using GreenSpace.Application.Features.Categories.Queries;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.ProductFeedback;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ProductFeedbacks.Queries
{
    public class GetAllProductFeedbackQuery : IRequest<List<ProductFeedbackViewModel>>
    {
        public class QueryHandler : IRequestHandler<GetAllProductFeedbackQuery, List<ProductFeedbackViewModel>>
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
            public async Task<List<ProductFeedbackViewModel>> Handle(GetAllProductFeedbackQuery request, CancellationToken cancellationToken)
            {
                var feedbacks = await _unitOfWork.ProductFeedbackRepository.GetAllAsync(p => p.Product, p => p.User);
                if (feedbacks.Count == 0) throw new NotFoundException("There are no Feedback in the database!");
                return _mapper.Map<List<ProductFeedbackViewModel>>(feedbacks);
            }
        }
    }
}
