namespace EvacuationPlanning.Core.Dto.Vehicles
{
    public class VehicleRequestDto
    {
        public string VehicleId { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }
    }
}
