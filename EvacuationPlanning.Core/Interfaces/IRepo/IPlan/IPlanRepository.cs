using EvacuationPlanning.Core.Entities.Plan;

namespace EvacuationPlanning.Core.Interfaces.IRepo.IPlan
{
    public interface IPlanRepository
    {
        Task InsertPlan(List<PlanEntities> data);
        Task<List<PlanEntities>> ViewPlan();
        Task UpdatePlan(PlanEntities data);
        Task DeletePlan();

    }
}
