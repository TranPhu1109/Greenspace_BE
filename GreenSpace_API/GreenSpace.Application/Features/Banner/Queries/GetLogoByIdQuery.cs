using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Banner;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Banner.Queries
{
    public class GetLogoByIdQuery : IRequest<LogoViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetLogoByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetLogoByIdQuery, LogoViewModel>
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
            public async Task<LogoViewModel> Handle(GetLogoByIdQuery request, CancellationToken cancellationToken)
            {
                var logo = await _unitOfWork.WebManagerRepository.GetByIdAsync(request.Id);
                if (logo is null) throw new NotFoundException($"Logo with ID-{request.Id} is not exist!");
                var result = _mapper.Map<LogoViewModel>(logo);
                return result;
            }
        }
    }
}
