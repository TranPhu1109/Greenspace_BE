using AutoMapper;
using FluentValidation;
using GreenSpace.Application.Features.Products.Commands;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.SignalR;
using GreenSpace.Application.ViewModels.DesignIdea;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.DesignIdeas.Commands
{
    public class CreateDesignIdeaCommand : IRequest<DesignIdeaViewModel>
    {
        public DesignIdeaCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateDesignIdeaCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("Name must not be null or empty");

                RuleFor(x => x.CreateModel.DesignIdeasCategoryId).NotNull().WithMessage("CategoryId must not be empty");

                RuleFor(x => x.CreateModel.DesignPrice).GreaterThan(0).WithMessage("Price must be greater than zero");

                RuleFor(x => x.CreateModel.Description).NotNull().NotEmpty().WithMessage("Description must not be null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<CreateDesignIdeaCommand, DesignIdeaViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;
            private readonly IHubContext<SignalrHub> _hubContext;
            public CommandHandler(IUnitOfWork unitOfWork,
                    ILogger<CommandHandler> logger,
                    IMapper mapper,
                    AppSettings appSettings,
                   IHubContext<SignalrHub> hubContext)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _mapper = mapper;
                _appSettings = appSettings;
                _hubContext = hubContext;
            }

            public async Task<DesignIdeaViewModel> Handle(CreateDesignIdeaCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Creating a new design idea:{DesignIdeaName}", request.CreateModel.Name);

                var image = _mapper.Map<Image>(request.CreateModel.Image);
                image.Id = Guid.NewGuid();
                await _unitOfWork.ImageRepository.AddAsync(image);

                var design = _mapper.Map<DesignIdea>(request.CreateModel);
                design.Id = Guid.NewGuid();
                design.ImageId = image.Id;

                //add table productDetails
                if (design.ProductDetails != null && design.ProductDetails.Any())
                {
                    var productList = design.ProductDetails.ToList();

                    foreach (var p in productList)
                    {
                        var product = await _unitOfWork.ProductRepository.GetByIdAsync(p.ProductId);
                        if (product == null) throw new NotFoundException($"Product with Id {p.ProductId} does not exist!");
                        p.Price = product.Price * p.Quantity;
                        p.Id = Guid.NewGuid();
                    }

                    var materialPrice = productList.Sum(p => p.Price); 
                    design.ProductDetails = productList;
                }

                design.MaterialPrice = design.ProductDetails?.Sum(p => p.Price) ?? 0;
                design.TotalPrice = request.CreateModel.DesignPrice + design.MaterialPrice;
                await _unitOfWork.DesignIdeaRepository.AddAsync(design);

                await _unitOfWork.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("messageReceived", "CreateDesign", $"{design.Id}");
                _logger.LogInformation("Design Idea created successfully with ID: {DesignIdeaId}", design.Id);

                var viewModle = _mapper.Map<DesignIdeaViewModel>(design);  

                return viewModle;
            }
        }
    }
}
