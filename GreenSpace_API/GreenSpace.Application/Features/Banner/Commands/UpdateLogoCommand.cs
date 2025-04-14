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

namespace GreenSpace.Application.Features.Banner.Commands
{
    public class UpdateLogoCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public LogoCreateModel UpdateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<UpdateLogoCommand>
        {
            public CommandValidation()
            {


            }
        }
        public class CommandHandler : IRequestHandler<UpdateLogoCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;



            public CommandHandler(IUnitOfWork unitOfWork,
                IMapper mapper, ILogger<CommandHandler> logger,
                AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
                _appSettings = appSettings;

            }

            public async Task<bool> Handle(UpdateLogoCommand request, CancellationToken cancellationToken)
            {

                var logo = await _unitOfWork.WebManagerRepository.GetByIdAsync(request.Id);
                if (logo is null) throw new NotFoundException($"blog with Id {request.Id} does not exist!");

                _mapper.Map(request.UpdateModel, logo);
                _unitOfWork.WebManagerRepository.Update(logo);
                return await _unitOfWork.SaveChangesAsync();
            }
        }

    }
}
