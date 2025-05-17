# Event Management System 🎫

<div align="center">

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)

</div>

## 📋 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Live Demo](#live-demo)
- [Technology Stack](#technology-stack)
- [Installation](#installation)
  - [Prerequisites](#prerequisites)
  - [Local Setup](#local-setup)
  - [Docker Setup](#docker-setup)
- [Configuration](#configuration)
  - [Connection Strings](#connection-strings)
  - [Admin Credentials](#admin-credentials)
- [Usage](#usage)
- [Localization](#localization)
- [Dark Mode](#dark-mode)
- [API Documentation](#api-documentation)
- [Contributing](#contributing)
- [License](#license)

## 🔍 Overview

Event Management System is a comprehensive web application built with ASP.NET Core MVC that allows users to browse, book, and manage events. The system includes user authentication, event categorization, booking management, and an admin panel for system administration.

## ✨ Features

- **User Authentication & Authorization**
  - User registration and login
  - Role-based access control (Admin and User roles)

- **Event Management**
  - Create, edit, and delete events
  - Categorize events and add tags
  - Upload and manage event images
  - Filter events by category and tags
  - Search events by name, description, or venue

- **Booking System**
  - Book events with multiple tickets
  - View booking history
  - Cancel bookings

- **Admin Panel**
  - Dashboard with statistics
  - User management
  - Event management
  - Category and tag management
  - Booking management

- **Multilingual Support**
  - English and Arabic languages
  - RTL layout support for Arabic

- **Responsive Design**
  - Mobile-friendly interface
  - Dark mode support

## 🌐 Live Demo

The application is deployed and accessible at: [https://eventu.runasp.net/](https://eventu.runasp.net/)

## 🛠️ Technology Stack

- **Backend**
  - ASP.NET Core 8.0 MVC
  - Entity Framework Core 8.0
  - Identity Framework for authentication
  - SQL Server database

- **Frontend**
  - Razor Views
  - Bootstrap 5.3
  - Font Awesome 6
  - jQuery


## 📦 Installation

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Docker](https://www.docker.com/products/docker-desktop) (optional, for containerized deployment)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)

### Local Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/Event_Management_System.git
   cd Event_Management_System
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update the connection string**
   
   Edit `appsettings.json` and update the `DefaultConnection` string to point to your SQL Server instance.

4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

6. **Access the application**
   
   Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`

### Docker Setup

1. **Build the Docker image**
   ```bash
   docker build -t event-management-system .
   ```

2. **Run the container**
   ```bash
   docker run -d -p 8080:80 --name event-management event-management-system
   ```

3. **Access the application**
   
   Open your browser and navigate to `http://localhost:8080`

#### Using Docker Compose

1. **Create a Docker Compose file**
   
   Create a `docker-compose.yml` file with the following content:

   ```yaml
   version: '3.8'
   
   services:
     webapp:
       build:
         context: .
         dockerfile: Dockerfile
       ports:
         - "8080:80"
       environment:
         - ASPNETCORE_ENVIRONMENT=Production
         - ConnectionStrings__DefaultConnection=Server=db;Database=EventManagementDb;User=sa;Password=YourStrong!Password;TrustServerCertificate=True;
       depends_on:
         - db
   
     db:
       image: mcr.microsoft.com/mssql/server:2022-latest
       environment:
         - ACCEPT_EULA=Y
         - SA_PASSWORD=YourStrong!Password
       ports:
         - "1433:1433"
       volumes:
         - sqldata:/var/opt/mssql
   
   volumes:
     sqldata:
   ```

2. **Run with Docker Compose**
   ```bash
   docker-compose up -d
   ```

3. **Access the application**
   
   Open your browser and navigate to `http://localhost:8080`

## ⚙️ Configuration

### Connection Strings

The application uses SQL Server by default. The connection string is stored in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=EventManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

For production environments, it's recommended to use environment variables or a secret manager.

### Admin Credentials

Admin credentials are defined in the `AdminUser` section of the configuration:

```json
"AdminUser": {
  "Email": "admin@events.com",
  "Password": "Admin123!",
  "FirstName": "Admin",
  "LastName": "User"
}
```

#### Using User Secrets (Development)

1. Initialize the user secrets for your project:
   ```bash
   dotnet user-secrets init
   ```

2. Set the admin credentials:
   ```bash
   dotnet user-secrets set "AdminUser:Email" "admin@events.com"
   dotnet user-secrets set "AdminUser:Password" "YourSecurePassword"
   dotnet user-secrets set "AdminUser:FirstName" "Admin"
   dotnet user-secrets set "AdminUser:LastName" "User"
   ```

#### Using Environment Variables (Production)

In production, you can set environment variables:

```bash
ADMINUSER__EMAIL=admin@events.com
ADMINUSER__PASSWORD=YourSecurePassword
ADMINUSER__FIRSTNAME=Admin
ADMINUSER__LASTNAME=User
```

#### Using Key Vault or Secrets Manager

For cloud deployments, consider using:
- Azure Key Vault
- AWS Secrets Manager
- HashiCorp Vault

## 🚀 Usage

### User Interface

1. **Home Page**: Browse featured and upcoming events
2. **Event Details**: View event information and book tickets
3. **My Bookings**: View and manage your bookings
4. **User Profile**: Update your personal information

### Admin Panel

1. **Dashboard**: View system statistics
2. **Events Management**: Create, edit, and delete events
3. **Categories**: Manage event categories
4. **Users**: Manage system users and assign roles
5. **Bookings**: View and manage all bookings

To access the admin panel:
1. Log in as an admin user
2. Click on "Admin Panel" in the top navigation bar

## 🌐 Localization

The application supports multiple languages:

- English (default)
- Arabic (with RTL support)

To change the language, click on the language dropdown in the top navigation bar.

## 🌓 Dark Mode

The application includes a dark mode feature that can be toggled by clicking the moon/sun icon in the navigation bar.


## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details. 
