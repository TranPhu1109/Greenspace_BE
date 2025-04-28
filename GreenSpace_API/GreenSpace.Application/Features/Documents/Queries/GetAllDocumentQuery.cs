using AutoMapper;
using GreenSpace.Application.Features.Documents.Queries;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Banner;
using GreenSpace.Application.ViewModels.Document;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Documents.Queries
{
    public class GetAllDocumentQuery : IRequest<List<DocumentViewModel>>
    {

        public class QueryHandler : IRequestHandler<GetAllDocumentQuery, List<DocumentViewModel>>
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

            public async Task<List<DocumentViewModel>> Handle(GetAllDocumentQuery request, CancellationToken cancellationToken)
            {
                var policy = await _unitOfWork.DocumentRepository.GetAllAsync();
                if (policy.Count == 0) throw new NotFoundException("There are no Policy in the database!");

                var viewModels = _mapper.Map<List<DocumentViewModel>>(policy);

                return viewModels;
            }
        }
    }
}

