namespace GreenSpace.Application.ViewModels.Users;

public class UserUpdateModel
{
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Password { get; set; }

}
