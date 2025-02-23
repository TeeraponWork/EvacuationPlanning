using EvacuationPlanning.Core.Entities.EvacuationZones;
using EvacuationPlanning.Core.Interfaces.IRedis;
using EvacuationPlanning.Core.Interfaces.IRepo.IEvacuationZones;

namespace EvacuationPlanning.Infrastructure.Repositories.EvacuationDataZones
{
    public class EvacuationZonesRepository : IEvacuationZonesRepository
    {
        private readonly IRedisService _redisService;
        public EvacuationZonesRepository(IRedisService redisService)
        {
            _redisService = redisService;
        }
        public async Task<bool> Add(EvacuationZonesEntities request)
        {
            string zoneKey = $"zone:{request.ZoneID}";
            var zoneData = new Dictionary<string, string>
            {
                { "Latitude", request.Latitude.ToString() },
                { "Longitude", request.Longitude.ToString() },
                { "Level", request.Level.ToString() },
                { "Capacity", request.NumberPeople.ToString() },
            };

            return await _redisService.SetHashAsync(zoneKey, zoneData, TimeSpan.FromHours(1));
        }
        public async Task<bool> AddSet(string zoneID)
        {
            return await _redisService.AddToSetAsync("zoneAll", zoneID);
        }
        public async Task<List<EvacuationZonesEntities>> GetAll()
        {
            var result = new List<EvacuationZonesEntities>();
            var zoneId = await _redisService.GetSetMemberAsync("zoneAll");
            if (zoneId.Count == 0) return result;

            foreach (var item in zoneId)
            {
                var data = await GetById(item);
                if (data == null) continue;
                result.Add(data);
            }
            return result;
        }
        public async Task<EvacuationZonesEntities> GetById(string zoneId)
        {
            string zoneIdKey = $"zone:{zoneId}";
            var data = await _redisService.GetHashAsync(zoneIdKey);
            if (data.Count == 0) return null;

            return new EvacuationZonesEntities
            {
                ZoneID = zoneId,
                Latitude = double.Parse(data["Latitude"]),
                Longitude = double.Parse(data["Longitude"]),
                NumberPeople = int.Parse(data["Capacity"]),
                Level = int.Parse(data["Level"]),
            };
        }
        public async Task<bool> DeleteAll()
        {
            var dataZones = await GetAll();
            foreach (var item in dataZones) 
            {
                await _redisService.DeleteAsync($"zone:{item.ZoneID}");
            }
            return await _redisService.DeleteAsync("zoneAll");
        }
    }
}
