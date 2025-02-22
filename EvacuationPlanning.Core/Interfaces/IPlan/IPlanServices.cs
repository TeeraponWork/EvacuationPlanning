using EvacuationPlanning.Core.Dto.Plan;
using EvacuationPlanning.Core.Model.Response;

namespace EvacuationPlanning.Core.Interfaces.IPlan
{
    public interface IPlanServices
    {
        Task<ResultResponseModel<object>> EvacuationScheduler();
        Task<ResultResponseModel<object>> StatusEvacuation();
        Task<ResultResponseModel<object>> UpdateEvacuation(UpdatePlanDto model);
        Task<ResultResponseModel<object>> DeleteEvacuation();

    }
}