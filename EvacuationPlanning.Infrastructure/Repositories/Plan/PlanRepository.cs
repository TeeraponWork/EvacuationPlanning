using EvacuationPlanning.Core.Entities.Plan;
using EvacuationPlanning.Core.Interfaces.IRedis;
using EvacuationPlanning.Core.Interfaces.IRepo.IPlan;

namespace EvacuationPlanning.Infrastructure.Repositories.Plan
{
    public class PlanRepository : IPlanRepository
    {
        private readonly IRedisService _redisService;
        public PlanRepository(IRedisService redisService)
        {
            _redisService = redisService;
        }
        public async Task<bool> AddOrUpdate(List<PlanEntities> request)
        {
            return await _redisService.SetJsonAsync("evacuationPlan", request, null);
        }
        public async Task<List<PlanEntities>?> GetPlan(string planId)
        {
            var plan = await _redisService.GetJsonAsync<List<PlanEntities>>(planId);
            return plan;
        }
        public async Task<bool> DeletePlan(string planId)
        {
            var plan = await _redisService.DeleteAsync(planId);
            return plan;
        }
    }
}