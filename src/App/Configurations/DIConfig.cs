using App.Extensions;
using Business.Interfaces;
using Business.Notifications;
using Business.Services;
using Data.Context;
using Data.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using System.Configuration;

namespace App.Configurations;

public static class DIConfig
{
    public static IServiceCollection ResolveDependencies(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IFornecedorRepository, FornecedorRepository>();
        services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();

        services.AddScoped<INotificador, Notificador>();
        services.AddScoped <IFornecedorService, FornecedorService>();
        services.AddScoped<IProdutoService, ProdutoService>();        

        return services;
    } 
}