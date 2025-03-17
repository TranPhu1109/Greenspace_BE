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
    public class CreateCategoryCommand : IRequest<CategoryViewModel>
    {
        public CategoryCreateModel CreateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<CreateCategoryCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            }
        }
        public class CommandHandler : IRequestHandler<CreateCategoryCommand, CategoryViewModel>
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
            public async Task<CategoryViewModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Create Cate:\n");
                var cate = _mapper.Map<Category>(request.CreateModel);
                cate.Id = Guid.NewGuid();
                await _unitOfWork.CategoryRepository.AddAsync(cate);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<CategoryViewModel>(cate);
            }
        }
    }
}
