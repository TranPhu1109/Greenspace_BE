using Firebase.Auth;
using FluentValidation;
using GreenSpace.Application.Features.User.Queries;
using GreenSpace.Application.Services;
using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Application.ViewModels.Users;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;
using MediatR;
using System.Data;

namespace GreenSpace.Application.Features.User.Commands;

public class RegisterUserCommand : IRequest<UserViewModel>
{
    public RegisterRequestModel Model { get; set; } = default!;
    public class CommandValidation : AbstractValidator<RegisterUserCommand>
    {
        public CommandValidation()
        {
            RuleFor(x => x.Model.Email).NotNull().NotEmpty()
            .EmailAddress()
            .WithMessage($"Email not valid");
            RuleFor(x => x.Model.Name).NotNull().NotEmpty().WithMessage($"Full Name is not valid");

        }
        public class CommandHandler : IRequestHandler<RegisterUserCommand, UserViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMediator _mediator;
            private readonly AppSettings _appSettings;
            public CommandHandler(IUnitOfWork unitOfWork, IMediator mediator, AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _appSettings = appSettings;
                _mediator = mediator;
            }
            public async Task<UserViewModel> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                var user = _unitOfWork.Mapper.Map<Domain.Entities.User>(request.Model);

                var isDupEmail = await _unitOfWork.UserRepository.WhereAsync(x => x.Email!.ToLower() == request.Model.Email!.ToLower());
                if (isDupEmail.Count() > 0)
                    throw new Exception($"Error: {nameof(RegisterUserCommand)}_email is duplicate!");

                if (!string.IsNullOrEmpty(request.Model.Phone))
                {
                    var isDupPhone = await _unitOfWork.UserRepository.WhereAsync(x => x.Phone!.ToLower() == request.Model.Phone!.ToLower());
                    if (isDupPhone.Count() > 0)
                        throw new Exception($"Error: {nameof(RegisterUserCommand)}_phone is duplicate!");
                }

                var createToFirebase = await CreateUserToFirebaseAsync(
                    email: request.Model.Email ?? "",
                    password: request.Model.Password ?? "");

                // Mặc định role là Customer
                var roleName = nameof(RoleEnum.Customer);

                var role = await _unitOfWork.RoleRepository.FirstOrDefaultAsync(x => x.RoleName.ToLower() == roleName.ToLower())
                    ?? throw new Exception($"Error: {nameof(RegisterUserCommand)}_no_role_found: role: {roleName}");

                user.RoleId = role.Id;

                // Tạo ví cho khách hàng
                var wallet = new Domain.Entities.UsersWallet
                {
                    Amount = 0,
                    Name = $"Ví của {user.Email}",
                    WalletType = nameof(WalletTypeEnum.Customer),
                    UserId = user.Id
                };
                await _unitOfWork.WalletRepository.AddAsync(wallet);
            
                user.WalletId = wallet.Id;
                await _unitOfWork.UserRepository.AddAsync(user);

                if (await _unitOfWork.SaveChangesAsync())
                {
                    return await _mediator.Send(new GetUserByIdQuery { Id = user.Id }, cancellationToken);
                }
                else
                {
                    throw new Exception($"Error: {nameof(RegisterUserCommand)}_Save Change Failed!");
                }
            }

            private async Task<bool> CreateUserToFirebaseAsync(string email, string password)
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey: _appSettings.FirebaseSettings.ApiKeY));
                try
                {
                    var result = await auth.CreateUserWithEmailAndPasswordAsync(email: email, password: password);
                    if (result.User is not null)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    throw new Exception($"{ex}");
                }
            }
        }
    }
}

