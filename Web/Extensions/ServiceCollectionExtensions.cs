using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Repositories;

namespace Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Đăng ký tất cả services từ Application layer một cách tự động
            services.Scan(scan => scan
                .FromAssemblyOf<ISanPhamService>()
                .AddClasses(classes => classes.Where(type => 
                    type.IsClass && 
                    !type.IsAbstract && 
                    type.GetInterfaces().Any(i => i.Namespace == "Application.Interfaces")))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // Đăng ký Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        /// <summary>
        /// Đăng ký tất cả services từ một assembly cụ thể
        /// </summary>
        public static IServiceCollection AddServicesFromAssembly<T>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<T>()
                .AddClasses(classes => classes.Where(type => 
                    type.IsClass && 
                    !type.IsAbstract && 
                    type.GetInterfaces().Any()))
                .AsImplementedInterfaces()
                .WithLifetime(lifetime));

            return services;
        }

        /// <summary>
        /// Đăng ký tất cả services từ namespace cụ thể
        /// </summary>
        public static IServiceCollection AddServicesFromNamespace<T>(this IServiceCollection services, string namespaceFilter, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<T>()
                .AddClasses(classes => classes.Where(type => 
                    type.IsClass && 
                    !type.IsAbstract && 
                    type.Namespace != null &&
                    type.Namespace.Contains(namespaceFilter) &&
                    type.GetInterfaces().Any()))
                .AsImplementedInterfaces()
                .WithLifetime(lifetime));

            return services;
        }
    }
} 