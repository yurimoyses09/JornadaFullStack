using Fina.Core;
using Fina.Core.Handlers;
using Final.Api.Data;
using Final.Api.Handlers;
using Microsoft.EntityFrameworkCore;

namespace Final.Api.Common.Api;

public static class BuildExtension
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        ApiConfiguration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

        Configuration.BackEndUrl = builder.Configuration.GetValue<string>("BackEndUrl") ?? string.Empty;
        Configuration.FrontEndUrl = builder.Configuration.GetValue<string>("FrontEndUrl") ?? string.Empty;
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x => 
        {
            x.CustomSchemaIds(n => n.FullName);
        });
    }

    public static void AddDataContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(x => 
        {
            x.UseSqlServer(ApiConfiguration.ConnectionString);
        });
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(
            options => options.AddPolicy(
                ApiConfiguration.CorsPolicyName,
                policy => policy
                .WithOrigins([
                Configuration.BackEndUrl,
                Configuration.FrontEndUrl
                ])
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()));
    }

    public static void AddSercives(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
        builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
    }
}
