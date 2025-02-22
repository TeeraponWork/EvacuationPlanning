namespace EvacuationPlanning.Core.Dto.Plan
{
    public class UpdatePlanDto
    {
        public Guid ZoneID { get; set; }
        public int EvacuatedPeople { get; set; }
        public string AssignedVehiclesId { get; set; } = null;
    }
}
