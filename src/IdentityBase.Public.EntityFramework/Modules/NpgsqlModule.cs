﻿using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IdentityBase.Public.EntityFramework
{
    public class NpgsqlModule : Autofac.Module
    {
        /// <summary>
        /// Loads dependencies 
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();
            var config = Current.Configuration;

            services.AddEntityFrameworkStores((options) =>
            {
                var migrationsAssembly = typeof(IServiceCollectionExtensions).GetTypeInfo().Assembly.GetName().Name;
                options.DbContextOptions = (dbBuilder) =>
                {
                    dbBuilder.UseNpgsql(config["EntityFramework:Npgsql:ConnectionString"], o => o.MigrationsAssembly(migrationsAssembly));
                };

                config.GetSection("EntityFramework").Bind(options);
            });

            builder.Populate(services);
        }
    }
}