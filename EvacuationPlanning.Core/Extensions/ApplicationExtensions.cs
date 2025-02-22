using EvacuationPlanning.Core.Interfaces.IDistanceCalculation;
using EvacuationPlanning.Core.Interfaces.IEvacuationZones;
using EvacuationPlanning.Core.Interfaces.IPlan;
using EvacuationPlanning.Core.Interfaces.IVehicles;
using EvacuationPlanning.Core.Services.DistanceCalculationServices;
using EvacuationPlanning.Core.Services.EvacuationZones;
using EvacuationPlanning.Core.Services.Plan;
using EvacuationPlanning.Core.Services.Vehicles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using EvacuationPlanning.Core.Dto.EvacuationZones;
using EvacuationPlanning.Core.Dto.Plan;
using EvacuationPlanning.Core.Dto.Vehicles;

namespace EvacuationPlanning.Core.Extensions
{
    public static class ApplicationExportExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<EvacuationZonesDto>();
            services.AddValidatorsFromAssemblyContaining<UpdatePlanDto>();
            services.AddValidatorsFromAssemblyContaining<VehiclesDto>();


            // Services
            services.AddScoped<IEvacuationZonesServices, EvacuationZonesServices>();
            services.AddScoped<IVehiclesService, VehiclesService>();
            services.AddScoped<IDistanceCalculationServices, DistanceCalculationServices>();
            services.AddScoped<IPlanServices, PlanServices>();
            services.AddScoped<IDistanceCalculationServices, DistanceCalculationServices>();
            services.AddScoped<IPlanCalculationServices, PlanCalculationServices>();
            services.AddScoped<IUpdatePlanProcessService, UpdatePlanProcessService>();

            return services;
        }
    }

}
