using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Blogs;
using GreenSpace.Application.ViewModels.ExternalProduct;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ExternalProduct.Queries
{
    public class GetExternalProductByIdQuery : IRequest<ExternalProductsViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetExternalProductByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetExternalProductByIdQuery, ExternalProductsViewModel>
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
            public async Task<ExternalProductsViewModel> Handle(GetExternalProductByIdQuery request, CancellationToken cancellationToken)
            {
                var external = await _unitOfWork.ExternalProductsRepository.GetByIdAsync(request.Id);
                if (external is null) throw new NotFoundException($"ExternalProducts with ID-{request.Id} is not exist!");
                var result = _mapper.Map<ExternalProductsViewModel>(external);
                return result;
            }
        }
    }
}
