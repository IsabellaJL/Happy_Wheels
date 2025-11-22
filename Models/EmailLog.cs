namespace PruebaC_sharp_IsabellaJimenez.Models
{
    public class EmailLog
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailStatus Status { get; set; }
        public DateTime SentAt { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} | Appointment ID: {AppointmentId} | To: {RecipientEmail} | Status: {Status} | Sent: {SentAt:yyyy-MM-dd HH:mm}";
        }
    }
}