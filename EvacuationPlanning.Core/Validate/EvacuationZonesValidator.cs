using EvacuationPlanning.Core.Dto.EvacuationZones;
using FluentValidation;

namespace EvacuationPlanning.Core.Validate
{
    public class EvacuationZonesValidator : AbstractValidator<EvacuationZonesDto>
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
                .IsInEnum().WithMessage("กรุณากรอกค่า: ระดับความเร่งด่วนที่ถูกต้อง");
        }
    }
}
