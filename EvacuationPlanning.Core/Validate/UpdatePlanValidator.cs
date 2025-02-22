using EvacuationPlanning.Core.Dto.Plan;
using FluentValidation;

namespace EvacuationPlanning.Core.Validate
{
    public class UpdatePlanValidator : AbstractValidator<UpdatePlanDto>
    {
        public UpdatePlanValidator()
        {
            RuleFor(x => x.ZoneID)
                .NotEmpty().WithMessage("กรุณากรอกค่า: ZoneID");

            RuleFor(x => x.EvacuatedPeople)
                .NotEqual(0).WithMessage("กรุณากรอกค่า: จำนวนประชาชน");
        }
    }
}