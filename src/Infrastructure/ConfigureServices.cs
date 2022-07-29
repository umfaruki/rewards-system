using Application.Common.Interfaces;
using Infrastructure.Helper;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),                
                 b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)).ReplaceService<ISqlGenerationHelper, NpgsqlSqlGenerationLowercasingHelper>());


        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddTransient<IDapperMonthlyReportHandler, DapperMonthlyReportHandler>();
        

        return services;
    }
}
