using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Banner;
using GreenSpace.Application.ViewModels.Blogs;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Banner.Commands
{
    public class UpdateBannerCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public BannerCreateModel UpdateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<UpdateBannerCommand>
        {
            public CommandValidation()
            {
             

            }
        }
        public class CommandHandler : IRequestHandler<UpdateBannerCommand, bool>
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

            public async Task<bool> Handle(UpdateBannerCommand request, CancellationToken cancellationToken)
            {

                var banner = await _unitOfWork.WebManagerRepository.GetByIdAsync(request.Id);
                if (banner is null) throw new NotFoundException($"blog with Id {request.Id} does not exist!");

                _mapper.Map(request.UpdateModel, banner);
                _unitOfWork.WebManagerRepository.Update(banner);
                return await _unitOfWork.SaveChangesAsync();
            }
        }

    }
}
