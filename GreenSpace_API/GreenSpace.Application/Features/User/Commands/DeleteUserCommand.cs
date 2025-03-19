using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.User.Commands;

public class DeleteUserCommand : IRequest
{
    public Guid Id { get; set; }
    public class CommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id)
                            ?? throw new Exception($"Error: {nameof(DeleteUserCommand)}_Not exist user with Id: {request.Id}");
            _unitOfWork.UserRepository.SoftRemove(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}