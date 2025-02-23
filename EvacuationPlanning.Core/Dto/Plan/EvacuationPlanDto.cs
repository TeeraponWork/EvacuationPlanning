namespace EvacuationPlanning.Core.Dto.Plan
{
    public class EvacuationPlanDto
    {
        public string ZoneID { get; set; }  
        public string VehicleID { get; set; } 
        public int Eta { get; set; } 
        public int NumberPeople { get; set; }
    }

    public class EvacuationZoneDto
    {
        public string ZoneID { get; set; } 
        public List<EvacuationVehicleDto> Vehicles { get; set; }
    }

    public class EvacuationVehicleDto
    {
        public string VehicleID { get; set; }
        public string Eta { get; set; } 
        public int NumberPeople { get; set; } 
    }
}
