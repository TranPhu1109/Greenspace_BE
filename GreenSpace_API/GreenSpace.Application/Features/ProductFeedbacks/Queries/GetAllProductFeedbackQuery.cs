﻿using AutoMapper;
using GreenSpace.Application.Features.Categories.Queries;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.Utilities;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.DesignIdea;
using GreenSpace.Application.ViewModels.ProductFeedback;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ProductFeedbacks.Queries
{
    public class GetAllProductFeedbackQuery : IRequest<PaginatedList<ProductFeedbackViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public class QueryHandler : IRequestHandler<GetAllProductFeedbackQuery, PaginatedList<ProductFeedbackViewModel>>
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
            public async Task<PaginatedList<ProductFeedbackViewModel>> Handle(GetAllProductFeedbackQuery request, CancellationToken cancellationToken)
            {
                var feedbacks = await _unitOfWork.ProductFeedbackRepository.GetAllAsync(p => p.Product, p => p.User);
                if (feedbacks.Count == 0) throw new NotFoundException("There are no Feedback in the database!");
                var viewModels = _mapper.Map<List<ProductFeedbackViewModel>>(feedbacks);

                return PaginatedList<ProductFeedbackViewModel>.Create(
                            source: viewModels.AsQueryable(),
                            pageIndex: request.PageNumber,
                            pageSize: request.PageSize
                    );

            }
        }
    }
}
