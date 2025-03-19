using AutoMapper;
using FluentValidation;
using GreenSpace.Application.Data;
using GreenSpace.Application.ViewModels.Roles;
using MediatR;

namespace GreenSpace.Application.Features.Role.Queries;

public class RoleQuery : IRequest<IEnumerable<RoleViewModel>>
{
    public Guid Id { get; set; } = Guid.Empty;
    public class QueryValidation : AbstractValidator<RoleQuery>
    {
        public QueryValidation()
        {

        }
    }

    public class QueryHandler : IRequestHandler<RoleQuery, IEnumerable<RoleViewModel>>
    {
        private readonly IConnectionConfiguration _connection;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public QueryHandler(IConnectionConfiguration connection, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _connection = connection;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<RoleViewModel>> Handle(RoleQuery request, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.RoleRepository.WhereAsync(x => x.Id == request.Id);

            if (role == null)
            {
                throw new Exception("No_Data_Found");
                
            }
            return _mapper.Map<IEnumerable<RoleViewModel>>(role);
        }
    }
}