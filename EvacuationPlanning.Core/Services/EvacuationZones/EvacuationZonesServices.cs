using EvacuationPlanning.Core.Dto.EvacuationZones;
using EvacuationPlanning.Core.Entities.EvacuationZones;
using EvacuationPlanning.Core.Interfaces.IEvacuationZones;
using EvacuationPlanning.Core.Interfaces.IRepo.IEvacuationZones;
using EvacuationPlanning.Core.Model.Response;
using EvacuationPlanning.Core.Services.Plan;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EvacuationPlanning.Core.Services.EvacuationZones
{
    public class EvacuationZonesServices : IEvacuationZonesServices
    {
        private readonly IEvacuationZonesRepository _evacuationZonesRepository;
        private readonly IValidator<EvacuationZonesDto> _validator;
        private readonly ILogger<EvacuationZonesServices> _logger;
        public EvacuationZonesServices(IEvacuationZonesRepository evacuationZonesRepository,
            IValidator<EvacuationZonesDto> validator,
            ILogger<EvacuationZonesServices> logger
            )
        {
            _evacuationZonesRepository = evacuationZonesRepository;
            _validator = validator;
            _logger = logger;
        }

        public async Task<ResultResponseModel<object>> Add(EvacuationZonesDto request)
        {
            _logger.LogInformation("เริ่มต้นกระบวนการวางแผนการอพยพ");
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
                    Level = request.Level
                };
                var result = await _evacuationZonesRepository.Add(requestData);
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
                return ResultResponseModel<object>.ExceptionResponse($"ระบบทำงานผิดผลาด: { ex.Message}");
            }
        }
    }
}