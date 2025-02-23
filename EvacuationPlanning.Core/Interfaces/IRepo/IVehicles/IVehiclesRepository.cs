using EvacuationPlanning.Core.Entities.Vehicles;

namespace EvacuationPlanning.Core.Interfaces.IRepo.IVehicles
{
    public interface IVehiclesRepository
    {
        Task<bool> Add(VehiclesEntities request);
        Task<bool> AddSet(string vehicleId);
        Task<List<VehiclesEntities>> GetAll();
        Task<VehiclesEntities> GetById(string vehicleId);
        Task<bool> DeleteAll();
    }
}
