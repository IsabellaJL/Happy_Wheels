using PruebaC_sharp_IsabellaJimenez.Models;

namespace PruebaC_sharp_IsabellaJimenez.Services
{
    public class VehicleService
    {
        private readonly DataStorage _storage;

        public VehicleService(DataStorage storage)
        {
            _storage = storage;
        }

        public void RegisterVehicle(string licensePlate, string brand, string model, int year, VehicleType type, int clientId)
        {
            try
            {
                ValidateVehicleData(licensePlate, brand, model, year);

                var client = _storage.Clients.FirstOrDefault(c => c.Id == clientId);
                if (client == null)
                {
                    throw new InvalidOperationException($"Client with ID {clientId} not found.");
                }

                if (_storage.Vehicles.Any(v => v.LicensePlate.Equals(licensePlate, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"A vehicle with license plate {licensePlate} already exists.");
                }

                var vehicle = new Vehicle
                {
                    Id = _storage.GetNextVehicleId(),
                    LicensePlate = licensePlate.ToUpper(),
                    Brand = brand,
                    Model = model,
                    Year = year,
                    Type = type,
                    ClientId = clientId
                };

                _storage.Vehicles.Add(vehicle);
                Console.WriteLine($"\n✓ Vehicle registered successfully with ID: {vehicle.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error registering vehicle: {ex.Message}");
            }
        }

        public void EditVehicle(int vehicleId, string brand, string model, int year, VehicleType type)
        {
            try
            {
                var vehicle = _storage.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found.");
                }

                ValidateVehicleData(vehicle.LicensePlate, brand, model, year);

                vehicle.Brand = brand;
                vehicle.Model = model;
                vehicle.Year = year;
                vehicle.Type = type;

                Console.WriteLine($"\n✓ Vehicle updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error editing vehicle: {ex.Message}");
            }
        }

        public void ListVehiclesByClient(int clientId)
        {
            try
            {
                var vehicles = _storage.Vehicles.Where(v => v.ClientId == clientId).ToList();

                if (!vehicles.Any())
                {
                    Console.WriteLine($"\nNo vehicles registered for client ID {clientId}.");
                    return;
                }

                Console.WriteLine($"\n=== VEHICLES FOR CLIENT ID {clientId} ===");
                foreach (var vehicle in vehicles)
                {
                    Console.WriteLine(vehicle);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error listing vehicles: {ex.Message}");
            }
        }

        public void SearchVehicleByPlate(string licensePlate)
        {
            try
            {
                var vehicle = _storage.Vehicles.FirstOrDefault(v => 
                    v.LicensePlate.Equals(licensePlate, StringComparison.OrdinalIgnoreCase));

                if (vehicle == null)
                {
                    Console.WriteLine($"\nNo vehicle found with license plate: {licensePlate}");
                    return;
                }

                var client = _storage.Clients.FirstOrDefault(c => c.Id == vehicle.ClientId);
                Console.WriteLine("\n=== VEHICLE FOUND ===");
                Console.WriteLine(vehicle);
                Console.WriteLine($"Owner: {client?.Name ?? "Unknown"}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error searching vehicle: {ex.Message}");
            }
        }

        public Vehicle GetVehicleById(int vehicleId)
        {
            return _storage.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
        }

        private void ValidateVehicleData(string licensePlate, string brand, string model, int year)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
                throw new ArgumentException("License plate cannot be empty.");

            if (string.IsNullOrWhiteSpace(brand))
                throw new ArgumentException("Brand cannot be empty.");

            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model cannot be empty.");

            if (year < 1900 || year > DateTime.Now.Year + 1)
                throw new ArgumentException($"Year must be between 1900 and {DateTime.Now.Year + 1}.");
        }
    }
}