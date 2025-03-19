using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.DesignIdea;
using GreenSpace.Application.ViewModels.Products;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.DesignIdeas.Queries
{
    public class GetDesignIdeaByQuery : IRequest<DesignIdeaViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetDesignIdeaByQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetDesignIdeaByQuery, DesignIdeaViewModel>
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
            public async Task<DesignIdeaViewModel> Handle(GetDesignIdeaByQuery request, CancellationToken cancellationToken)
            {
                var design = await _unitOfWork.DesignIdeaRepository.GetByIdAsync(request.Id, x => x.Image, x => x.DesignIdeasCategory,x => x.ProductDetails);
                if (design is null) throw new NotFoundException($"DesignIdea with ID-{request.Id} is not exist!");
                var result = _mapper.Map<DesignIdeaViewModel>(design);
                return result;
            }
        }
    }
}
