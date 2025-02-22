using EvacuationPlanning.Core.Entities.Vehicles;
using EvacuationPlanning.Core.Interfaces.IRepo.IVehicles;

namespace EvacuationPlanning.Infrastructure.Repositories.Vehicles
{
    public class VehiclesRepository : IVehiclesRepository
    {
        private static List<VehiclesEntities> DataVehicles = new List<VehiclesEntities>();

        public Task<bool> Add(VehiclesEntities request)
        {
            if (request == null) return Task.FromResult(false);

            var data = new VehiclesEntities()
            {
                VehiclesId = Guid.NewGuid(),
                Speed = request.Speed,
                Capacity = request.Capacity,
                Type = request.Type,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
            };

            DataVehicles.Add(data);

            return Task.FromResult(true);
        }

        public async Task DeleteAll()
        {
            DataVehicles.Clear();
        }

        public Task<List<VehiclesEntities>> GetAll()
        {
            var data = new List<VehiclesEntities>
            {
                new VehiclesEntities
                {
                    VehiclesId = new Guid("11111111-1111-1111-1111-111111111111"),
                    Type = "Bus",
                    Capacity = 40,
                    Latitude = 13.7563,
                    Longitude = 100.5018,
                    Speed = 60
                },
                new VehiclesEntities
                {
                    VehiclesId = new Guid("22222222-2222-2222-2222-222222222222"),
                    Type = "Van",
                    Capacity = 12,
                    Latitude = 13.7450,
                    Longitude = 100.4934,
                    Speed = 50
                },
                new VehiclesEntities
                {
                    VehiclesId = new Guid("33333333-3333-3333-3333-333333333333"),
                    Type = "Boat",
                    Capacity = 30,
                    Latitude = 13.7300,
                    Longitude = 100.5000,
                    Speed = 40
                }
            };
            return Task.FromResult(data);

        }
    }
}
