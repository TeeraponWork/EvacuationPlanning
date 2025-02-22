namespace EvacuationPlanning.Core.Dto.Plan
{
    public class EvacuationPlanDto
    {
        public Guid ZoneID { get; set; }  
        public Guid VehicleID { get; set; } 
        public double Eta { get; set; } 
        public int NumberPeople { get; set; }
    }

    public class EvacuationZoneDto
    {
        public Guid ZoneID { get; set; } 
        public List<EvacuationVehicleDto> Vehicles { get; set; }
    }

    public class EvacuationVehicleDto
    {
        public Guid VehicleID { get; set; }
        public double Eta { get; set; } 
        public int NumberPeople { get; set; } 
    }
}
