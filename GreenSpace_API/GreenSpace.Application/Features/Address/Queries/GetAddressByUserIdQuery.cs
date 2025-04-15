using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Address;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GreenSpace.Application.Features.Address.Queries
{
    public class GetAddressByUserIdQuery : IRequest<List<AddressViewModel>>
    {
        public Guid UserId { get; set; } = Guid.Empty;
        public class QueryHandler : IRequestHandler<GetAddressByUserIdQuery, List<AddressViewModel>>
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
            public async Task<List<AddressViewModel>> Handle(GetAddressByUserIdQuery request, CancellationToken cancellationToken)
            {
                var address = await _unitOfWork.AddressRepository.WhereAsync(x => x.UserId == request.UserId);
                if (address == null || !address.Any())
                {
                    throw new NotFoundException($"There are no Address in DB.");
                }
                var viewModels = _mapper.Map<List<AddressViewModel>>(address);
                return viewModels;
            }
        }
    }
}
