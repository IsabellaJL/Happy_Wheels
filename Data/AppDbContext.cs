using PruebaC_sharp_IsabellaJimenez.Models;

namespace PruebaC_sharp_IsabellaJimenez.Data;

public class AppDbContext
{
    public List<Client> Clients { get; set; } = new List<Client>();
    public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    public List<Inspector> Inspectors { get; set; } = new List<Inspector>();
    public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    public List<EmailLog> EmailHistories { get; set; } = new List<EmailLog>();

    public AppDbContext()
    {
        // No default data (clean and empty)
    }
}