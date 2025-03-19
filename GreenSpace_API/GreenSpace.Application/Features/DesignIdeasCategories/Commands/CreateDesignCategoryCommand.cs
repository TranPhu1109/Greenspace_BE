using AutoMapper;
using FluentValidation;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.DesignIdeasCategory;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.DesignIdeasCategories.Commands
{
    public class CreateDesignCategoryCommand : IRequest<DesignIdeasCategoryViewModel>
    {
        public DesignIdeasCategoryCreateModel CreateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<CreateDesignCategoryCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("Name must not null or empty");
            }
        }
        public class CommandHandler : IRequestHandler<CreateDesignCategoryCommand, DesignIdeasCategoryViewModel>
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
            public async Task<DesignIdeasCategoryViewModel> Handle(CreateDesignCategoryCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Create design Cate:\n");
                var cate = _mapper.Map<DesignIdeasCategory>(request.CreateModel);
                cate.Id = Guid.NewGuid();
                await _unitOfWork.DesignIdeasCategoryRepository.AddAsync(cate);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<DesignIdeasCategoryViewModel>(cate);
            }
        }
    }
}
