namespace EvacuationPlanning.Core.Common
{
    public class CalculationEta
    {
        public double ComputeEta(double distance, double speed)
        {
            return (distance / speed) * 60;
        }
    }
}