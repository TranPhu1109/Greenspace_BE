﻿using FluentValidation;
using GreenSpace.Application.Features.User.Queries;
using GreenSpace.Application.ViewModels.Users;
using GreenSpace.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                var isDup = await _unitOfWork.UserRepository.WhereAsync(x => x.Email!.ToLower() == request.Model.Email!.ToLower() ||
                    x.Phone!.ToLower() == request.Model.Phone!.ToLower());
                if (isDup.Count() > 0)
                    throw new Exception($"Error: {nameof(CreateUserCommand)}_email or phone is duplicate!");
                //var createToFirebase = await CreateUserToFirebaseAsync(
                //    email: request.Model.Email ?? "",
                // password: request.Model.Password ?? "");
                request.Model.RoleName = request.Model.RoleName ?? nameof(RoleEnum.Customer);
                var role = await _unitOfWork.RoleRepository.FirstOrDefaultAsync(x => x.RoleName.ToLower() == request.Model.RoleName.ToLower())
                    ?? throw new Exception($"Error: {nameof(CreateUserCommand)}_no_role_found: role: {request.Model.RoleName}");
                //user.WalletId = await CreateWallet(user.Id);
                user.RoleId = role.Id;
                await _unitOfWork.UserRepository.AddAsync(user);
                if (await _unitOfWork.SaveChangesAsync())
                {
                    if (role.RoleName == nameof(RoleEnum.Customer))
                    {
                        //var wallet = new Domain.Entities.Wallet
                        //{
                        //    Amount = 0,
                        //    Name = $"Ví của {user.Email}",
                        //    WalletType = nameof(WalletTypeEnum.Customer),
                        //    //
                        //    = null,
                        //    UserId = user.Id
                        //};
                        //await _unitOfWork.WalletRepository.AddAsync(wallet);
                    }
                    return await _mediator.Send(new GetUserByIdQuery { Id = user.Id }, cancellationToken);
                }
                else
                {
                    throw new Exception($"Error: {nameof(CreateUserCommand)}_Save Change Failed!");
                }
            }
            //private async Task<bool> CreateUserToFirebaseAsync(string email, string password)
            //{
            //    var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey: _appSettings.FirebaseSettings.ApiKeY));
            //    try
            //    {
            //        var result = await auth.CreateUserWithEmailAndPasswordAsync(email: email, password: password);
            //        if (result.User is not null)
            //        {
            //            return true;
            //        }
            //        return false;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception($"{ex}");
            //    }

            //}

            //private async Task<Guid> CreateWallet(Guid userId)
            //{
            //    var wallet = new Domain.Entities.Wallet { Name = "User-Wallet", Amount = 0, WalletType = WalletTypeEnum.Customer.ToString(), UserId = userId };
            //    await _unitOfWork.WalletRepository.AddAsync(wallet);
            //    return wallet.Id;
            //}

        }
    }
}
