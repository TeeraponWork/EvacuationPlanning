namespace EvacuationPlanning.Core.Model.DistanceCalculation
{
    public class CoordinateModel
    {
        public Guid ZoneID { get; set; }
        public Guid VehiclesId { get; set; }
        public double StartLatitude { get; set; }
        public double StartLongitude { get; set; }
        public double EndLatitude { get; set; }
        public double EndLongitude { get; set; }
    }

}
