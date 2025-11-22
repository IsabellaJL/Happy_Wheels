using System.Text.RegularExpressions;
using PruebaC_sharp_IsabellaJimenez.Models;

namespace PruebaC_sharp_IsabellaJimenez.Services
{
    public class ClientService
    {
        private readonly DataStorage _storage;

        public ClientService(DataStorage storage)
        {
            _storage = storage;
        }

        public void RegisterClient(string name, string document, string phone, string email, string address)
        {
            try
            {
                ValidateClientData(name, document, phone, email);

                if (_storage.Clients.Any(c => c.Document.Equals(document, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"A client with document {document} already exists.");
                }

                var client = new Client
                {
                    Id = _storage.GetNextClientId(),
                    Name = name,
                    Document = document,
                    Phone = phone,
                    Email = email,
                    Address = address
                };

                _storage.Clients.Add(client);
                Console.WriteLine($"\n✓ Client registered successfully with ID: {client.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error registering client: {ex.Message}");
            }
        }

        public void EditClient(int clientId, string name, string phone, string email, string address)
        {
            try
            {
                var client = _storage.Clients.FirstOrDefault(c => c.Id == clientId);
                if (client == null)
                {
                    throw new InvalidOperationException($"Client with ID {clientId} not found.");
                }

                ValidateClientData(name, client.Document, phone, email);

                client.Name = name;
                client.Phone = phone;
                client.Email = email;
                client.Address = address;

                Console.WriteLine($"\n✓ Client updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error editing client: {ex.Message}");
            }
        }

        public void ListAllClients()
        {
            try
            {
                if (!_storage.Clients.Any())
                {
                    Console.WriteLine("\nNo clients registered in the system.");
                    return;
                }

                Console.WriteLine("\n=== CLIENT LIST ===");
                foreach (var client in _storage.Clients.OrderBy(c => c.Name))
                {
                    Console.WriteLine(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error listing clients: {ex.Message}");
            }
        }

        public Client GetClientById(int clientId)
        {
            return _storage.Clients.FirstOrDefault(c => c.Id == clientId);
        }

        private void ValidateClientData(string name, string document, string phone, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            if (string.IsNullOrWhiteSpace(document))
                throw new ArgumentException("Document cannot be empty.");

            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone cannot be empty.");

            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format.");
        }
    }
}