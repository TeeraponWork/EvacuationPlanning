using EvacuationPlanning.Core.Dto.Vehicles;
using FluentValidation;

namespace EvacuationPlanning.Core.Validate
{
    public class VehiclesValidator : AbstractValidator<VehicleRequestDto>
    {
        public VehiclesValidator()
        {
            RuleFor(x => x.Latitude)
                .NotEqual(0).WithMessage("กรุณากรอกค่า: ตำแหน่ง Latitude");

            RuleFor(x => x.Longitude)
                .NotEqual(0).WithMessage("กรุณากรอกค่า: ตำแหน่ง Longitude");

            RuleFor(x => x.Type)
                .NotNull().WithMessage("กรุณากรอกค่า: ประเภทของยานพาหนะ");

            RuleFor(x => x.Capacity)
                .NotEqual(0).WithMessage("กรุณากรอกค่า: จำนวนคนที่ยานพาหนะสามารถขนส่งได้ต่อเที่ยว");

            RuleFor(x => x.Speed)
                .NotEqual(0).WithMessage("กรุณากรอกค่า: ความเร็วเฉลี่ยของยานพาหนะ (กม./ชม.)");
        }
    }
}