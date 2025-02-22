using EvacuationPlanning.Core.Dto.EvacuationZones;
using EvacuationPlanning.Core.Model.Response;

namespace EvacuationPlanning.Core.Interfaces.IEvacuationZones
{
    public interface IEvacuationZonesServices
    {
        Task<ResultResponseModel<object>> Add(EvacuationZonesDto request);
    }
}
