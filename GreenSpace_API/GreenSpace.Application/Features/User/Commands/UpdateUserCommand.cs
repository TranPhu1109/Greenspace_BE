using FluentValidation;
using GreenSpace.Application.ViewModels.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.User.Commands;

public class UpdateUserCommand : IRequest<UserViewModel>
{
    public Guid Id { get; set; } = Guid.Empty;
    public UserUpdateModel Model { get; set; } = default!;
    public class CommandValidation : AbstractValidator<UpdateUserCommand>
    {
        public CommandValidation()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
            RuleFor(x => x.Model.Name).NotNull().NotEmpty()
                            .WithMessage("Name must not null or empty");

        }
    }
    public class CommandHandler : IRequestHandler<UpdateUserCommand, UserViewModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<UserViewModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.Id)
                   ?? throw new Exception($"Error: {nameof(UpdateUserCommand)}_no_user_found of Id: {request.Id}");

            if (!string.IsNullOrWhiteSpace(request.Model.Name))
                user.Name = request.Model.Name;

            if (!string.IsNullOrWhiteSpace(request.Model.Phone))
                user.Phone = request.Model.Phone;

            if (!string.IsNullOrWhiteSpace(request.Model.Address))
                user.Address = request.Model.Address;

            if (!string.IsNullOrWhiteSpace(request.Model.AvatarUrl))
                user.AvatarUrl = request.Model.AvatarUrl;

            if (!string.IsNullOrWhiteSpace(request.Model.Password))
                user.Password = request.Model.Password;

            
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return _unitOfWork.Mapper.Map<UserViewModel>(user);
        }
    }
}
