namespace EvacuationPlanning.Core.Entities.EvacuationZones
{
    public class EvacuationZonesEntities
    {
        public string ZoneID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int NumberPeople { get; set; }
        public int Level { get; set; }
    }
}
