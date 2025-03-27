using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Blogs;
using GreenSpace.Application.ViewModels.Products;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Blogs.Commands
{
    public class UpdateBlogCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public BlogCreateModel UpdateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<UpdateBlogCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.UpdateModel.Author).NotNull().NotEmpty().WithMessage("Name must not be null or empty");

                RuleFor(x => x.UpdateModel.Title).NotNull().WithMessage("Title must not be empty");

                RuleFor(x => x.UpdateModel.Description).NotNull().NotEmpty().WithMessage("Description must not be null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<UpdateBlogCommand, bool>
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

            public async Task<bool> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
            {

                var blog = await _unitOfWork.BlogRepository.GetByIdAsync(request.Id, p => p.Image);
                if (blog is null) throw new NotFoundException($"blog with Id {request.Id} does not exist!");

                //  cập nhật ảnh 
                if (request.UpdateModel.Image is not null)
                {
                    blog.Image.ImageUrl = !string.IsNullOrEmpty(request.UpdateModel.Image.ImageUrl) ? request.UpdateModel.Image.ImageUrl : blog.Image.ImageUrl;
                    blog.Image.Image2 = !string.IsNullOrEmpty(request.UpdateModel.Image.Image2) ? request.UpdateModel.Image.Image2 : blog.Image.Image2;
                    blog.Image.Image3 = !string.IsNullOrEmpty(request.UpdateModel.Image.Image3) ? request.UpdateModel.Image.Image3 : blog.Image.Image3;

                }
                _mapper.Map(request.UpdateModel, blog);
                _unitOfWork.BlogRepository.Update(blog);
                return await _unitOfWork.SaveChangesAsync();
            }
        }

    }
}
