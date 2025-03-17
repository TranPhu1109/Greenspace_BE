using AutoMapper;
using GreenSpace.Application.Data;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Categories.Queries
{
    public class GetAllCategoryQuery : IRequest<List<CategoryViewModel>>
    {
        public class QueryHandler : IRequestHandler<GetAllCategoryQuery, List<CategoryViewModel>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ILogger<QueryHandler> _logger;

            public QueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<QueryHandler> logger)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
            }
            public async Task<List<CategoryViewModel>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
            {
                var cates = await _unitOfWork.CategoryRepository.GetAllAsync();
                if (cates.Count == 0)throw new NotFoundException("There are no categories in the database!");
                return _mapper.Map<List<CategoryViewModel>>(cates);
            }
        }
    }

}
