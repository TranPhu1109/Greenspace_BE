namespace GreenSpace.Application;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = default!;
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; } = default!;
}