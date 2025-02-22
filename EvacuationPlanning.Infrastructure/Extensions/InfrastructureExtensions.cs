using EvacuationPlanning.Core.Interfaces.IRepo.IEvacuationZones;
using EvacuationPlanning.Core.Interfaces.IRepo.IPlan;
using EvacuationPlanning.Core.Interfaces.IRepo.IVehicles;
using EvacuationPlanning.Infrastructure.Repositories.EvacuationDataZones;
using EvacuationPlanning.Infrastructure.Repositories.Plan;
using EvacuationPlanning.Infrastructure.Repositories.Vehicles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EvacuationPlanning.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtensions
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services,
            IConfiguration config)
        {
            // Repository
            services.AddScoped<IEvacuationZonesRepository, EvacuationZonesRepository>();
            services.AddScoped<IVehiclesRepository, VehiclesRepository>();
            services.AddScoped<IVehiclesRepository, VehiclesRepository>();
            services.AddScoped<IPlanRepository, PlanRepository>();

            return services;
        }
    }

}
