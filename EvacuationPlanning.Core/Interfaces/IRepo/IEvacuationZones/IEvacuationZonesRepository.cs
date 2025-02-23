using EvacuationPlanning.Core.Entities.EvacuationZones;

namespace EvacuationPlanning.Core.Interfaces.IRepo.IEvacuationZones
{
    public interface IEvacuationZonesRepository
    {
        Task<bool> Add(EvacuationZonesEntities request);
        Task<bool> AddSet(string zoneID);
        Task<EvacuationZonesEntities> GetById(string zoneId);
        Task<List<EvacuationZonesEntities>> GetAll();



        Task<bool> DeleteAll();
    }
}
