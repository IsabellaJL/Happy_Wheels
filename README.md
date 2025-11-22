# Happy_Wheels_Technical_Review_Center

A comprehensive C# console application for managing technical-mechanical vehicle review appointments, designed
to digitalize and optimize the management of clients, vehicles, inspectors, and appointments.

## Table of Contents

- Overview
- Features
- Project Structure
- Prerequisites
- Installation
- Usage
- Business Rules
- Technical Details
- Sample Data
- Error Handling


## Overview
This system was developed to solve the operational challenges faced by "Happy Wheels" Technical Review Center, 
which previously managed appointments using physical agendas and spreadsheets. The new digital system eliminates:

- Duplicate appointments for the same inspector or vehicle
- Difficulty finding client and vehicle information
- Lack of control over technical inspectors and their inspection types
- Loss of information when agendas are damaged or misplaced
- Inability to verify which vehicles have scheduled appointments


# Features
## 1. Client Management

- Register new clients with personal data (name, document, phone, email, address)
- Edit existing client information
- Validate unique document identification
- List all registered clients

## 2. Vehicle Management

- Register vehicles associated with specific clients
- Support for three vehicle types: Automobile, Motorcycle, Heavy Vehicle
- Edit vehicle information
- Validate unique license plates
- List all vehicles by client
- Search vehicles by license plate

## 3. Inspector Management

- Register technical inspectors with their specialization
- Three inspection types: Light, Heavy, Motorcycle
- Edit inspector information
- Validate unique inspector documents
- List all inspectors with optional filtering by inspection type

## 4. Appointment Management

- Schedule technical review appointments
- Automatic compatibility validation (inspector type vs. vehicle type)
- Prevent scheduling conflicts:

Inspectors cannot have multiple appointments at the same time
Vehicles cannot have multiple appointments at the same time


- Cancel appointments (status change to "Cancelled")
- Complete appointments (status change to "Completed")
- List appointments by client, vehicle, or inspector
- Automatic email confirmation upon appointment scheduling

## 5. Email System

- Automatic confirmation emails sent to clients
- Complete email history with status tracking (Sent/Not Sent)
- Email simulation


# Project Structure


```plaintext
PruebaC-sharp_IsabellaJimenez/
│
├── Models/
│   ├── Enums.cs                  # VehicleType, InspectionType, AppointmentStatus, EmailStatus
│   ├── Client.cs                 # Client entity
│   ├── Vehicle.cs                # Vehicle entity
│   ├── Inspector.cs              # Inspector entity
│   ├── Appointment.cs            # Appointment entity
│   ├── EmailLog.cs               # Email log entity
│
├── Data/                         # In-memory data storage with ID generation
│   ├── AppDbContext.cs
│
├── Services/
│   ├── ClientService.cs          # Client business logic
│   ├── VehicleService.cs         # Vehicle business logic
│   ├── InspectorService.cs       # Inspector business logic
│   ├── AppointmentService.cs     # Appointment business logic
│   └── EmailService.cs           # Email sending and logging
│
└── Program.cs                     # Main entry point and user interface menus
```

# Prerequisites

NET SDK 8.0 or higher


# Installation

## 1. Clone or Download the Project
Create a new directory and add all the files according to the project structure above.

## 2. Organize Files
Create the following folder structure:
mkdir Models
mkdir Services

Then place each file in its corresponding folder:

All model classes in the Models/ folder
All service classes in the Services/ folder
Program.cs in the root directory

# Usage

## Main Menu
When you run the application, you'll see the main menu:


║  HAPPY WHEELS - TECHNICAL REVIEW SYSTEM  ║

1.  Client Management
2.  Vehicle Management
3.  Inspector Management
4.  Appointment Management
5.  Email History
0.  Exit

## Navigation

- Enter the number corresponding to your desired option
- Follow the on-screen prompts
- Press 0 to return to the previous menu or exit

## The system will:

- Validate compatibility between vehicle and inspector types
- Check for scheduling conflicts
- Send confirmation email to the client
- Log the email status


## Business Rules

```plaintext
Vehicle and Inspector Compatibility

Vehicle Type        |  Compatible Inspector Type
Automobile          |  Light
Motorcycle          |  Motorcycle
Heavy Vehicle       |  Heavy
```

## Validation Rules

- Unique Constraints:

Client documents must be unique
Inspector documents must be unique
Vehicle license plates must be unique


- Appointment Constraints:

One inspector cannot have multiple appointments at the same time
One vehicle cannot have multiple appointments at the same time
Inspector type must be compatible with vehicle type


- Data Validation:

Email addresses must follow valid format
Vehicle year must be between 1900 and current year + 1
All required fields must be provided


# Technical Details

## Technologies Used

- Language: C# 
- Framework: .NET 8.0
- Data Storage: In-memory Lists and LINQ
- Architecture: Service-oriented with separation of concerns

## Design Patterns Applied

- Service Layer Pattern: Business logic separated into service classes
- Repository Pattern: Data storage abstraction through DataStorage class
- Single Responsibility Principle: Each service handles one domain entity

## Data Structures

- List<T> for storing entities
- LINQ for querying and filtering data
- Auto-incrementing ID counters for entity identification

## OOP Principles Applied

- Encapsulation: Private fields and public methods
- Abstraction: Service layer abstracts business logic
- Inheritance: Implicit through .NET base classes
- Polymorphism: Enum-based type system


# Sample Data
The system comes preloaded with sample data for testing:

## Clients

1. Bloom Peters (DOC123456)
2. Valtor Garcia (DOC789012)
3. Chimera Johnson (DOC345678)

Inspectors

1. Riven Rodriguez - Light Inspection
2. Musa Martinez - Motorcycle Inspection
3. Helio Hernandez - Heavy Inspection

Vehicles

1. ABC123 - Toyota Corolla 2020 (Automobile) - Owner: Bloom Peters
2. XYZ789 - Honda CBR500 2021 (Motorcycle) - Owner: Valtor Garcia
3. DEF456 - Ford F-150 2019 (Automobile) - Owner: Chimera Johnson
4. GHI789 - Yamaha MT-07 2022 (Motorcycle) - Owner: Bloom Peters


# Email System

## Email Confirmation Format

Subject: Technical Review Appointment Confirmation

Dear [Client Name],

Your technical review appointment has been confirmed:

Vehicle: [Brand] [Model] ([License Plate])
Date: [Day, Month DD, YYYY]
Time: [HH:mm]
Inspector: [Inspector Name]
Inspection Type: [Type]

Please arrive 10 minutes before your scheduled time.

Best regards,
Happy Wheels Technical Review Center

## Email Simulation

- The system simulates email sending
- All email attempts are logged with their status
- View complete email history from the main menu


# Coder info

- Isabella Jiménez Lázaro
- Clan caimán
- isabellajimenezlazaro@gmail.com
