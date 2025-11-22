namespace PruebaC_sharp_IsabellaJimenez.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int InspectorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} | Vehicle ID: {VehicleId} | Inspector ID: {InspectorId} | Date: {AppointmentDate:yyyy-MM-dd HH:mm} | Status: {Status}";
        }
    }
}