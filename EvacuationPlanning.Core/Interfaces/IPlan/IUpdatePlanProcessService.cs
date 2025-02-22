using EvacuationPlanning.Core.Dto.Plan;
using EvacuationPlanning.Core.Model.Response;

namespace EvacuationPlanning.Core.Interfaces.IPlan
{
    public interface IUpdatePlanProcessService
    {
        Task<ResultResponseModel<string>> UpdatePlanProcess(UpdatePlanDto model);
    }
}
