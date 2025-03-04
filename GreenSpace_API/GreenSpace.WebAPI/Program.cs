using GreenSpace.Application.GlobalExceptionHandling;
using GreenSpace.Application.SignalR;
using GreenSpace.Infrastructure;
using GreenSpace.WebAPI;
using GreenSpace.WebAPI.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
await builder.AddWebAPIServicesAsync();


//var appSettings = builder.Configuration.GetSection("ConnectionStrings").Get<AppSettings>();
//builder.Services.AddSingleton(appSettings);

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//var assembly = Assembly.GetExecutingAssembly();
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
// Add Entity Framework Core (EF Core) for database access
//builder.Services.AddDbContext<AppDbContext>(options =>
    //options.UseSqlServer(connectionString));

// Register Dapper connection configuration (if using Dapper)
//builder.Services.AddScoped<IConnectionConfiguration, ConnectionConfiguration>();
//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//app.UseSwagger();
//app.UseSwaggerUI();


//app.UseHttpsRedirection();

//app.UseAuthorization();
////ApplyMigration();
//app.MapControllers();

//app.Run();

//void ApplyMigration()
//{
//    using (var scope = app!.Services.CreateScope())
//    {
//        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//        if (_db.Database.GetPendingMigrations().Count() > 0)
//        {
//            _db.Database.Migrate();
//        }
//    }
//}





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