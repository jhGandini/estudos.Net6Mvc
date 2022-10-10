using Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.Configurations;

public static class IdentityConfig
{
    public static IServiceCollection AddIdentityContiguration(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<CookiePolicyOptions>(options => {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddDbContext<IDPDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("IDP")));

        services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<IDPDbContext>();

        return services;
    }
}

