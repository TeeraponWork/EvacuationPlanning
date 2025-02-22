using EvacuationPlanning.Core.Entities.EvacuationZones;
using EvacuationPlanning.Core.Interfaces.IRepo.IEvacuationZones;

namespace EvacuationPlanning.Infrastructure.Repositories.EvacuationDataZones
{
    public class EvacuationZonesRepository : IEvacuationZonesRepository
    {
        private static List<EvacuationZonesEntities> DataEvacuationZones = new List<EvacuationZonesEntities>();

        public Task<bool> Add(EvacuationZonesEntities request)
        {
            if (request == null) return Task.FromResult(false);

            var data = new EvacuationZonesEntities()
            {
                ZoneID = Guid.NewGuid(),
                NumberPeople = request.NumberPeople,
                Longitude = request.Longitude,
                Latitude = request.Latitude,
                Level = request.Level
            };

            DataEvacuationZones.Add(data);

            return Task.FromResult(true);
        }

        public async Task<bool> DeleteAll()
        {
            DataEvacuationZones.Clear();
            return await Task.FromResult(true);
        }

        public Task<List<EvacuationZonesEntities>> GetAll()
        {
            var data = new List<EvacuationZonesEntities>
            {
                new EvacuationZonesEntities
                {
                    ZoneID = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Latitude = 13.7570,
                    Longitude = 100.5020,
                    NumberPeople = 50,
                    Level = 5
                },
                new EvacuationZonesEntities
                {
                    ZoneID = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Latitude = 13.7480,
                    Longitude = 100.4950,
                    NumberPeople = 30,
                    Level = 4
                },
                new EvacuationZonesEntities
                {
                    ZoneID = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                    Latitude = 13.7400,
                    Longitude = 100.4900,
                    NumberPeople = 20,
                    Level = 3
                }
            };
            return Task.FromResult(data);
        }
    }
}
