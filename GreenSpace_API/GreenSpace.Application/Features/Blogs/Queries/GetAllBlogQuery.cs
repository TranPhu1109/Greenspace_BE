using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
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
    public class GetAllBlogQuery : IRequest<PaginatedList<BlogViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetAllBlogQuery, PaginatedList<BlogViewModel>>
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<QueryHandler> logger;

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<QueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                this.logger = logger;
            }

            public async Task<PaginatedList<BlogViewModel>> Handle(GetAllBlogQuery request, CancellationToken cancellationToken)
            {



                var blogs = await _unitOfWork.BlogRepository.GetAllAsync(x => x.Image);
                if (blogs.Count == 0) throw new NotFoundException("There are no blog in DB!");
                var viewModels = _mapper.Map<List<BlogViewModel>>(blogs);

                return PaginatedList<BlogViewModel>.Create(
                            source: viewModels.AsQueryable(),
                            pageIndex: request.PageNumber,
                            pageSize: request.PageSize
                    );
            }
        }
    }
}
