namespace EvacuationPlanning.Core.Dto.Plan
{
    public class UpdatePlanDto
    {
        public string ZoneID { get; set; }
        public int EvacuatedPeople { get; set; }
        public string AssignedVehiclesId { get; set; } = null;
    }
}
