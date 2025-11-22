namespace PruebaC_sharp_IsabellaJimenez.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} | Name: {Name} | Doc: {Document} | Phone: {Phone} | Email: {Email}";
        }
    }
}