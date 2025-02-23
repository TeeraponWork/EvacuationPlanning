namespace EvacuationPlanning.Core.Entities.Plan
{
    public class PlanEntities
    {
        public string ZoneID { get; set; }
        public string AssignedVehiclesId { get; set; }
        public string UsedVehiclesId { get; set; } = null;
        public int PeopleTotal { get; set; }
        public int EvacuatedPeople { get; set; }
        public int RemainingPeople { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
