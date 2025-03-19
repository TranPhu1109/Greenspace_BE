using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.DesignIdeasCategory;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.DesignIdeasCategories.Queries
{
    public class GetAllDesignCategoryQuery : IRequest<List<DesignIdeasCategoryViewModel>>
    {
        public class QueryHandler : IRequestHandler<GetAllDesignCategoryQuery, List<DesignIdeasCategoryViewModel>>
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
            public async Task<List<DesignIdeasCategoryViewModel>> Handle(GetAllDesignCategoryQuery request, CancellationToken cancellationToken)
            {
                var cates = await _unitOfWork.DesignIdeasCategoryRepository.GetAllAsync();
                if (cates.Count == 0) throw new NotFoundException("There are no categories in the database!");
                return _mapper.Map<List<DesignIdeasCategoryViewModel>>(cates);
            }
        }
    }
}
