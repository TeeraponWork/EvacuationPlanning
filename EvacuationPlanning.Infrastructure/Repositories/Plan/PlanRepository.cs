using EvacuationPlanning.Core.Entities.Plan;
using EvacuationPlanning.Core.Interfaces.IRepo.IPlan;

namespace EvacuationPlanning.Infrastructure.Repositories.Plan
{
    public class PlanRepository : IPlanRepository
    {
        static List<PlanEntities> planEntities = new List<PlanEntities>();

        public async Task DeletePlan()
        {
            planEntities.Clear();
        }

        public async Task InsertPlan(List<PlanEntities> data)
        {
            foreach (var item in data)
            {
                planEntities.Add(item);
            }
        }

        public async Task UpdatePlan(PlanEntities data)
        {
            var plan = planEntities.FirstOrDefault(x => x.ZoneID == data.ZoneID);
            if (plan != null)
            {
                plan.EvacuatedPeople = data.EvacuatedPeople;
                plan.UpdateDate = DateTime.Now;
                plan.AssignedVehiclesId = data.AssignedVehiclesId;
                plan.RemainingPeople = data.RemainingPeople;
            }
        }

        public Task<List<PlanEntities>> ViewPlan()
        {
            return Task.FromResult(planEntities);
        }
    }
}
