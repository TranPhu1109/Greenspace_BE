using GreenSpace.Application.GlobalExceptionHandling;
using GreenSpace.Application.SignalR;
using GreenSpace.Infrastructure;
using GreenSpace.WebAPI;
using GreenSpace.WebAPI.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
await builder.AddWebAPIServicesAsync();
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 8080); // Lắng nghe trên tất cả các interface
});

var app = builder.Build();
app.UseCors();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Green Space API V1");
    c.RoutePrefix = string.Empty; // Đặt Swagger UI tại root (http://localhost:8080)
});

app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseMiddleware<PerformanceMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

ApplyMigration();
app.MapControllers();
app.MapHub<SignalrHub>("/hub");

app.Run();


void ApplyMigration()
{
    using (var scope = app!.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {

            _db.Database.Migrate();
        }
    }
}