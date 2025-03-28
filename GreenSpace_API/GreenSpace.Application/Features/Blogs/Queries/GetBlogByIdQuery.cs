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

namespace GreenSpace.Application.Features.Blogs.Queries
{
    public class GetBlogByIdQuery : IRequest<BlogViewModel>
    {
        public Guid Id { get; set; } = default!;

        public class QueryValidation : AbstractValidator<GetBlogByIdQuery>
        {
            public QueryValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            }
        }

        public class QueryHandler : IRequestHandler<GetBlogByIdQuery, BlogViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<QueryHandler> _logger;

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<QueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
            }
            public async Task<BlogViewModel> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
            {
                var blog = await _unitOfWork.BlogRepository.GetByIdAsync(request.Id, x => x.Image);
                if (blog is null) throw new NotFoundException($"Blog with ID-{request.Id} is not exist!");
                var result = _mapper.Map<BlogViewModel>(blog);
                return result;
            }
        }
    }
}
