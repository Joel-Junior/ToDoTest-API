using TodosProject.Infra.Structure.IoC.MapEntitiesXDto;
using TodosProject.Application;
using TodosProject.Application.Interfaces;
using TodosProject.Application.Services;
using TodosProject.Infra.Data.Context;
using TodosProject.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TodosProject.Infra.Structure.IoC
{
    public static class DependencyContainer
    {
        public static IServiceCollection RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGeneralService, GeneralService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<ILicenseService, LicenseService>();
            services.AddScoped<IAccessGroupService, AccessGroupService>();           
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }

        public static IServiceCollection RegisterDbConnection(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TodosProjectContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }

        public static IServiceCollection RegisterMapperConfig(IServiceCollection services)
        {
            services.AddSingleton(MapperConfigurations.CreateMap().CreateMapper());
            return services;
        }
    }
}
