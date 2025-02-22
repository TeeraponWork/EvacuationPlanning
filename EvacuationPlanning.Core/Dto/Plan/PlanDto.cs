namespace EvacuationPlanning.Core.Dto.Plan
{
    public class PlanDto
    {
        public Guid ZoneID { get; set; }
        public Guid VehiclesId { get; set; }
        public DateTime Estimated { get; set; }
        public int NumberPeopleEvacuated { get; set; }
    }
}