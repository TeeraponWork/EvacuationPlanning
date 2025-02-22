namespace EvacuationPlanning.Core.Model.DistanceCalculation
{
    public class ResultDistanceModel
    {
        public Guid ZoneID { get; set; }
        public Guid VehiclesId { get; set; }
        public double Distance { get; set; }
    }
}
