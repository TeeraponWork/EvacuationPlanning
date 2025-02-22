using EvacuationPlanning.Core.Model.DistanceCalculation;

namespace EvacuationPlanning.Core.Interfaces.IDistanceCalculation
{
    public interface IDistanceCalculationServices
    {
        double ConvertDegreesToRadians(double degrees);
        double CalculationDistance(CoordinateModel data);
    }
}
