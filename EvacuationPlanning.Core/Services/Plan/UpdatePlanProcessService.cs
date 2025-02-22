using EvacuationPlanning.Core.Dto.Plan;
using EvacuationPlanning.Core.Entities.Plan;
using EvacuationPlanning.Core.Interfaces.IPlan;
using EvacuationPlanning.Core.Interfaces.IRepo.IEvacuationZones;
using EvacuationPlanning.Core.Interfaces.IRepo.IPlan;
using EvacuationPlanning.Core.Interfaces.IRepo.IVehicles;
using EvacuationPlanning.Core.Model.Response;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EvacuationPlanning.Core.Services.Plan
{
    public class UpdatePlanProcessService : IUpdatePlanProcessService
    {
        private readonly IValidator<UpdatePlanDto> _validator;
        private readonly IPlanRepository _planRepository;
        private readonly IEvacuationZonesRepository _evacuationZonesRepository;
        private readonly IVehiclesRepository _vehiclesRepository;
        private readonly ILogger<UpdatePlanProcessService> _logger;

        public UpdatePlanProcessService(
            IValidator<UpdatePlanDto> validator,
            IPlanRepository planRepository,
            IEvacuationZonesRepository evacuationZonesRepository,
            IVehiclesRepository vehiclesRepository,
            ILogger<UpdatePlanProcessService> logger)
        {
            _validator = validator;
            _planRepository = planRepository;
            _evacuationZonesRepository = evacuationZonesRepository;
            _vehiclesRepository = vehiclesRepository;
            _logger = logger;
        }
        public async Task<ResultResponseModel<string>> UpdatePlanProcess(UpdatePlanDto model)
        {
            _logger.LogInformation("เริ่มต้นกระบวนการ ดึงข้อมูล Vehicles EvacuationZones Plan");
            var dataVehicles = await _vehiclesRepository.GetAll();
            var dataEvacuationZones = await _evacuationZonesRepository.GetAll();
            var dataPlan = await _planRepository.ViewPlan();
            _logger.LogInformation("จบกระบวนการ ดึงข้อมูล Vehicles EvacuationZones Plan");

            _logger.LogInformation("เริ่มต้นกระบวนการ ตรวจสอบข้อมูล Validation");
            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
            {
                var errorMessage = validationResult.Errors.First().ErrorMessage;
                return ResultResponseModel<string>.ErrorResponse(errorMessage);
            }
            _logger.LogInformation("จบกระบวนการ ตรวจสอบข้อมูล Validation");

            _logger.LogInformation("เริ่มต้นกระบวนการ UpdatePlanProcess");
            var vehicleIdList = model.AssignedVehiclesId.Split(',').ToList();
            var checkVehicles = dataVehicles.Where(x => vehicleIdList.Contains(x.VehiclesId.ToString())).ToList();
            if (checkVehicles == null) ResultResponseModel<string>.ErrorResponse("กรุณาระบุ ยานพาหนะที่ใช้");
            if (checkVehicles.Count != vehicleIdList.Count) ResultResponseModel<string>.ErrorResponse("กรุณาระบุ ยานพาหนะที่ใช้ให้ถูกต้อง");

            int peopleLeftInZone = dataPlan.Where(x => x.ZoneID == model.ZoneID).Select(x => x.PeopleTotal).FirstOrDefault() - model.EvacuatedPeople;
            var dataUpdate = new PlanEntities
            {
                EvacuatedPeople = model.EvacuatedPeople,
                AssignedVehiclesId = model.AssignedVehiclesId,
                RemainingPeople = dataPlan.Where(x => x.ZoneID == model.ZoneID).Select(x => x.PeopleTotal).FirstOrDefault() - model.EvacuatedPeople,
                ZoneID = model.ZoneID,
            };
            await _planRepository.UpdatePlan(dataUpdate);

            _logger.LogInformation("จบกระบวนการ UpdatePlanProcess");
            return ResultResponseModel<string>.ErrorResponse("อับเดจ ข้อมูล");
        }
    }
}
