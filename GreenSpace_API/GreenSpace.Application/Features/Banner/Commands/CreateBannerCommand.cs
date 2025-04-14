using AutoMapper;
using FluentValidation;
using GreenSpace.Application.ViewModels.Banner;
using GreenSpace.Application.ViewModels.Blogs;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Banner.Commands
{
    public class CreateBannerCommand : IRequest<BannerViewModel>
    {
        public BannerCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateBannerCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.ImageBanner).NotNull().NotEmpty().WithMessage("Name must not be null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<CreateBannerCommand, BannerViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;

            public CommandHandler(IUnitOfWork unitOfWork,
                    IMapper mapper,
                    ILogger<CommandHandler> logger,
                    AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
                _appSettings = appSettings;
            }


            public async Task<BannerViewModel> Handle(CreateBannerCommand request, CancellationToken cancellationToken)
            {
              
                var banner = _mapper.Map<WebManager>(request.CreateModel);
                banner.Id = Guid.NewGuid();

                await _unitOfWork.WebManagerRepository.AddAsync(banner);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Banner created successfully with ID: {BannerId}", banner.Id);


                var ViewModel = _mapper.Map<BannerViewModel>(banner);


                return ViewModel;
            }


        }
    }
}
