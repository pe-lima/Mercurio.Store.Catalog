using Catalog.Application.DTOs.Product;
using Catalog.Application.Mappers.ProductMapper;
using Catalog.Application.Mappers;
using Catalog.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Catalog.Application.Common.Behaviors;
using FluentValidation;

namespace Catalog.Application.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // (Handlers)
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // (Behaviors)
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // (Validators)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // (Mappers)
            services.AddScoped<IMapper<Product, ProductDto>, ProductMapper>();


            return services;
        }
    }
}
