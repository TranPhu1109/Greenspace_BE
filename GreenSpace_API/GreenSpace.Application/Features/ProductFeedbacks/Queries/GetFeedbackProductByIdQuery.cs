using AutoMapper;
using FluentValidation;
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
    public class GetFeedbackProductByIdQuery : IRequest<ProductFeedbackViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetFeedbackProductByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }
        public class QueryHandler : IRequestHandler<GetFeedbackProductByIdQuery, ProductFeedbackViewModel>
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
            public async Task<ProductFeedbackViewModel> Handle(GetFeedbackProductByIdQuery request, CancellationToken cancellationToken)
            {
                var feed = await _unitOfWork.ProductFeedbackRepository.GetByIdAsync(request.Id,p => p.Product, p => p.User);
                if (feed is null) throw new NotFoundException($"FeedBack with ID-{request.Id} is not exist!");
                return _mapper.Map<ProductFeedbackViewModel>(feed);
            }
        }
    }
}
