namespace GreenSpace.Application;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = default!;
    public JWTOptions JWTOptions { get; set; } = default!;
    public FirebaseSettings FirebaseSettings { get; set; } = default!;
    public EmailConfig Email { get; set; } = new();
    public VnPay VnPay { get; set; } = new();
}

public class JWTOptions
{
    public string Secret { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
}
public class ConnectionStrings
{
    public string DefaultConnection { get; set; } = default!;
}
public class FirebaseSettings
{
    public string SenderId { get; set; } = default!;
    public string ServerKey { get; set; } = default!;
    public string ApiKeY { get; set; } = default!;
    public string Bucket { get; set; } = default!;
    public string AuthEmail { get; set; } = default!;
    public string AuthPassword { get; set; } = default!;
}
public class VnPay
{
    public string Vnp_TmnCode { get; set; } = string.Empty;
    public string Vnp_HashSecret { get; set; } = string.Empty;
}

public class EmailConfig
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}
public class CloudinarySettings
{
    public string CloudName { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string ApiSecret { get; set; } = string.Empty;
}
public class GhnSettings
{
    public string ShopId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
