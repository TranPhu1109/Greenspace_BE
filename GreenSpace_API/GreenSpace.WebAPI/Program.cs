using GreenSpace.Application.GlobalExceptionHandling;
using GreenSpace.Application.SignalR;
using GreenSpace.Infrastructure;
using GreenSpace.WebAPI;
using GreenSpace.WebAPI.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
await builder.AddWebAPIServicesAsync();


var app = builder.Build();
app.UseCors();

//app.UseHangfireDashboard("/hangfire", new DashboardOptions { IgnoreAntiforgeryToken = true, Authorization = new[] { new DashboardAuthorizationFilter() } }, null);
//RecurringJob.AddOrUpdate<IBusRouteService>("check-routes", interService => interService.CheckNewCreatedRoute(), Cron.Monthly());
//RecurringJob.AddOrUpdate<IProductService>("Update-product-quantity", interService => interService.UpdateProduct(), Cron.Daily());
// Configure the HTTP request pipeline.
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