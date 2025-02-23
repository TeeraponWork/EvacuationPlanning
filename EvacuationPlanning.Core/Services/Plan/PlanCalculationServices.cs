using EvacuationPlanning.Core.Common;
using EvacuationPlanning.Core.Dto.Plan;
using EvacuationPlanning.Core.Entities.EvacuationZones;
using EvacuationPlanning.Core.Interfaces.IDistanceCalculation;
using EvacuationPlanning.Core.Interfaces.IPlan;
using EvacuationPlanning.Core.Interfaces.IRepo.IEvacuationZones;
using EvacuationPlanning.Core.Interfaces.IRepo.IVehicles;
using EvacuationPlanning.Core.Model.DistanceCalculation;
using Microsoft.Extensions.Logging;

namespace EvacuationPlanning.Core.Services.Plan
{
    public class PlanCalculationServices : IPlanCalculationServices
    {
        private readonly IDistanceCalculationServices _distanceCalculationServices;
        private readonly IEvacuationZonesRepository _evacuationZonesRepository;
        private readonly IVehiclesRepository _vehiclesRepository;
        private readonly ILogger<PlanCalculationServices> _logger;

        public PlanCalculationServices(IDistanceCalculationServices distanceCalculationServices,
            IEvacuationZonesRepository evacuationZonesRepository,
            IVehiclesRepository vehiclesRepository,
            ILogger<PlanCalculationServices> logger)
        {
            _distanceCalculationServices = distanceCalculationServices;
            _evacuationZonesRepository = evacuationZonesRepository;
            _vehiclesRepository = vehiclesRepository;
            _logger = logger;
        }
        public ResultDistanceModel ProcessEvacuationDistances(CoordinateModel model)
        {
            _logger.LogInformation("เริ่มต้นกระบวน Calculation Distance");
            double result = _distanceCalculationServices.CalculationDistance(model);
            var resultDistance = new ResultDistanceModel()
            {
                ZoneID = model.ZoneID,
                VehiclesId = model.VehiclesId,
                Distance = result
            };
            _logger.LogInformation("จบกระบวน Calculation Distance");
            return resultDistance;
        }

        public async Task<List<EvacuationZoneDto>> ProcessEvacuationPlan(
            List<ResultDistanceModel> dataDistance, 
            List<EvacuationZonesEntities> dataEvacuationZones)
        {
            var result = new List<EvacuationPlanDto>();
            var etaCalculator = new CalculationEta();

            _logger.LogInformation("เริ่มต้นกระบวนการ ดึงข้อมูลพาหนะ,พื้นที่อพยพ");
            var dataVehiclesAll = await _vehiclesRepository.GetAll();
            var dataEvacuationZonesAll = await _evacuationZonesRepository.GetAll();
            _logger.LogInformation("จบกระบวนการ ดึงข้อมูลพาหนะ,พื้นที่อพยพ");

            _logger.LogInformation("เริ่มต้นกระบวนการ ลำดับของพื้นที่เร่งด่วน");
            dataEvacuationZones = PriorityUrgency(dataEvacuationZones);
            _logger.LogInformation("จบกระบวนการ ลำดับของพื้นที่เร่งด่วน");

            _logger.LogInformation("เริ่มต้นกระบวนการ สร้างแผนอพยพ");
            foreach (var itemEvacuationZones in dataEvacuationZones)
            {
                _logger.LogInformation($"เริ่มต้นกระบวนการ สร้างแผนอพยพ: {itemEvacuationZones.ZoneID}");

                var evacuationVehicles = dataDistance.Where(x => x.ZoneID == itemEvacuationZones.ZoneID).ToList();
                //จำนวนคนที่อพยพ
                var dataNumberPeople = dataEvacuationZonesAll.Where(x => x.ZoneID == itemEvacuationZones.ZoneID).FirstOrDefault();
                if (dataNumberPeople?.NumberPeople == 0) continue;
                int numberPeople = dataNumberPeople.NumberPeople;

                foreach (var item in evacuationVehicles)
                {
                    _logger.LogInformation($"เริ่มต้นกระบวนการ สร้างวางแผนพาหนะ:{item.VehiclesId}");
                    var dataByVehicles = dataVehiclesAll.Where(x => x.VehicleId == item.VehiclesId).FirstOrDefault();
                    if (numberPeople != 0)
                    {
                        var data = new EvacuationPlanDto()
                        {
                            Eta = etaCalculator.ComputeEta(item.Distance, dataByVehicles.Speed),
                            NumberPeople = Math.Min(dataByVehicles.Capacity, numberPeople),
                            VehicleID = dataByVehicles.VehicleId,
                            ZoneID = item.ZoneID,
                        };
                        result.Add(data);
                        numberPeople -= data.NumberPeople;
                        _logger.LogInformation($"สร้างวางแผนพาหนะ:{dataByVehicles.VehicleId} ของ Zone: {item.ZoneID}");
                    }
                }
                _logger.LogInformation($"จบกระบวนการ สร้างแผนอพยพ: {itemEvacuationZones.ZoneID}");
            }
            _logger.LogInformation("จบกระบวนการ สร้างแผนอพยพ");

            var dataPlan = GroupByEvacuationPlan(result);
            return dataPlan;
        }
        private List<EvacuationZoneDto> GroupByEvacuationPlan(List<EvacuationPlanDto> data) 
        {
            var result = data.GroupBy(x => x.ZoneID)
                               .Select(g => new EvacuationZoneDto
                               {
                                   ZoneID = g.Key,
                                   Vehicles = g.Select(x => new EvacuationVehicleDto
                                   {
                                       Eta = $"{x.Eta} minutes" ,
                                       NumberPeople = x.NumberPeople,
                                       VehicleID = x.VehicleID,
                                   }).ToList()
                               }).ToList();

            return result;
        }
        private List<EvacuationZonesEntities> PriorityUrgency(List<EvacuationZonesEntities> data)
        {
            return data.OrderByDescending(x => x.Level).ToList();
        }
    }
}
