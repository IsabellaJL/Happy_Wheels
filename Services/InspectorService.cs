using System.Text.RegularExpressions;
using PruebaC_sharp_IsabellaJimenez.Models;

namespace PruebaC_sharp_IsabellaJimenez.Services
{
    public class InspectorService
    {
        private readonly DataStorage _storage;

        public InspectorService(DataStorage storage)
        {
            _storage = storage;
        }

        public void RegisterInspector(string name, string document, InspectionType inspectionType, string phone, string email)
        {
            try
            {
                ValidateInspectorData(name, document, phone, email);

                if (_storage.Inspectors.Any(i => i.Document.Equals(document, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException($"An inspector with document {document} already exists.");
                }

                var inspector = new Inspector
                {
                    Id = _storage.GetNextInspectorId(),
                    Name = name,
                    Document = document,
                    InspectionType = inspectionType,
                    Phone = phone,
                    Email = email
                };

                _storage.Inspectors.Add(inspector);
                Console.WriteLine($"\n✓ Inspector registered successfully with ID: {inspector.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error registering inspector: {ex.Message}");
            }
        }

        public void EditInspector(int inspectorId, string name, string phone, string email, InspectionType inspectionType)
        {
            try
            {
                var inspector = _storage.Inspectors.FirstOrDefault(i => i.Id == inspectorId);
                if (inspector == null)
                {
                    throw new InvalidOperationException($"Inspector with ID {inspectorId} not found.");
                }

                ValidateInspectorData(name, inspector.Document, phone, email);

                inspector.Name = name;
                inspector.Phone = phone;
                inspector.Email = email;
                inspector.InspectionType = inspectionType;

                Console.WriteLine($"\n✓ Inspector updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error editing inspector: {ex.Message}");
            }
        }

        public void ListAllInspectors(InspectionType? filterType = null)
        {
            try
            {
                var inspectors = filterType.HasValue
                    ? _storage.Inspectors.Where(i => i.InspectionType == filterType.Value).ToList()
                    : _storage.Inspectors.ToList();

                if (!inspectors.Any())
                {
                    Console.WriteLine("\nNo inspectors found.");
                    return;
                }

                Console.WriteLine(filterType.HasValue
                    ? $"\n=== INSPECTORS - TYPE: {filterType.Value} ==="
                    : "\n=== ALL INSPECTORS ===");

                foreach (var inspector in inspectors.OrderBy(i => i.Name))
                {
                    Console.WriteLine(inspector);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ Error listing inspectors: {ex.Message}");
            }
        }

        public Inspector GetInspectorById(int inspectorId)
        {
            return _storage.Inspectors.FirstOrDefault(i => i.Id == inspectorId);
        }

        private void ValidateInspectorData(string name, string document, string phone, string email)
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