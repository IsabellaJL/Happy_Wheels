using PruebaC_sharp_IsabellaJimenez.Models;
using PruebaC_sharp_IsabellaJimenez.Services;

namespace PruebaC_sharp_IsabellaJimenez
{
    class Program
    {
        static DataStorage storage = new DataStorage();
        static ClientService clientService = new ClientService(storage);
        static VehicleService vehicleService = new VehicleService(storage);
        static InspectorService inspectorService = new InspectorService(storage);
        static EmailService emailService = new EmailService(storage);
        static AppointmentService appointmentService = new AppointmentService(storage, emailService);

        static void Main(string[] args)
        {
            LoadSampleData();

            bool running = true;
            while (running)
            {
                try
                {
                    Console.WriteLine("\n╔════════════════════════════════════════════╗");
                    Console.WriteLine("║  HAPPY WHEELS - TECHNICAL REVIEW SYSTEM   ║");
                    Console.WriteLine("╚════════════════════════════════════════════╝");
                    Console.WriteLine("\n1.  Customer's Management");
                    Console.WriteLine("2.  Vehicle Management");
                    Console.WriteLine("3.  Inspector Management");
                    Console.WriteLine("4.  Appointment Management");
                    Console.WriteLine("5.  Email History");
                    Console.WriteLine("0.  Exit");
                    Console.Write("\nSelect an option: ");

                    string? option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            ClientMenu();
                            break;
                        case "2":
                            VehicleMenu();
                            break;
                        case "3":
                            InspectorMenu();
                            break;
                        case "4":
                            AppointmentMenu();
                            break;
                        case "5":
                            emailService.ViewEmailHistory();
                            break;
                        case "0":
                            running = false;
                            Console.WriteLine("\nThank you for using Happy wheels Technical Review system!");
                            break;
                        default:
                            Console.WriteLine("\n✗ Invalid option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n✗ Unexpected error: {ex.Message}");
                }
            }
        }

        static void LoadSampleData()
        {
            // Default Clients
            clientService.RegisterClient("Bloom Peters", "DOC123456", "555-0101", "bloom.it@email.com", "123 Main St");
            clientService.RegisterClient("Valtor Garcia", "DOC789012", "555-0102", "lord.valtor@email.com", "456 Oak Ave");
            clientService.RegisterClient("Chimera Johnson", "DOC345678", "555-0103", "chimera.j@email.com", "789 Pine Rd");

            // Default Inspectors
            inspectorService.RegisterInspector("Riven Rodriguez", "INS001", InspectionType.Light, "555-0201", "riven.r@happywheels.com");
            inspectorService.RegisterInspector("Musa Martinez", "INS002", InspectionType.Motorcycle, "555-0202", "musa.m@happywheels.com");
            inspectorService.RegisterInspector("Helio Hernandez", "INS003", InspectionType.Heavy, "555-0203", "helio.h@happywheels.com");

            // Default Vehicles
            vehicleService.RegisterVehicle("ABC123", "Toyota", "Corolla", 2020, VehicleType.Automobile, 1);
            vehicleService.RegisterVehicle("XYZ789", "Honda", "CBR500", 2021, VehicleType.Motorcycle, 2);
            vehicleService.RegisterVehicle("DEF456", "Ford", "F-150", 2019, VehicleType.Automobile, 3);
            vehicleService.RegisterVehicle("GHI789", "Yamaha", "MT-07", 2022, VehicleType.Motorcycle, 1);

            Console.WriteLine("\n✓ Sample data loaded successfully!");
        }

        static void ClientMenu()
        {
            Console.WriteLine("\n=== CUSTOMER'S MANAGEMENT ===");
            Console.WriteLine("1. Register New Customer");
            Console.WriteLine("2. Edit Customer");
            Console.WriteLine("3. List All Customers");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("\nSelect an option: ");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    RegisterClientMenu();
                    break;
                case "2":
                    EditClientMenu();
                    break;
                case "3":
                    clientService.ListAllClients();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("\n✗ Invalid option.");
                    break;
            }
        }

        static void RegisterClientMenu()
        {
            Console.WriteLine("\n--- Register New Customer ---");
            Console.Write("Name: ");
            string? name = Console.ReadLine();

            Console.Write("Document: ");
            string? document = Console.ReadLine();

            Console.Write("Phone: ");
            string? phone = Console.ReadLine();

            Console.Write("Email: ");
            string? email = Console.ReadLine();

            Console.Write("Address: ");
            string? address = Console.ReadLine();

            clientService.RegisterClient(name, document, phone, email, address);
        }

        static void EditClientMenu()
        {
            Console.WriteLine("\n--- Edit Customer ---");
            Console.Write("Enter Customer's ID: ");
            if (int.TryParse(Console.ReadLine(), out int clientId))
            {
                var client = clientService.GetClientById(clientId);
                if (client != null)
                {
                    Console.WriteLine($"\nCurrent data: {client}");
                    Console.Write("New Name (press Enter to keep current): ");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name)) name = client.Name;

                    Console.Write("New Phone (press Enter to keep current): ");
                    string phone = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(phone)) phone = client.Phone;

                    Console.Write("New Email (press Enter to keep current): ");
                    string email = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(email)) email = client.Email;

                    Console.Write("New Address (press Enter to keep current): ");
                    string address = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(address)) address = client.Address;

                    clientService.EditClient(clientId, name, phone, email, address);
                }
                else
                {
                    Console.WriteLine($"\n✗ Customer with ID {clientId} not found.");
                }
            }
            else
            {
                Console.WriteLine("\n✗ Invalid ID format.");
            }
        }

        static void VehicleMenu()
        {
            Console.WriteLine("\n=== VEHICLE MANAGEMENT ===");
            Console.WriteLine("1. Register New Vehicle");
            Console.WriteLine("2. Edit Vehicle");
            Console.WriteLine("3. List Vehicles by Customer");
            Console.WriteLine("4. Search Vehicle by Plate");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("\nSelect an option: ");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    RegisterVehicleMenu();
                    break;
                case "2":
                    EditVehicleMenu();
                    break;
                case "3":
                    ListVehiclesByClientMenu();
                    break;
                case "4":
                    SearchVehicleMenu();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("\n✗ Invalid option.");
                    break;
            }
        }

        static void RegisterVehicleMenu()
        {
            Console.WriteLine("\n--- Register New Vehicle ---");
            clientService.ListAllClients();

            Console.Write("\nClient ID: ");
            if (!int.TryParse(Console.ReadLine(), out int clientId))
            {
                Console.WriteLine("\n✗ Invalid Customer's ID.");
                return;
            }

            Console.Write("License Plate: ");
            string plate = Console.ReadLine();

            Console.Write("Brand: ");
            string brand = Console.ReadLine();

            Console.Write("Model: ");
            string model = Console.ReadLine();

            Console.Write("Year: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("\n✗ Invalid year.");
                return;
            }

            Console.WriteLine("\nVehicle Types:");
            Console.WriteLine("0. Automobile");
            Console.WriteLine("1. Motorcycle");
            Console.WriteLine("2. HeavyVehicle");
            Console.Write("Select type: ");
            if (!int.TryParse(Console.ReadLine(), out int typeIndex) || typeIndex < 0 || typeIndex > 2)
            {
                Console.WriteLine("\n✗ Invalid vehicle type.");
                return;
            }

            VehicleType type = (VehicleType)typeIndex;
            vehicleService.RegisterVehicle(plate, brand, model, year, type, clientId);
        }

        static void EditVehicleMenu()
        {
            Console.WriteLine("\n--- Edit Vehicle ---");
            Console.Write("Enter Vehicle ID: ");
            if (int.TryParse(Console.ReadLine(), out int vehicleId))
            {
                var vehicle = vehicleService.GetVehicleById(vehicleId);
                if (vehicle != null)
                {
                    Console.WriteLine($"\nCurrent data: {vehicle}");
                    
                    Console.Write("New Brand (press Enter to keep current): ");
                    string brand = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(brand)) brand = vehicle.Brand;

                    Console.Write("New Model (press Enter to keep current): ");
                    string model = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(model)) model = vehicle.Model;

                    Console.Write("New Year (press Enter to keep current): ");
                    string yearInput = Console.ReadLine();
                    int year = string.IsNullOrWhiteSpace(yearInput) ? vehicle.Year : int.Parse(yearInput);

                    Console.WriteLine("\nVehicle Types:");
                    Console.WriteLine("0. Automobile");
                    Console.WriteLine("1. Motorcycle");
                    Console.WriteLine("2. HeavyVehicle");
                    Console.Write($"New Type (press Enter to keep current [{vehicle.Type}]): ");
                    string typeInput = Console.ReadLine();
                    VehicleType type = string.IsNullOrWhiteSpace(typeInput) ? vehicle.Type : (VehicleType)int.Parse(typeInput);

                    vehicleService.EditVehicle(vehicleId, brand, model, year, type);
                }
                else
                {
                    Console.WriteLine($"\n✗ Vehicle with ID {vehicleId} not found.");
                }
            }
            else
            {
                Console.WriteLine("\n✗ Invalid ID format.");
            }
        }

        static void ListVehiclesByClientMenu()
        {
            Console.WriteLine("\n--- List Vehicles by Customer ---");
            Console.Write("Enter Customer's ID: ");
            if (int.TryParse(Console.ReadLine(), out int clientId))
            {
                vehicleService.ListVehiclesByClient(clientId);
            }
            else
            {
                Console.WriteLine("\n✗ Invalid ID format.");
            }
        }

        static void SearchVehicleMenu()
        {
            Console.WriteLine("\n--- Search Vehicle by Plate ---");
            Console.Write("Enter License Plate: ");
            string plate = Console.ReadLine();
            vehicleService.SearchVehicleByPlate(plate);
        }

        static void InspectorMenu()
        {
            Console.WriteLine("\n=== INSPECTOR MANAGEMENT ===");
            Console.WriteLine("1. Register New Inspector");
            Console.WriteLine("2. Edit Inspector");
            Console.WriteLine("3. List All Inspectors");
            Console.WriteLine("4. List Inspectors by Type");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("\nSelect an option: ");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    RegisterInspectorMenu();
                    break;
                case "2":
                    EditInspectorMenu();
                    break;
                case "3":
                    inspectorService.ListAllInspectors();
                    break;
                case "4":
                    ListInspectorsByTypeMenu();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("\n✗ Invalid option.");
                    break;
            }
        }

        static void RegisterInspectorMenu()
        {
            Console.WriteLine("\n--- Register New Inspector ---");
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Document: ");
            string document = Console.ReadLine();

            Console.WriteLine("\nInspection Types:");
            Console.WriteLine("0. Light");
            Console.WriteLine("1. Heavy");
            Console.WriteLine("2. Motorcycle");
            Console.Write("Select type: ");
            if (!int.TryParse(Console.ReadLine(), out int typeIndex) || typeIndex < 0 || typeIndex > 2)
            {
                Console.WriteLine("\n✗ Invalid inspection type.");
                return;
            }

            Console.Write("Phone: ");
            string phone = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            InspectionType type = (InspectionType)typeIndex;
            inspectorService.RegisterInspector(name, document, type, phone, email);
        }

        static void EditInspectorMenu()
        {
            Console.WriteLine("\n--- Edit Inspector ---");
            Console.Write("Enter Inspector ID: ");
            if (int.TryParse(Console.ReadLine(), out int inspectorId))
            {
                var inspector = inspectorService.GetInspectorById(inspectorId);
                if (inspector != null)
                {
                    Console.WriteLine($"\nCurrent data: {inspector}");
                    
                    Console.Write("New Name (press Enter to keep current): ");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name)) name = inspector.Name;

                    Console.Write("New Phone (press Enter to keep current): ");
                    string phone = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(phone)) phone = inspector.Phone;

                    Console.Write("New Email (press Enter to keep current): ");
                    string email = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(email)) email = inspector.Email;

                    Console.WriteLine("\nInspection Types:");
                    Console.WriteLine("0. Light");
                    Console.WriteLine("1. Heavy");
                    Console.WriteLine("2. Motorcycle");
                    Console.Write($"New Type (press Enter to keep current [{inspector.InspectionType}]): ");
                    string typeInput = Console.ReadLine();
                    InspectionType type = string.IsNullOrWhiteSpace(typeInput) ? inspector.InspectionType : (InspectionType)int.Parse(typeInput);

                    inspectorService.EditInspector(inspectorId, name, phone, email, type);
                }
                else
                {
                    Console.WriteLine($"\n✗ Inspector with ID {inspectorId} not found.");
                }
            }
            else
            {
                Console.WriteLine("\n✗ Invalid ID format.");
            }
        }

        static void ListInspectorsByTypeMenu()
        {
            Console.WriteLine("\n--- List Inspectors by Type ---");
            Console.WriteLine("0. Light");
            Console.WriteLine("1. Heavy");
            Console.WriteLine("2. Motorcycle");
            Console.Write("Select type: ");
            if (int.TryParse(Console.ReadLine(), out int typeIndex) && typeIndex >= 0 && typeIndex <= 2)
            {
                InspectionType type = (InspectionType)typeIndex;
                inspectorService.ListAllInspectors(type);
            }
            else
            {
                Console.WriteLine("\n✗ Invalid inspection type.");
            }
        }

        static void AppointmentMenu()
        {
            Console.WriteLine("\n=== APPOINTMENT MANAGEMENT ===");
            Console.WriteLine("1. Schedule Appointment");
            Console.WriteLine("2. Cancel Appointment");
            Console.WriteLine("3. Complete Appointment");
            Console.WriteLine("4. List Appointments by Customer");
            Console.WriteLine("5. List Appointments by Vehicle");
            Console.WriteLine("6. List Appointments by Inspector");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("\nSelect an option: ");

            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ScheduleAppointmentMenu();
                    break;
                case "2":
                    CancelAppointmentMenu();
                    break;
                case "3":
                    CompleteAppointmentMenu();
                    break;
                case "4":
                    ListAppointmentsByClientMenu();
                    break;
                case "5":
                    ListAppointmentsByVehicleMenu();
                    break;
                case "6":
                    ListAppointmentsByInspectorMenu();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("\n✗ Invalid option.");
                    break;
            }
        }

        static void ScheduleAppointmentMenu()
        {
            Console.WriteLine("\n--- Schedule Appointment ---");
            
            Console.Write("Enter Vehicle ID: ");
            if (!int.TryParse(Console.ReadLine(), out int vehicleId))
            {
                Console.WriteLine("\n✗ Invalid Vehicle ID.");
                return;
            }

            Console.Write("Enter Inspector ID: ");
            if (!int.TryParse(Console.ReadLine(), out int inspectorId))
            {
                Console.WriteLine("\n✗ Invalid Inspector ID.");
                return;
            }

            Console.Write("Enter Date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("\n✗ Invalid date format.");
                return;
            }

            Console.Write("Enter Time (HH:mm): ");
            if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan time))
            {
                Console.WriteLine("\n✗ Invalid time format.");
                return;
            }

            DateTime appointmentDate = date.Date + time;
            appointmentService.ScheduleAppointment(vehicleId, inspectorId, appointmentDate);
        }

        static void CancelAppointmentMenu()
        {
            Console.WriteLine("\n--- Cancel Appointment ---");
            Console.Write("Enter Appointment ID: ");
            if (int.TryParse(Console.ReadLine(), out int appointmentId))
            {
                appointmentService.CancelAppointment(appointmentId);
            }
            else
            {
                Console.WriteLine("\n✗ Invalid ID format.");
            }
        }

        static void CompleteAppointmentMenu()
        {
            Console.WriteLine("\n--- Complete Appointment ---");
            Console.Write("Enter Appointment ID: ");
            if (int.TryParse(Console.ReadLine(), out int appointmentId))
            {
                appointmentService.CompleteAppointment(appointmentId);
            }
            else
            {
                Console.WriteLine("\n✗ Invalid ID format.");
            }
        }

        static void ListAppointmentsByClientMenu()
        {
            Console.WriteLine("\n--- List Appointments by Customer ---");
            Console.Write("Enter Client ID: ");
            if (int.TryParse(Console.ReadLine(), out int clientId))
            {
                appointmentService.ListAppointmentsByClient(clientId);
            }
            else
            {
                Console.WriteLine("\n✗ Invalid ID format.");
            }
        }

        static void ListAppointmentsByVehicleMenu()
        {
            Console.WriteLine("\n--- List Appointments by Vehicle ---");
            Console.Write("Enter Vehicle ID: ");
            if (int.TryParse(Console.ReadLine(), out int vehicleId))
            {
                appointmentService.ListAppointmentsByVehicle(vehicleId);
            }
            else
            {
                Console.WriteLine("\n✗ Invalid ID format.");
            }
        }

        static void ListAppointmentsByInspectorMenu()
        {
            Console.WriteLine("\n--- List Appointments by Inspector ---");
            Console.Write("Enter Inspector ID: ");
            if (int.TryParse(Console.ReadLine(), out int inspectorId))
            {
                appointmentService.ListAppointmentsByInspector(inspectorId);
            }
            else
            {
                Console.WriteLine("\n✗ Invalid ID format.");
            }
        }
    }
}