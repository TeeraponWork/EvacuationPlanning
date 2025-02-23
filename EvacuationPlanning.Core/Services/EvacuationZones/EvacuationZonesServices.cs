using EvacuationPlanning.Core.Dto.EvacuationZones;
using EvacuationPlanning.Core.Entities.EvacuationZones;
using EvacuationPlanning.Core.Interfaces.IEvacuationZones;
using EvacuationPlanning.Core.Interfaces.IRepo.IEvacuationZones;
using EvacuationPlanning.Core.Model.Location;
using EvacuationPlanning.Core.Model.Response;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EvacuationPlanning.Core.Services.EvacuationZones
{
    public class EvacuationZonesServices : IEvacuationZonesServices
    {
        private readonly IEvacuationZonesRepository _evacuationZonesRepository;
        private readonly IValidator<EvacuationZonesRequestDto> _validator;
        private readonly ILogger<EvacuationZonesServices> _logger;
        public EvacuationZonesServices(IEvacuationZonesRepository evacuationZonesRepository,
            IValidator<EvacuationZonesRequestDto> validator,
            ILogger<EvacuationZonesServices> logger
            )
        {
            _evacuationZonesRepository = evacuationZonesRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<ResultResponseModel<object>> Add(EvacuationZonesRequestDto request)
        {
            _logger.LogInformation("เริ่มต้นกระบวนการเพิ่มแผนการอพยพ");
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
                var requestData = new EvacuationZonesEntities
                {
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    NumberPeople = request.NumberPeople,
                    Level = request.Level,
                    ZoneID = request.ZoneID,
                };

                await _evacuationZonesRepository.Add(requestData);
                await _evacuationZonesRepository.AddSet(requestData.ZoneID.ToString());
                _logger.LogInformation("จบกระบวนการนำเข้าข้อมูล");

                _logger.LogInformation("เริ่มต้นกระบวนดึงแผนการอพยพ");
                var data = await _evacuationZonesRepository.GetAll();
                var result = data.Select(x => new EvacuationZonesResponseDto
                {
                    ZoneID = x.ZoneID,
                    LocationCoordinates = new LocationModel { Latitude = x.Latitude, Longitude = x.Longitude },
                    NumberofPeople = x.NumberPeople,
                    UrgencyLevel = x.Level
                }).ToList();

                _logger.LogInformation("จบกระบวนดึงแผนการอพยพ");
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