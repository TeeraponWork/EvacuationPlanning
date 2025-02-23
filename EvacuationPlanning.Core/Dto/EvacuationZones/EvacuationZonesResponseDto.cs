using EvacuationPlanning.Core.Model.Location;

namespace EvacuationPlanning.Core.Dto.EvacuationZones
{
    public class EvacuationZonesResponseDto
    {
        public string ZoneID { get; set; }
        public LocationModel LocationCoordinates { get; set; }
        public int NumberofPeople { get; set; }
        public int UrgencyLevel { get; set; }
    }
}
