using PruebaC_sharp_IsabellaJimenez.Models;

namespace PruebaC_sharp_IsabellaJimenez.Services
{
    public class EmailService
    {
        private readonly DataStorage _storage;

        public EmailService(DataStorage storage)
        {
            _storage = storage;
        }

        public void SendAppointmentConfirmation(Appointment appointment, Client client, Vehicle vehicle, Inspector inspector)
        {
            try
            {
                var subject = "Technical Review Appointment Confirmation";
                var body = $@"
Dear {client.Name},

Your technical review appointment has been confirmed:

Vehicle: {vehicle.Brand} {vehicle.Model} ({vehicle.LicensePlate})
Date: {appointment.AppointmentDate:dddd, MMMM dd, yyyy}
Time: {appointment.AppointmentDate:HH:mm}
Inspector: {inspector.Name}
Inspection Type: {inspector.InspectionType}

Please arrive 10 minutes before your scheduled time.

Best regards,
Happy Wheels Technical Review Center";

                // Simulate email sending
                bool success = SimulateEmailSending(client.Email);

                var emailLog = new EmailLog
                {
                    Id = _storage.GetNextEmailLogId(),
                    AppointmentId = appointment.Id,
                    RecipientEmail = client.Email,
                    Subject = subject,
                    Body = body,
                    Status = success ? EmailStatus.Sent : EmailStatus.NotSent,
                    SentAt = DateTime.Now
                };

                _storage.EmailLogs.Add(emailLog);

                if (success)
                {
                    Console.WriteLine($"✓ Confirmation email sent to {client.Email}");
                }
                else
                {
                    Console.WriteLine($"✗ Failed to send confirmation email to {client.Email}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error sending email: {ex.Message}");
            }
        }

        public void ViewEmailHistory()
        {
            try
            {
                if (!_storage.EmailLogs.Any())
                {
                    Console.WriteLine("\nNo email history available.");
                    return;
                }

                Console.WriteLine("\n=== EMAIL HISTORY ===");
                foreach (var log in _storage.EmailLogs.OrderByDescending(e => e.SentAt))
                {
                    Console.WriteLine(log);
                }

                var sentCount = _storage.EmailLogs.Count(e => e.Status == EmailStatus.Sent);
                var notSentCount = _storage.EmailLogs.Count(e => e.Status == EmailStatus.NotSent);
                Console.WriteLine($"\nTotal: {_storage.EmailLogs.Count} | Sent: {sentCount} | Not Sent: {notSentCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error viewing email history: {ex.Message}");
            }
        }

        private bool SimulateEmailSending(string email)
        {
            // Simulate 90% success rate
            return new Random().Next(100) < 90;
        }
    }
}