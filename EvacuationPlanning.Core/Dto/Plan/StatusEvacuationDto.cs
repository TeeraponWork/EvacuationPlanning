namespace EvacuationPlanning.Core.Dto.Plan
{
    public class StatusEvacuationDto
    {
        public Guid ZoneID { get; set; }
        public int TotalEvacuated { get; set; }
        public int RemainingPeople { get; set; }
        public string LastVehicleUsed { get; set; }
    }
}