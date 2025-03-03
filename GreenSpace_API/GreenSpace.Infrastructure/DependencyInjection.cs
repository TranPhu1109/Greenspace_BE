using Microsoft.Extensions.DependencyInjection;
using GreenSpace.Application.Profiles;
using Microsoft.EntityFrameworkCore;

namespace GreenSpace.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string dbConnection)
    {
        services.AddAutoMapper(typeof(MapperConfigurationProfile));
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(dbConnection));
        return services;
    }
}
