# Event Management System

## Admin Credentials Management

The application uses configuration-based admin credentials stored in `appsettings.json`. In a production environment, you should:

1. Move these credentials to secret storage like Azure Key Vault, AWS Secrets Manager, or environment variables
2. Use User-Secrets in development environment

### Admin User Configuration

Admin credentials are defined in the `AdminUser` section of the configuration:

```json
"AdminUser": {
  "Email": "admin@events.com",
  "Password": "Admin123!",
  "FirstName": "Admin",
  "LastName": "User"
}
```

### Using User Secrets (Development)

1. Initialize the user secrets for your project:
   ```
   dotnet user-secrets init
   ```

2. Set the admin credentials:
   ```
   dotnet user-secrets set "AdminUser:Email" "admin@events.com"
   dotnet user-secrets set "AdminUser:Password" "YourSecurePassword"
   dotnet user-secrets set "AdminUser:FirstName" "Admin"
   dotnet user-secrets set "AdminUser:LastName" "User"
   ```

### Using Environment Variables (Production)

In production, you can set environment variables:

```
ADMINUSER__EMAIL=admin@events.com
ADMINUSER__PASSWORD=YourSecurePassword
ADMINUSER__FIRSTNAME=Admin
ADMINUSER__LASTNAME=User
```

### Using Key Vault or Secrets Manager

For cloud deployments, consider using:
- Azure Key Vault
- AWS Secrets Manager
- HashiCorp Vault

## Administrator Management

The application includes a user management interface that allows:

1. Viewing all registered users
2. Editing user details
3. Assigning admin privileges to users
4. Removing users

To access these features:
1. Log in as an admin user
2. Navigate to Admin Panel (link appears in top navigation for admin users)
3. Click on "Users" in the sidebar or the Users card on the dashboard 