namespace EvacuationPlanning.Core.Common
{
    public class CalculationEta
    {
        public int ComputeEta(double distance, double speed)
        {
            return (int)Math.Round((distance / speed) * 60);
        }
    }
}