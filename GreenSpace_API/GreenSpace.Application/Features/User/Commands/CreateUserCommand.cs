using Firebase.Auth;
using FluentValidation;
using GreenSpace.Application.Features.User.Queries;
using GreenSpace.Application.ViewModels.Users;
using GreenSpace.Domain.Enum;
using MediatR;

namespace GreenSpace.Application.Features.User.Commands
{
    public class CreateUserCommand : IRequest<UserViewModel>
    {
        public UserCreateModel Model { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateUserCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.Model.Email).NotNull().NotEmpty()
                .EmailAddress()
                .WithMessage($"Email not valid");
                RuleFor(x => x.Model.Name).NotNull().NotEmpty().WithMessage($"Full Name is not valid");

            }
        }
        public class CommandHandler : IRequestHandler<CreateUserCommand, UserViewModel>
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
            public async Task<UserViewModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var user = _unitOfWork.Mapper.Map<Domain.Entities.User>(request.Model);

                var isDupEmail = await _unitOfWork.UserRepository.WhereAsync(x => x.Email!.ToLower() == request.Model.Email!.ToLower());
                if (isDupEmail.Count() > 0)
                    throw new Exception($"Error: {nameof(CreateUserCommand)}_email is duplicate!");
                var isDupPhone = await _unitOfWork.UserRepository.WhereAsync(x => x.Phone!.ToLower() == request.Model.Phone!.ToLower());
                if (isDupPhone.Count() > 0)
                    throw new Exception($"Error: {nameof(CreateUserCommand)}_phone is duplicate!");

                var createToFirebase = await CreateUserToFirebaseAsync(
                    email: request.Model.Email ?? "",
                 password: request.Model.Password ?? "");
                request.Model.RoleName = request.Model.RoleName ?? nameof(RoleEnum.Customer);

                var role = await _unitOfWork.RoleRepository.FirstOrDefaultAsync(x => x.RoleName.ToLower() == request.Model.RoleName.ToLower())
                    ?? throw new Exception($"Error: {nameof(CreateUserCommand)}_no_role_found: role: {request.Model.RoleName}");

                user.RoleId = role.Id;


                await _unitOfWork.UserRepository.AddAsync(user);
                if (await _unitOfWork.SaveChangesAsync())
                {
                    if (role.RoleName == nameof(RoleEnum.Customer).ToLower())
                    {
                        var wallet = new Domain.Entities.UsersWallet
                        {
                            Amount = 0,
                            Name = $"Ví của {user.Email}",
                            WalletType = nameof(WalletTypeEnum.Customer),
                            UserId = user.Id
                        };
                        await _unitOfWork.WalletRepository.AddAsync(wallet);
                        if (!await _unitOfWork.SaveChangesAsync())
                        {
                            throw new Exception($"Error: Failed to save wallet for customer!");
                        }
                    }
                    return await _mediator.Send(new GetUserByIdQuery { Id = user.Id }, cancellationToken);

            {
                throw new Exception($"Error: Failed to save wallet for customer!");
            }
                }
                else
                {
                    throw new Exception($"Error: {nameof(CreateUserCommand)}_Save Change Failed!");
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
