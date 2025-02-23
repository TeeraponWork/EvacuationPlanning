using EvacuationPlanning.Core.Dto.Vehicles;
using EvacuationPlanning.Core.Interfaces.IRepo.IEvacuationZones;
using EvacuationPlanning.Core.Model.Response;

namespace EvacuationPlanning.Core.Interfaces.IVehicles
{
    public interface IVehiclesService
    {
        Task<ResultResponseModel<object>> Add(VehicleRequestDto request);
    }
}