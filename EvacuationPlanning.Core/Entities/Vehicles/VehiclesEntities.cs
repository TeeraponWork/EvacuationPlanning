namespace EvacuationPlanning.Core.Entities.Vehicles
{
    public class VehiclesEntities
    {
        public Guid VehiclesId { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }
    }
}