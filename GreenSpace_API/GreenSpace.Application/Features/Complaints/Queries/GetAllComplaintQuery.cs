using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Blogs;
using GreenSpace.Application.ViewModels.Complaints;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Complaints.Queries
{
    public class GetAllComplaintQuery : IRequest<PaginatedList<ComplaintViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetAllComplaintQuery, PaginatedList<ComplaintViewModel>>
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

            public async Task<PaginatedList<ComplaintViewModel>> Handle(GetAllComplaintQuery request, CancellationToken cancellationToken)
            {



                var complains = await _unitOfWork.ComplaintRepository.GetAllAsync(x => x.Image,x => x.User);
                if (complains.Count == 0) throw new NotFoundException("There are no complaint in DB!");
                var viewModels = _mapper.Map<List<ComplaintViewModel>>(complains);

                return PaginatedList<ComplaintViewModel>.Create(
                            source: viewModels.AsQueryable(),
                            pageIndex: request.PageNumber,
                            pageSize: request.PageSize
                    );
            }
        }
    }
}
