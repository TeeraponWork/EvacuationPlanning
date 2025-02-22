using EvacuationPlanning.Core.Dto.Plan;
using EvacuationPlanning.Core.Entities.Plan;
using EvacuationPlanning.Core.Interfaces.IPlan;
using EvacuationPlanning.Core.Interfaces.IRepo.IEvacuationZones;
using EvacuationPlanning.Core.Interfaces.IRepo.IPlan;
using EvacuationPlanning.Core.Interfaces.IRepo.IVehicles;
using EvacuationPlanning.Core.Model.DistanceCalculation;
using EvacuationPlanning.Core.Model.Response;
using Microsoft.Extensions.Logging;

namespace EvacuationPlanning.Core.Services.Plan
{
    public class PlanServices : IPlanServices
    {
        private readonly IPlanCalculationServices _planCalculationServices;
        private readonly IUpdatePlanProcessService _updatePlanProcessService;
        private readonly IPlanRepository _planRepository;
        private readonly IEvacuationZonesRepository _evacuationZonesRepository;
        private readonly IVehiclesRepository _vehiclesRepository;
        private readonly ILogger<PlanServices> _logger;
        public PlanServices(
            IPlanCalculationServices planCalculationServices,
            IUpdatePlanProcessService updatePlanProcessService,
            IPlanRepository planRepository,
            IEvacuationZonesRepository evacuationZonesRepository,
            IVehiclesRepository vehiclesRepository,
            ILogger<PlanServices> logger)
        {
            _planCalculationServices = planCalculationServices;
            _updatePlanProcessService = updatePlanProcessService;
            _planRepository = planRepository;
            _evacuationZonesRepository = evacuationZonesRepository;
            _vehiclesRepository = vehiclesRepository;
            _logger = logger;
        }
        public async Task<ResultResponseModel<object>> EvacuationScheduler()
        {
            _logger.LogInformation("เริ่มต้นกระบวน สร้างแผนยานพาหนะกับพื้นที่อพยพ");
            try
            {
                _logger.LogInformation("เริ่มต้นกระบวน ดึงข้อมูลยานพาหนะกับพื้นที่อพยพ");
                var resultDistance = new List<ResultDistanceModel>();
                var dataVehicles = await _vehiclesRepository.GetAll();
                var dataEvacuationZones = await _evacuationZonesRepository.GetAll();

                _logger.LogInformation("เริ่มต้นกระบวน คำนวณระยะห่างระหว่างยานพาหนะกับพื้นที่อพยพ");
                foreach (var itemEvacuation in dataEvacuationZones)
                {
                    foreach (var itemVehicles in dataVehicles)
                    {
                        var data = new CoordinateModel()
                        {
                            VehiclesId = itemVehicles.VehiclesId,
                            ZoneID = itemEvacuation.ZoneID,
                            StartLatitude = itemEvacuation.Latitude,
                            StartLongitude = itemEvacuation.Longitude,
                            EndLatitude = itemVehicles.Latitude,
                            EndLongitude = itemVehicles.Longitude,
                        };
                        _logger.LogInformation($"เริ่มต้นกระบวน คำนวณระยะห่างระหว่างยานพาหนะ: {itemVehicles.VehiclesId} กับพื้นที่อพยพ: {itemEvacuation.ZoneID}");
                        var resultDistanceCalculation = _planCalculationServices.ProcessEvacuationDistances(data);
                        resultDistance.Add(resultDistanceCalculation);
                        _logger.LogInformation($"จบกระบวน คำนวณระยะห่างระหว่างยานพาหนะ: {itemVehicles.VehiclesId} กับพื้นที่อพยพ: {itemEvacuation.ZoneID}");
                    }
                }

                _logger.LogInformation($"เริ่มต้นกระบวน ProcessEvacuationPlan");
                var result = await _planCalculationServices.ProcessEvacuationPlan(resultDistance, dataEvacuationZones);
                if (result != null)
                {
                    _logger.LogInformation($"เริ่มต้นกระบวน นำเข้าข้อมูลแผนอพยพ");
                    var resultInsert = result.Select(item => new PlanEntities
                    {
                        ZoneID = item.ZoneID,
                        AssignedVehiclesId = string.Join(",", item.Vehicles.Select(v => v.VehicleID)),
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        EvacuatedPeople = 0,
                        PeopleTotal = item.Vehicles.Sum(v => v.NumberPeople),
                        RemainingPeople = item.Vehicles.Sum(v => v.NumberPeople),
                        UsedVehiclesId = null
                    }).ToList();
                    await _planRepository.InsertPlan(resultInsert);
                    _logger.LogInformation($"จบกระบวน นำเข้าข้อมูลแผนอพยพ");

                    return ResultResponseModel<object>.SuccessResponse(result);
                }
                return ResultResponseModel<object>.SuccessResponse("ไม่มีข้อมูลอพยพ");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"เกิดข้อผิดผลาด: {ex.Message}");
                return ResultResponseModel<object>.ExceptionResponse(ex.Message);
            }
        }
        public async Task<ResultResponseModel<object>> StatusEvacuation()
        {
            _logger.LogInformation("เริ่มต้นกระบวน ดูสถานะแผนข้อมูลอพยพ");
            try
            {
                var result = new List<StatusEvacuationDto>();

                _logger.LogInformation("เริ่มต้นกระบวน ค้นหาแผนข้อมูลอพยพในฐานข้อมูล");
                var data = await _planRepository.ViewPlan();
                foreach (var item in data)
                {
                    result.Add(new StatusEvacuationDto
                    {
                        LastVehicleUsed = item.UsedVehiclesId,
                        RemainingPeople = item.RemainingPeople,
                        TotalEvacuated = item.EvacuatedPeople,
                        ZoneID = item.ZoneID
                    });
                }
                _logger.LogInformation("จบกระบวน ดูสถานะแผนข้อมูลอพยพ");
                return ResultResponseModel<object>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"เกิดข้อผิดผลาด: {ex.Message}");
                return ResultResponseModel<object>.ExceptionResponse(ex.Message);
            }
        }

        public async Task<ResultResponseModel<object>> UpdateEvacuation(UpdatePlanDto model)
        {
            _logger.LogInformation("เริ่มต้นกระบวน อับเดจสถานะแผนข้อมูลอพยพ");
            try
            {
                var result = await _updatePlanProcessService.UpdatePlanProcess(model);
                if (result.IsSuccess == true)
                {
                    _logger.LogInformation("จบกระบวน อับเดจสถานะแผนข้อมูลอพยพ");
                    return ResultResponseModel<object>.SuccessResponse("ดำเนินการสำเร็จ");
                }
                else 
                {
                    _logger.LogInformation($"อับเดจสถานะแผนข้อมูลอพยพผิดผลาด: {result.ErrorMessage}");
                    return ResultResponseModel<object>.ErrorResponse("เกิดข้อผิดผลาด");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"เกิดข้อผิดผลาด: {ex.Message}");
                return ResultResponseModel<object>.ExceptionResponse(ex.Message);
            }
        }

        public async Task<ResultResponseModel<object>> DeleteEvacuation()
        {
            _logger.LogInformation("เริ่มต้นกระบวน ลบแผนข้อมูลอพยพ");
            try
            {
                _logger.LogInformation("เริ่มต้นกระบวนลบข้อมูล Plan,EvacuationZones,Vehicles");
                await _planRepository.DeletePlan();
                await _evacuationZonesRepository.DeleteAll();
                await _vehiclesRepository.DeleteAll();
                _logger.LogInformation("จบกระบวนลบข้อมูล Plan,EvacuationZones,Vehicles");
                return ResultResponseModel<object>.SuccessResponse("ดำเนินการสำเร็จ");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"เกิดข้อผิดผลาด: {ex.Message}");
                return ResultResponseModel<object>.ExceptionResponse(ex.Message);
            }
        }
    }
}