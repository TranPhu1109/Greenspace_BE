using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Categories.Commands
{
    public class UpdateCategoryCommand : IRequest<bool>
    {
        public CategoryUpdateModel UpdateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<UpdateCategoryCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.UpdateModel.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
                RuleFor(x => x.UpdateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            }
        }

        public class CommandHandler : IRequestHandler<UpdateCategoryCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;
            public CommandHandler(IUnitOfWork unitOfWork,
                    ILogger<CommandHandler> logger,
                    IMapper mapper,
                    AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _mapper = mapper;
                _appSettings = appSettings;
            }

            public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update Menu:\n");
                var cate = await _unitOfWork.CategoryRepository.GetByIdAsync(request.UpdateModel.Id);
                if (cate is null) throw new NotFoundException($"Category with Id-{request.UpdateModel.Id} is not exist!");
                _mapper.Map(request.UpdateModel, cate);
                _unitOfWork.CategoryRepository.Update(cate);
                var result = await _unitOfWork.SaveChangesAsync();
                return result;
            }
        }
    }
}
