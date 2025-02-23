using EvacuationPlanning.Core.Model.Location;

namespace EvacuationPlanning.Core.Dto.Vehicles
{
    public class VehicleResponseDto
    {
        public string VehicleID { get; set; }
        public int Capacity { get; set; }
        public string Type { get; set; }
        public LocationModel LocationCoordinates { get; set; }
        public double Speed { get; set; }
    }
}
