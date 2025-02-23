using EvacuationPlanning.Core.Common.Enum;
using EvacuationPlanning.Core.Dto.EvacuationZones;
using FluentValidation;

namespace EvacuationPlanning.Core.Validate
{
    public class EvacuationZonesValidator : AbstractValidator<EvacuationZonesRequestDto>
    {
        public EvacuationZonesValidator()
        {
            RuleFor(x => x.Latitude)
                .NotEqual(0).WithMessage("กรุณากรอกค่า: ตำแหน่ง Latitude");

            RuleFor(x => x.Longitude)
                .NotEqual(0).WithMessage("กรุณากรอกค่า: ตำแหน่ง Longitude");

            RuleFor(x => x.NumberPeople)
                .GreaterThan(0).WithMessage("กรุณากรอกค่า: จำนวนคนที่ต้องอพยพ");

            RuleFor(x => x.Level)
                .InclusiveBetween((int)UrgencyLevel.Low, (int)UrgencyLevel.High)
                .WithMessage("กรุณากรอกค่าระดับความเร่งด่วนที่ถูกต้อง: 1 (Low) ถึง 5 (High)");
        }
    }
}
