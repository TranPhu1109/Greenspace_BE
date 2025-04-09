using GreenSpace.Application.GlobalExceptionHandling;
using GreenSpace.Application.Services;
using GreenSpace.Application.SignalR;
using GreenSpace.Infrastructure;
using GreenSpace.WebAPI;
using GreenSpace.WebAPI.Middlewares;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
await builder.AddWebAPIServicesAsync();

var app = builder.Build();
app.UseCors();

QuestPDF.Settings.License = LicenseType.Community;

app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseMiddleware<PerformanceMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/hangfire");
RecurringJob.AddOrUpdate<GhnJobService>(
    "fetch-ghn-order",
    job => job.FetchGhnOrder(),
    Cron.MinuteInterval(1)
);
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