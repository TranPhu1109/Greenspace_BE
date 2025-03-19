using Firebase.Auth;
using FluentValidation;
using GreenSpace.Application.Features.User.Queries;
using GreenSpace.Application.Services;
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
                // Kiểm tra email đã tồn tại chưa
                var existingUser = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == request.Model.Email);
                if (existingUser != null)
                    throw new Exception("Email đã được sử dụng");

                // Lấy role từ database
                var roleInDb = await _unitOfWork.RoleRepository.FirstOrDefaultAsync(x => x.RoleName.ToLower() == request.Model.Role.ToLower())
                            ?? throw new Exception($"Error: {nameof(AuthService)}_ Role Not found: rolename: {request.Model.Role}");

                // Tạo user mới
                GreenSpace.Domain.Entities.User newUser = new()
                {
                    Name = request.Model.Name,
                    Phone = request.Model.Phone ?? string.Empty,
                    FCMToken = string.Empty,
                    Id = Guid.NewGuid(),
                    Email = request.Model.Email,
                    RoleId = roleInDb.Id,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Model.Password) // Mã hóa mật khẩu
                };

                // Tạo JWT token
                //string jwtToken = _jwtTokenGenerator.GenerateToken(newUser, roleInDb.RoleName);
                //newUser.JWTToken = jwtToken;

                // Lưu user vào database
                await _unitOfWork.UserRepository.AddAsync(newUser);
                if (await _unitOfWork.SaveChangesAsync())
                {


                    return await _mediator.Send(new GetUserByIdQuery { Id = newUser.Id }, cancellationToken);
                }
                else throw new Exception($"Error: {nameof(AuthService)}_{nameof(RegisterUserCommand)}: SaveChange new User Failed");
            }
        }
    }
}
