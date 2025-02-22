using EvacuationPlanning.Core.Entities.Vehicles;

namespace EvacuationPlanning.Core.Interfaces.IRepo.IVehicles
{
    public interface IVehiclesRepository
    {
        Task<bool> Add(VehiclesEntities request);
        Task<List<VehiclesEntities>> GetAll();
        Task DeleteAll();
    }
}
