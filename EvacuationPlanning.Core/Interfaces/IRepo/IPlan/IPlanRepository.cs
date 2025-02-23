using EvacuationPlanning.Core.Entities.Plan;

namespace EvacuationPlanning.Core.Interfaces.IRepo.IPlan
{
    public interface IPlanRepository
    {
        Task<bool> AddOrUpdate(List<PlanEntities> request);
        Task<List<PlanEntities>?> GetPlan(string planId);
        Task<bool> DeletePlan(string planId);
    }
}
