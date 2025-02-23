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
            var dataPlan = await _planRepository.GetPlan("evacuationPlan");
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
            var checkVehicles = dataVehicles.Where(x => vehicleIdList.Contains(x.VehicleId.ToString())).ToList();
            if (checkVehicles == null) return ResultResponseModel<string>.ErrorResponse("กรุณาระบุ ยานพาหนะที่ใช้");
            if (checkVehicles.Count != vehicleIdList.Count) ResultResponseModel<string>.ErrorResponse("กรุณาระบุ ยานพาหนะที่ใช้ให้ถูกต้อง");

            if (dataPlan == null) return ResultResponseModel<string>.ErrorResponse("ไม่พบแผนที่อพยพ");
            var dataPlanUpdate = dataPlan.Where(x => x.ZoneID == model.ZoneID).ToList();
            if(dataPlanUpdate.Count == 0) return ResultResponseModel<string>.ErrorResponse("ไม่พบรหัส Plan Id");

            int peopleLeftInZone = dataPlanUpdate.Select(x => x.PeopleTotal).FirstOrDefault() - model.EvacuatedPeople;
            if (peopleLeftInZone < 0) return ResultResponseModel<string>.ErrorResponse("จำนวนคนอพยพที่เหลืออยู่ไม่สามารถติดลบได้");

            var dataUpdate = dataPlan.Where(x => x.ZoneID == model.ZoneID).ToList();
            dataUpdate.ForEach(x =>
            {
                x.EvacuatedPeople = model.EvacuatedPeople;
                x.UsedVehiclesId = model.AssignedVehiclesId;
                x.RemainingPeople = peopleLeftInZone;
                x.UpdateDate = DateTime.Now;
            });
            await _planRepository.AddOrUpdate(dataPlan);

            _logger.LogInformation("จบกระบวนการ UpdatePlanProcess");
            return ResultResponseModel<string>.SuccessResponse("อับเดจข้อมูล สำเร็จ");
        }
    }
}
