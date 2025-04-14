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
    public class GetBannerByIdQuery : IRequest<BannerViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetBannerByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetBannerByIdQuery, BannerViewModel>
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
            public async Task<BannerViewModel> Handle(GetBannerByIdQuery request, CancellationToken cancellationToken)
            {
                var banner = await _unitOfWork.WebManagerRepository.GetByIdAsync(request.Id);
                if (banner is null) throw new NotFoundException($"Banner with ID-{request.Id} is not exist!");
                var result = _mapper.Map<BannerViewModel>(banner);
                return result;
            }
        }
    }
}
