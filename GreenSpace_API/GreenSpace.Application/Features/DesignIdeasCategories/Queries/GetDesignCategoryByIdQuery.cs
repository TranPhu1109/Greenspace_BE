using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.DesignIdeasCategory;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.DesignIdeasCategories.Queries
{

    public class GetDesignCategoryByIdQuery : IRequest<DesignIdeasCategoryViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetDesignCategoryByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }
        public class QueryHandler : IRequestHandler<GetDesignCategoryByIdQuery, DesignIdeasCategoryViewModel>
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
            public async Task<DesignIdeasCategoryViewModel> Handle(GetDesignCategoryByIdQuery request, CancellationToken cancellationToken)
            {
                var cate = await _unitOfWork.DesignIdeasCategoryRepository.GetByIdAsync(request.Id);
                if (cate is null) throw new NotFoundException($"Category with ID-{request.Id} is not exist!");
                return _mapper.Map<DesignIdeasCategoryViewModel>(cate);
            }
        }
    }
}
