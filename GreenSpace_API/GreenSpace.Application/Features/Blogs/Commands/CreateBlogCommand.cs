using AutoMapper;
using FluentValidation;
using GreenSpace.Application.ViewModels.Blogs;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Blogs.Commands
{
    public class CreateBlogCommand : IRequest<BlogViewModel>
    {
        public BlogCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateBlogCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.Author).NotNull().NotEmpty().WithMessage("Name must not be null or empty");

                RuleFor(x => x.CreateModel.Title).NotNull().WithMessage("Title must not be empty");

                RuleFor(x => x.CreateModel.Description).NotNull().NotEmpty().WithMessage("Description must not be null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<CreateBlogCommand, BlogViewModel>
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


            public async Task<BlogViewModel> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Creating Blog: {Name}", request.CreateModel.Title);

                // Tạo mới Image
                var image = _mapper.Map<Image>(request.CreateModel.Image);
                image.Id = Guid.NewGuid();
                await _unitOfWork.ImageRepository.AddAsync(image);

                // Tạo mới blog
                var blog = _mapper.Map<Blog>(request.CreateModel);
                blog.Id = Guid.NewGuid();
                blog.ImageId = image.Id;

                await _unitOfWork.BlogRepository.AddAsync(blog);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Blog created successfully with ID: {BlogId}", blog.Id);


                var ViewModel = _mapper.Map<BlogViewModel>(blog);


                return ViewModel;
            }


        }
    }
}
