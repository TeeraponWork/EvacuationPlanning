namespace EvacuationPlanning.Core.Dto.EvacuationZones
{
    public class EvacuationZonesRequestDto
    {
        public string ZoneID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int NumberPeople { get; set; }
        public int Level { get; set; }
    }
}
