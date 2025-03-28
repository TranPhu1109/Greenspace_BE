using Firebase.Auth;
using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Application.ViewModels.Users;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;


namespace GreenSpace.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppSettings _appSettings;
    private readonly IJWTTokenGenerator _jwtTokenGenerator;
    public AuthService(IUnitOfWork unitOfWork, AppSettings appSettings, IJWTTokenGenerator jWTTokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _appSettings = appSettings;
        _jwtTokenGenerator = jWTTokenGenerator;
    }
    public async Task<LoginResponseModel> LoginAsync(string token, string? FCMToken, string role)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(apiKey: _appSettings.FirebaseSettings.ApiKeY));

        var user = await auth.GetUserAsync(token)
            ?? throw new Exception($"Error at {nameof(AuthService)}: Firebase can not get this user");

        if (user is null)
            throw new Exception($"Error at: {nameof(IAuthService)}_ User not exist on firebase authentication");

        var userInDb = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == user.Email, x => x.Role);

        if (userInDb is not null)
        {
            string newToken = _jwtTokenGenerator.GenerateToken(user: userInDb, role: userInDb.Role.RoleName);
            userInDb.JWTToken = newToken;
            if (FCMToken is not null)
            {
                userInDb.FCMToken = FCMToken;
            }
            _unitOfWork.UserRepository.Update(userInDb);
            await _unitOfWork.SaveChangesAsync();
            // TODO: Gen token rồi trả Token system
            return new LoginResponseModel
            {
                Token = newToken,
                User = _unitOfWork.Mapper.Map<UserViewModel>(userInDb)
            };
        }
        else
        {
            var roleInDb = await _unitOfWork.RoleRepository.FirstOrDefaultAsync(x => x.RoleName.ToLower() == role.ToLower())
                        ?? throw new Exception($"Error: {nameof(AuthService)}_ Role Not found: rolename: {role}");

            // TODO: Insert Db
            GreenSpace.Domain.Entities.User newUser = new()
            {
                Name = string.Empty,
                Phone = string.Empty,
                FCMToken = FCMToken,
                Id = Guid.NewGuid(),
                Email = user.Email,
                RoleId = roleInDb.Id,
            };
            string jwtToken = _jwtTokenGenerator.GenerateToken(newUser, roleInDb.RoleName);
            newUser.JWTToken = jwtToken;

            await _unitOfWork.UserRepository.AddAsync(newUser);
            if (await _unitOfWork.SaveChangesAsync())
            {
                var wallet = new UsersWallet
                {
                    Name = $"User {user.Email}'s Wallet",
                    Amount = 0,
                    WalletType = nameof(WalletTypeEnum.Customer),
                    UserId = newUser.Id
                };
                await _unitOfWork.WalletRepository.AddAsync(wallet);
                await _unitOfWork.SaveChangesAsync();
                return new LoginResponseModel
                {
                    Token = jwtToken,
                    User = _unitOfWork.Mapper.Map<UserViewModel>(newUser)
                };
            }
            else throw new Exception($"Error: {nameof(AuthService)}_{nameof(LoginAsync)}: SaveChange new User Failed");


        }
    }

    public async Task<LoginResponseModel> LoginWithEmailPasswordAsync(string email, string password, string? FCMToken)
    {
        // Kiểm tra xem người dùng có tồn tại trong cơ sở dữ liệu không
        var userInDb = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == email, x => x.Role);

        if (userInDb is null)
            throw new Exception($"Error at: {nameof(IAuthService)}_ User with email {email} does not exist");

        // Kiểm tra mật khẩu
        
        bool isPasswordValid = await VerifyPasswordAsync(email, password);

        if (!isPasswordValid)
            throw new Exception($"Error at: {nameof(IAuthService)}_ Invalid password for user {email}");

        
        string newToken = _jwtTokenGenerator.GenerateToken(user: userInDb, role: userInDb.Role.RoleName);
        userInDb.JWTToken = newToken;

        
        if (FCMToken is not null)
        {
            userInDb.FCMToken = FCMToken;
        }

        
        _unitOfWork.UserRepository.Update(userInDb);
        await _unitOfWork.SaveChangesAsync();

        return new LoginResponseModel
        {
            Token = newToken,
            User = _unitOfWork.Mapper.Map<UserViewModel>(userInDb)
        };
    }

    private async Task<bool> VerifyPasswordAsync(string email, string password)
    {
        
        var user = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == email);

        if (user == null)
            return false;

        return BCrypt.Net.BCrypt.Verify(password, user.Password);

    }

    public async Task<LoginResponseModel> RefreshTokenAsync(string token)
    {
        var user = (await _unitOfWork.UserRepository.WhereAsync(x => x.JWTToken == token, x => x.Role)).FirstOrDefault() ?? throw new Exception("Not have any user with provided token");
        if (user is not null)
        {
            return new LoginResponseModel
            {
                Token = _jwtTokenGenerator.GenerateToken(user, user.Role.RoleName),
                User = _unitOfWork.Mapper.Map<UserViewModel>(user)
            };
        }
        else
        {
            return new();
        }
    }
}
