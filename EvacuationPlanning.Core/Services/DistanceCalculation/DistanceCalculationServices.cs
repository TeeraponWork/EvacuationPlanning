using EvacuationPlanning.Core.Interfaces.IDistanceCalculation;
using EvacuationPlanning.Core.Model.DistanceCalculation;
using Microsoft.Extensions.Logging;

namespace EvacuationPlanning.Core.Services.DistanceCalculationServices
{
    public class DistanceCalculationServices : IDistanceCalculationServices
    {
        private readonly ILogger<DistanceCalculationServices> _logger;
        public DistanceCalculationServices(ILogger<DistanceCalculationServices> logger)
        {
            _logger = logger;
        }
        //ref:https://www.geeksforgeeks.org/haversine-formula-to-find-distance-between-two-points-on-a-sphere/
        public double CalculationDistance(CoordinateModel data)
        {
            _logger.LogInformation("เริ่มต้นกระบวนแปลง Longitude,Latitude InRadians");
            double startLongitudeInRadians = ConvertDegreesToRadians(data.StartLongitude);
            double startLatitudeInRadians = ConvertDegreesToRadians(data.StartLatitude);
            double endLongitudeInRadians = ConvertDegreesToRadians(data.EndLongitude);
            double endLatitudeInRadians = ConvertDegreesToRadians(data.EndLatitude);
            _logger.LogInformation("จบกระบวนแปลง Longitude,Latitude InRadians");

            _logger.LogInformation("เริ่มต้นกระบวนการ Haversine formula");
            double dlon = endLongitudeInRadians - startLongitudeInRadians;
            double dlat = endLatitudeInRadians - startLatitudeInRadians;

            double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                       Math.Cos(startLatitudeInRadians) * Math.Cos(endLatitudeInRadians) *
                       Math.Pow(Math.Sin(dlon / 2), 2);

            double c = 2 * Math.Asin(Math.Sqrt(a));

            // Radius of earth in 
            // kilometers. Use 3956 
            // for miles
            double r = 6371;

            _logger.LogInformation("จบกระบวนการ Haversine formula");

            double result = (c * r);
            return result;
        }
        public double ConvertDegreesToRadians(double degrees)
        {
            _logger.LogInformation("เริ่มต้นกระบวน Convert Degrees To Radians");
            double result = (degrees * Math.PI) / 180;
            _logger.LogInformation("จบกระบวน Convert Degrees To Radians");
            return result;
        }
    }
}