using EvacuationPlanning.Core.Dto.Vehicles;
using EvacuationPlanning.Core.Entities.Vehicles;
using EvacuationPlanning.Core.Interfaces.IRepo.IVehicles;
using EvacuationPlanning.Core.Interfaces.IVehicles;
using EvacuationPlanning.Core.Model.Location;
using EvacuationPlanning.Core.Model.Response;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EvacuationPlanning.Core.Services.Vehicles
{
    public class VehiclesService : IVehiclesService
    {
        private readonly IValidator<VehicleRequestDto> _validator;
        private readonly IVehiclesRepository _vehiclesRepository;
        private readonly ILogger<VehiclesService> _logger;
        public VehiclesService(IValidator<VehicleRequestDto> validator,
            IVehiclesRepository vehiclesRepository,
            ILogger<VehiclesService> logger)
        {
            _validator = validator;
            _vehiclesRepository = vehiclesRepository;
            _logger = logger;
        }
        public async Task<ResultResponseModel<object>> Add(VehicleRequestDto request)
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
                    VehicleId = request.VehicleId
                };

                await _vehiclesRepository.Add(requestData);
                await _vehiclesRepository.AddSet(requestData.VehicleId.ToString());
                var data = await _vehiclesRepository.GetAll();

                var result = data.Select(x => new VehicleResponseDto
                {
                    VehicleID = x.VehicleId,
                    Capacity = x.Capacity,
                    Type = x.Type,
                    LocationCoordinates = new LocationModel { Latitude = x.Latitude, Longitude = x.Longitude },
                    Speed = x.Speed,
                }).ToList();
                _logger.LogInformation("จบกระบวนการนำเข้าข้อมูล");
                return ResultResponseModel<object>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ระบบทำงานผิดผลาด: {ex.Message}");
                return ResultResponseModel<object>.ExceptionResponse($"ระบบทำงานผิดผลาด: {ex.Message}");
            }
        }
    }
}
