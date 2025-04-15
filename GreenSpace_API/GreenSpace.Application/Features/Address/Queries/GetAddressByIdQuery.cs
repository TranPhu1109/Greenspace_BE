using AutoMapper;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Address;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GreenSpace.Application.Features.Address.Queries
{
    public class GetAddressByIdQuery : IRequest<AddressViewModel>
    {
        public Guid Id { get; set; } = Guid.Empty;
        public class QueryHandler : IRequestHandler<GetAddressByIdQuery, AddressViewModel>
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
            public async Task<AddressViewModel> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
            {
                var address = await _unitOfWork.AddressRepository.GetByIdAsync(request.Id);
                if (address == null)
                {
                    throw new NotFoundException($"Address with Id-{request.Id} is not exist!");
                }
                return _mapper.Map<AddressViewModel>(address);
            }
        }
    }
}
