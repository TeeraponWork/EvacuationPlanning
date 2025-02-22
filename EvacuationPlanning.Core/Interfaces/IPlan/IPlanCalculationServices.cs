using EvacuationPlanning.Core.Dto.Plan;
using EvacuationPlanning.Core.Entities.EvacuationZones;
using EvacuationPlanning.Core.Model.DistanceCalculation;

namespace EvacuationPlanning.Core.Interfaces.IPlan
{
    public interface IPlanCalculationServices
    {
        ResultDistanceModel ProcessEvacuationDistances(CoordinateModel model);
        Task<List<EvacuationZoneDto>> ProcessEvacuationPlan(List<ResultDistanceModel> dataDistance, List<EvacuationZonesEntities> dataEvacuationZones);
    }
}
