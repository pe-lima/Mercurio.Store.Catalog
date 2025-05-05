using Catalog.Domain.Interfaces.Repositories;
using Catalog.Infrastructure.Data.Context;
using Catalog.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            // (MongoDb)
            services.AddSingleton<MongoDbContext>();

            // (Repositories)
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
