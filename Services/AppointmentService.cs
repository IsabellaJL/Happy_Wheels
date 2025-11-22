using PruebaC_sharp_IsabellaJimenez.Models;

namespace PruebaC_sharp_IsabellaJimenez.Services
{
    public class AppointmentService
    {
        private readonly DataStorage _storage;
        private readonly EmailService _emailService;

        public AppointmentService(DataStorage storage, EmailService emailService)
        {
            _storage = storage;
            _emailService = emailService;
        }

        public void ScheduleAppointment(int vehicleId, int inspectorId, DateTime appointmentDate)
        {
            try
            {
                var vehicle = _storage.Vehicles.FirstOrDefault(v => v.Id == vehicleId);
                if (vehicle == null)
                {
                    throw new InvalidOperationException($"Vehicle with ID {vehicleId} not found.");
                }

                var inspector = _storage.Inspectors.FirstOrDefault(i => i.Id == inspectorId);
                if (inspector == null)
                {
                    throw new InvalidOperationException($"Inspector with ID {inspectorId} not found.");
                }

                var client = _storage.Clients.FirstOrDefault(c => c.Id == vehicle.ClientId);

                // Validate compatibility
                if (!IsCompatible(vehicle.Type, inspector.InspectionType))
                {
                    throw new InvalidOperationException(
                        $"Inspector type '{inspector.InspectionType}' is not compatible with vehicle type '{vehicle.Type}'.");
                }

                // Validate inspector availability
                if (HasInspectorConflict(inspectorId, appointmentDate))
                {
                    throw new InvalidOperationException(
                        $"Inspector already has an appointment at {appointmentDate:yyyy-MM-dd HH:mm}.");
                }

                // Validate vehicle availability
                if (HasVehicleConflict(vehicleId, appointmentDate))
                {
                    throw new InvalidOperationException(
                        $"Vehicle already has an appointment at {appointmentDate:yyyy-MM-dd HH:mm}.");
                }

                var appointment = new Appointment
                {
                    Id = _storage.GetNextAppointmentId(),
                    VehicleId = vehicleId,
                    InspectorId = inspectorId,
                    AppointmentDate = appointmentDate,
                    Status = AppointmentStatus.Scheduled,
                    CreatedAt = DateTime.Now
                };

                _storage.Appointments.Add(appointment);
                Console.WriteLine($"\n✓ Appointment scheduled successfully with ID: {appointment.Id}");

                // Send confirmation email
                _emailService.SendAppointmentConfirmation(appointment, client, vehicle, inspector);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error scheduling appointment: {ex.Message}");
            }
        }

        public void CancelAppointment(int appointmentId)
        {
            try
            {
                var appointment = _storage.Appointments.FirstOrDefault(a => a.Id == appointmentId);
                if (appointment == null)
                {
                    throw new InvalidOperationException($"Appointment with ID {appointmentId} not found.");
                }

                if (appointment.Status == AppointmentStatus.Cancelled)
                {
                    throw new InvalidOperationException("Appointment is already cancelled.");
                }

                appointment.Status = AppointmentStatus.Cancelled;
                Console.WriteLine($"\n✓ Appointment {appointmentId} cancelled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error cancelling appointment: {ex.Message}");
            }
        }

        public void CompleteAppointment(int appointmentId)
        {
            try
            {
                var appointment = _storage.Appointments.FirstOrDefault(a => a.Id == appointmentId);
                if (appointment == null)
                {
                    throw new InvalidOperationException($"Appointment with ID {appointmentId} not found.");
                }

                if (appointment.Status == AppointmentStatus.Completed)
                {
                    throw new InvalidOperationException("Appointment is already completed.");
                }

                appointment.Status = AppointmentStatus.Completed;
                Console.WriteLine($"\n✓ Appointment {appointmentId} marked as completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error completing appointment: {ex.Message}");
            }
        }

        public void ListAppointmentsByClient(int clientId)
        {
            try
            {
                var vehicleIds = _storage.Vehicles
                    .Where(v => v.ClientId == clientId)
                    .Select(v => v.Id)
                    .ToList();

                var appointments = _storage.Appointments
                    .Where(a => vehicleIds.Contains(a.VehicleId))
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();

                if (!appointments.Any())
                {
                    Console.WriteLine($"\nNo appointments found for client ID {clientId}.");
                    return;
                }

                Console.WriteLine($"\n=== APPOINTMENTS FOR CLIENT ID {clientId} ===");
                foreach (var apt in appointments)
                {
                    var vehicle = _storage.Vehicles.First(v => v.Id == apt.VehicleId);
                    var inspector = _storage.Inspectors.First(i => i.Id == apt.InspectorId);
                    Console.WriteLine($"{apt} | Vehicle: {vehicle.LicensePlate} | Inspector: {inspector.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error listing appointments: {ex.Message}");
            }
        }

        public void ListAppointmentsByVehicle(int vehicleId)
        {
            try
            {
                var appointments = _storage.Appointments
                    .Where(a => a.VehicleId == vehicleId)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();

                if (!appointments.Any())
                {
                    Console.WriteLine($"\nNo appointments found for vehicle ID {vehicleId}.");
                    return;
                }

                Console.WriteLine($"\n=== APPOINTMENTS FOR VEHICLE ID {vehicleId} ===");
                foreach (var apt in appointments)
                {
                    var inspector = _storage.Inspectors.First(i => i.Id == apt.InspectorId);
                    Console.WriteLine($"{apt} | Inspector: {inspector.Name}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error listing appointments: {ex.Message}");
            }
        }

        public void ListAppointmentsByInspector(int inspectorId)
        {
            try
            {
                var appointments = _storage.Appointments
                    .Where(a => a.InspectorId == inspectorId)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();

                if (!appointments.Any())
                {
                    Console.WriteLine($"\nNo appointments found for inspector ID {inspectorId}.");
                    return;
                }

                Console.WriteLine($"\n=== APPOINTMENTS FOR INSPECTOR ID {inspectorId} ===");
                foreach (var apt in appointments)
                {
                    var vehicle = _storage.Vehicles.First(v => v.Id == apt.VehicleId);
                    Console.WriteLine($"{apt} | Vehicle: {vehicle.LicensePlate}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error listing appointments: {ex.Message}");
            }
        }

        private bool IsCompatible(VehicleType vehicleType, InspectionType inspectionType)
        {
            return (vehicleType, inspectionType) switch
            {
                (VehicleType.Motorcycle, InspectionType.Motorcycle) => true,
                (VehicleType.Automobile, InspectionType.Light) => true,
                (VehicleType.HeavyVehicle, InspectionType.Heavy) => true,
                _ => false
            };
        }

        private bool HasInspectorConflict(int inspectorId, DateTime appointmentDate)
        {
            return _storage.Appointments.Any(a =>
                a.InspectorId == inspectorId &&
                a.AppointmentDate == appointmentDate &&
                a.Status == AppointmentStatus.Scheduled);
        }

        private bool HasVehicleConflict(int vehicleId, DateTime appointmentDate)
        {
            return _storage.Appointments.Any(a =>
                a.VehicleId == vehicleId &&
                a.AppointmentDate == appointmentDate &&
                a.Status == AppointmentStatus.Scheduled);
        }
    }
}