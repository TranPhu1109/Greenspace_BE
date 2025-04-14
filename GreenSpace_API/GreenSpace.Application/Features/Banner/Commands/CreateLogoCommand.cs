using AutoMapper;
using FluentValidation;
using GreenSpace.Application.ViewModels.Banner;
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
    public class CreateLogoCommand : IRequest<LogoViewModel>
    {
        public LogoCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateLogoCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.ImageLogo).NotNull().NotEmpty().WithMessage("Name must not be null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<CreateLogoCommand, LogoViewModel>
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


            public async Task<LogoViewModel> Handle(CreateLogoCommand request, CancellationToken cancellationToken)
            {
                var exist = await _unitOfWork.WebManagerRepository.WhereAsync(x => x.ImageLogo !=null);
                if (exist.Count > 0) throw new Exception("There are logo in the database exsit !");

                var logo = _mapper.Map<WebManager>(request.CreateModel);
                logo.Id = Guid.NewGuid();

                await _unitOfWork.WebManagerRepository.AddAsync(logo);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Logo created successfully with ID: {BannerId}", logo.Id);


                var ViewModel = _mapper.Map<LogoViewModel>(logo);


                return ViewModel;
            }


        }
    }
}
