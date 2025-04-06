using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Images;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Products.Commands
{
    public class UpdateProductCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ProductUpdateModel UpdateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<UpdateProductCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.UpdateModel.Name).NotNull().NotEmpty().WithMessage("Name must not be null or empty");

                RuleFor(x => x.UpdateModel.CategoryId).NotNull().WithMessage("CategoryId must not be empty");

                RuleFor(x => x.UpdateModel.Price).GreaterThan(0).WithMessage("Price must be greater than zero");

                RuleFor(x => x.UpdateModel.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock must be zero or greater");

                RuleFor(x => x.UpdateModel.Description).NotNull().NotEmpty().WithMessage("Description must not be null or empty");

                RuleFor(x => x.UpdateModel.Size).GreaterThan(0).WithMessage("Size must be greater than zero");

                RuleFor(x => x.UpdateModel.CategoryId).NotNull().WithMessage("Category must not be null");

            }
        }
        public class CommandHandler : IRequestHandler<UpdateProductCommand, bool>
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

            public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {

                var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id, p => p.Image,p => p.ProductDetails);
                if (product is null)throw new NotFoundException($"Product with Id {request.Id} does not exist!");

                //  cập nhật ảnh 
                if (request.UpdateModel.Image is not null)
                {
                        product.Image.ImageUrl = !string.IsNullOrEmpty(request.UpdateModel.Image.ImageUrl) ? request.UpdateModel.Image.ImageUrl : product.Image.ImageUrl;
                        product.Image.Image2 = !string.IsNullOrEmpty(request.UpdateModel.Image.Image2) ? request.UpdateModel.Image.Image2 : product.Image.Image2;
                        product.Image.Image3 = !string.IsNullOrEmpty(request.UpdateModel.Image.Image3) ? request.UpdateModel.Image.Image3 : product.Image.Image3;                   
                    
                }
                if (request.UpdateModel.Price != product.Price)
                {
                    
                    product.Price = request.UpdateModel.Price;

                    // Cập nhật giá trong ProductDetail 
                    foreach (var productDetail in product.ProductDetails)
                    {
                        productDetail.Price = product.Price * productDetail.Quantity;
                        //_unitOfWork.ProductDetailRepository.Update(productDetail);
                    }

                }
                     
                _mapper.Map(request.UpdateModel, product);
                _unitOfWork.ProductRepository.Update(product);

                var result = await _unitOfWork.SaveChangesAsync() ;

                bool designIdeaUpdated = false;
                var designIdeaIds = product.ProductDetails.Select(pd => pd.DesignIdeaId).Distinct().ToList();
                foreach (var designIdeaId in designIdeaIds)
                {
                    var productDetails = await _unitOfWork.ProductDetailRepository.WhereAsync(pd => pd.DesignIdeaId == designIdeaId);

                    var materialPrice = productDetails.Sum(pd => pd.Price);

                    var designIdea = await _unitOfWork.DesignIdeaRepository.GetByIdAsync(designIdeaId);
                    if (designIdea != null)
                    {
                        designIdea.MaterialPrice = materialPrice;
                        designIdea.TotalPrice = designIdea.DesignPrice + designIdea.MaterialPrice;
                        _unitOfWork.DesignIdeaRepository.Update(designIdea);
                        designIdeaUpdated = true;
                    }
                }

                if (designIdeaUpdated)
                {
                    var result1 = await _unitOfWork.SaveChangesAsync();
                    return result1; 
                }


                return result;
            }


        }

    }
}
