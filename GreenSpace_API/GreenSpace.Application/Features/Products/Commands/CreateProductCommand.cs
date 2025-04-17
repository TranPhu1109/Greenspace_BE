using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenSpace.Domain.Entities;
using GreenSpace.Application.ViewModels.Products;
using static System.Net.Mime.MediaTypeNames;
using GreenSpace.Application.ViewModels.Images;

namespace GreenSpace.Application.Features.Products.Commands
{
    public class CreateProductCommand : IRequest<ProductViewModel>
    {
        public ProductCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateProductCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.Name).NotNull().NotEmpty().WithMessage("Name must not be null or empty");

                RuleFor(x => x.CreateModel.CategoryId).NotNull().WithMessage("CategoryId must not be empty");

                RuleFor(x => x.CreateModel.Price).GreaterThan(0).WithMessage("Price must be greater than zero");

                RuleFor(x => x.CreateModel.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock must be zero or greater");

                RuleFor(x => x.CreateModel.Description).NotNull().NotEmpty().WithMessage("Description must not be null or empty");

                RuleFor(x => x.CreateModel.Size).GreaterThan(0).WithMessage("Size must be greater than zero");

                RuleFor(x => x.CreateModel.CategoryId).NotNull().WithMessage("Category must not be null");

            }
        }
        public class CommandHandler : IRequestHandler<CreateProductCommand, ProductViewModel>
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


            public async Task<ProductViewModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Creating a new product: {ProductName}", request.CreateModel.Name);

                // Kiểm tra tên sản phẩm đã tồn tại
                var existProduct = await _unitOfWork.ProductRepository.FirstOrDefaultAsync(p => p.Name.ToLower() == request.CreateModel.Name.ToLower());

                if (existProduct != null)
                {

                    throw new InvalidOperationException($"Product with name '{request.CreateModel.Name}' already exists.");
                }

                // Tạo mới Image
                var image = _mapper.Map<Domain.Entities.Image>(request.CreateModel.Image);
                image.Id = Guid.NewGuid();
                await _unitOfWork.ImageRepository.AddAsync(image);

                // Tạo mới Product
                var product = _mapper.Map<Product>(request.CreateModel);
                product.Id = Guid.NewGuid();
                product.ImageId = image.Id; 

                await _unitOfWork.ProductRepository.AddAsync(product);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);

                
                var productViewModel = _mapper.Map<ProductViewModel>(product);
         

                return productViewModel;
            }


        }
    }
}
