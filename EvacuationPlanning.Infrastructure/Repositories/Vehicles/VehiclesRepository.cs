using EvacuationPlanning.Core.Entities.Vehicles;
using EvacuationPlanning.Core.Interfaces.IRedis;
using EvacuationPlanning.Core.Interfaces.IRepo.IVehicles;

namespace EvacuationPlanning.Infrastructure.Repositories.Vehicles
{
    public class VehiclesRepository : IVehiclesRepository
    {
        private readonly IRedisService _redisService;
        public VehiclesRepository(IRedisService redisService)
        {
            _redisService = redisService;
        }
        private static List<VehiclesEntities> DataVehicles = new List<VehiclesEntities>();

        public async Task<bool> Add(VehiclesEntities request)
        {
            string vehicleKey = $"vehicle:{request.VehicleId}";
            var vehicleData = new Dictionary<string, string>
            {
                { "Latitude", request.Latitude.ToString() },
                { "Longitude", request.Longitude.ToString() },
                { "Type", request.Type },
                { "Capacity", request.Capacity.ToString() },
                { "Speed", request.Speed.ToString() }
            };

            return await _redisService.SetHashAsync(vehicleKey, vehicleData, TimeSpan.FromHours(1));
        }
        public async Task<bool> AddSet(string vehicleId)
        {
            return await _redisService.AddToSetAsync("vehicleAll", vehicleId);
        }
        public async Task<List<VehiclesEntities>> GetAll()
        {
            var result = new List<VehiclesEntities>();
            var vehicleId = await _redisService.GetSetMemberAsync("vehicleAll");
            if(vehicleId.Count == 0) return result;

            foreach (var item in vehicleId)
            {
                var data = await GetById(item);
                if (data == null) continue; 
                result.Add(data);
            }
            return result;            
        }
        public async Task<VehiclesEntities> GetById(string vehicleId)
        {
            string vehicleKey = $"vehicle:{vehicleId}";
            var data = await _redisService.GetHashAsync(vehicleKey);
            if (data.Count == 0) return null;

            return new VehiclesEntities
            {
                VehicleId = vehicleId,
                Latitude = double.Parse(data["Latitude"]),
                Longitude = double.Parse(data["Longitude"]),
                Type = data["Type"],
                Capacity = int.Parse(data["Capacity"]),
                Speed = int.Parse(data["Speed"])
            };
        }
        public async Task<bool> DeleteAll()
        {
            var dataVehicles = await GetAll();
            foreach (var item in dataVehicles)
            {
                await _redisService.DeleteAsync($"vehicle:{item.VehicleId}");
            }
            return await _redisService.DeleteAsync("vehicleAll");
        }
    }
}
