using App.Configurations;
using Data.Context;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables().Build();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Application")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityContiguration(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.ResolveDependencies();

builder.Services.AddTransient<IEmailSender, EmailSender>(i =>
                new EmailSender(
                    builder.Configuration["Email:Remetente"],
                    builder.Configuration["Email:Nome"],
                    builder.Configuration["Email:Smtp"],
                    builder.Configuration["Email:smtpPorta"],
                    builder.Configuration["Email:HeaderSender"]
                ));




builder.Services.AddMvcConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
    app.UseDatabaseErrorPage();
}
else
{
    app.UseExceptionHandler("/error/500");
    app.UseStatusCodePagesWithRedirects("/erro/{0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseGlobalizationConfig();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
