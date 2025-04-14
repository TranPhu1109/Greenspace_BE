using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Banner;
using GreenSpace.Application.ViewModels.Document;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Documents.Queries
{
    public class GetDocumentrByIdQuery : IRequest<DocumentViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetDocumentrByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetDocumentrByIdQuery, DocumentViewModel>
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
            public async Task<DocumentViewModel> Handle(GetDocumentrByIdQuery request, CancellationToken cancellationToken)
            {
                var policy = await _unitOfWork.DocumentRepository.GetByIdAsync(request.Id);
                if (policy is null) throw new NotFoundException($"Policy with ID-{request.Id} is not exist!");
                var result = _mapper.Map<DocumentViewModel>(policy);
                return result;
            }
        }
    }
}
