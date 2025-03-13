using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Products.Queries
{
    public class GetProductByQuery : IRequest<ProductViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetProductByQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetProductByQuery, ProductViewModel>
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
            public async Task<ProductViewModel> Handle(GetProductByQuery request, CancellationToken cancellationToken)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id, x => x.Image, x => x.Category);
                if (product is null) throw new BadRequestException($"Product with ID-{request.Id} is not exist!");
                var result = _mapper.Map<ProductViewModel>(product);
                return result;
            }
        }
    }
}
