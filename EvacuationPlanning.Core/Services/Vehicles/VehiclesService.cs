using EvacuationPlanning.Core.Dto.Vehicles;
using EvacuationPlanning.Core.Entities.Vehicles;
using EvacuationPlanning.Core.Interfaces.IRepo.IVehicles;
using EvacuationPlanning.Core.Interfaces.IVehicles;
using EvacuationPlanning.Core.Model.Response;
using EvacuationPlanning.Core.Services.Plan;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EvacuationPlanning.Core.Services.Vehicles
{
    public class VehiclesService : IVehiclesService
    {
        private readonly IValidator<VehiclesDto> _validator;
        private readonly IVehiclesRepository _vehiclesRepository;
        private readonly ILogger<VehiclesService> _logger;
        public VehiclesService(IValidator<VehiclesDto> validator, 
            IVehiclesRepository vehiclesRepository,
            ILogger<VehiclesService> logger)
        {
            _validator = validator;
            _vehiclesRepository = vehiclesRepository;
            _logger = logger;
        }
        public async Task<ResultResponseModel<object>> Add(VehiclesDto request)
        {
            _logger.LogInformation("เริ่มต้นกระบวนการเพิ่มยานพาหนะ");
            try
            {
                _logger.LogInformation("เริ่มกระบวนการตรวจสอบข้อมูล");
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    var errorMessage = validationResult.Errors.First().ErrorMessage;
                    return ResultResponseModel<object>.ErrorResponse(errorMessage);
                }

                _logger.LogInformation("เริ่มกระบวนการนำเข้าข้อมูล");
                var requestData = new VehiclesEntities
                {
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    Type = request.Type,
                    Capacity = request.Capacity,
                    Speed = request.Speed,
                };
                var result = await _vehiclesRepository.Add(requestData);

                _logger.LogInformation("จบกระบวนการนำเข้าข้อมูล");
                if (result)
                {
                    return ResultResponseModel<object>.SuccessResponse("บันทึกข้อมูลเรียบร้อย");
                }
                else
                {
                    return ResultResponseModel<object>.ErrorResponse("บันทึกข้อมูลไม่ได้: กรุณาลองอีกครั้ง");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ระบบทำงานผิดผลาด: {ex.Message}");
                return ResultResponseModel<object>.ExceptionResponse($"ระบบทำงานผิดผลาด: {ex.Message}");
            }
        }
    }
}
