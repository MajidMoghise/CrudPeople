using CrudPeople.CoreDomain.Contracts.Base.Commands;
using CrudPeople.CoreDomain.Contracts.People.Command;
using CrudPeople.CoreDomain.Contracts.People.Query;
using CrudPeople.Infrastructure.Configs.ConfigClassObject;
using CrudPeople.Infrastructure.EfCore.Repositories.Base.Commands.Base;
using CrudPeople.Infrastructure.EfCore.Repositories.People;
using CrudPeople.Infrastructure.Mongo.Repositories.Base;
using CrudPeople.Infrastructure.Mongo.Repositories.People;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using People.Infrastructure.Ef.Repositories.People.Commands;
using System.Diagnostics;

namespace CrudPeople.Infrastructure.Configs
{
    internal static class RepositoryRegistration
    {
        public delegate IUnitOfWork UnitOfWorkResolve(WorkingDatabase workingDb);
        internal static IServiceCollection AddRepository(this IServiceCollection services,IConfiguration config)
        {
            return services.AddSingeltonRepository()
                           .AddTranseintRepository()
                           .AddScoppedRepository(config);
        }
        private static IServiceCollection AddTranseintRepository(this IServiceCollection services)
        {
            return services;
        }
        private static IServiceCollection AddScoppedRepository(this IServiceCollection services,IConfiguration config)
        {
            var workingDb = config.GetValue<WorkingDatabase>("DataBaseWorking");
            if(workingDb is null)
            {
                throw new Exception("Config file is not correct in DataBaseWorking section");
            }
            services.AddScoped<EFUnitOfWork>();
            services.AddScoped<MongoUnitOfWork>();
            services.AddScoped<UnitOfWorkResolve>(serviceProvider => workingDb => {
                if(workingDb.Sql)
                {
                    return serviceProvider.GetService<EFUnitOfWork>();
                }
                else if(workingDb.MongoDb)
                {
                    return serviceProvider.GetService<MongoUnitOfWork>();
                }
                throw new Exception("Config file is not correct in DataBaseWorking section");

            });
            if (workingDb.Sql)
            {
                return services.AddScoped<IPeopleQueryRepository, PeopleQueryRepository>()
                               .AddScoped<IPeopleCommandRepository, PeopleCommandRepository>();
            }
            if(workingDb.MongoDb)
            {
                return services.AddScoped<IPeopleQueryRepository, PeopleMongoRepository>()
                           .AddScoped<IPeopleCommandRepository, PeopleMongoRepository>();

            }
            throw new Exception("Config file is not correct in DataBaseWorking section");

        }
        private static IServiceCollection AddSingeltonRepository(this IServiceCollection services)
        {
            return services;
        }

    }
}
