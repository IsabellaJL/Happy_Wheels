namespace PruebaC_sharp_IsabellaJimenez.Models
{
    public class DataStorage
    {
        public List<Client> Clients { get; set; } = new List<Client>();
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public List<Inspector> Inspectors { get; set; } = new List<Inspector>();
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<EmailLog> EmailLogs { get; set; } = new List<EmailLog>();

        private int clientIdCounter = 1;
        private int vehicleIdCounter = 1;
        private int inspectorIdCounter = 1;
        private int appointmentIdCounter = 1;
        private int emailLogIdCounter = 1;

        public int GetNextClientId() => clientIdCounter++;
        public int GetNextVehicleId() => vehicleIdCounter++;
        public int GetNextInspectorId() => inspectorIdCounter++;
        public int GetNextAppointmentId() => appointmentIdCounter++;
        public int GetNextEmailLogId() => emailLogIdCounter++;
    }

    
}