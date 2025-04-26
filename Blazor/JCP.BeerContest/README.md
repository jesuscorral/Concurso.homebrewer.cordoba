# Beer Contest Web Application

A Blazor web application for managing artisanal beer contests, built with Clean Architecture and vertical slices.

## Project Overview

This application allows for the organization and management of artisanal beer contests. It provides features for:

- Contest registration and management
- Beer registration by participants
- Beer judging by appointed judges
- User management with different roles (Participant, Judge, Administrator)

## Architecture

The application follows Clean Architecture principles with vertical slices for feature organization:

- **Domain Layer**: Contains the core business entities and repository interfaces
- **Application Layer**: Contains the application logic organized in vertical slices using MediatR
- **Infrastructure Layer**: Contains the implementation of repositories using Google Cloud Firestore
- **Web Layer**: Contains the Blazor web application with UI components

## Technologies Used

- ASP.NET Core Blazor
- Clean Architecture
- Vertical Slices Architecture with MediatR
- Google Cloud Firestore for data storage
- Google Authentication for user login

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- Google Cloud account with Firestore enabled
- Google OAuth credentials for authentication

### Configuration

1. Update the `appsettings.json` file with your Google Cloud project ID and credentials:

```json
{
  "Authentication": {
    "Google": {
      "ClientId": "your-google-client-id",
      "ClientSecret": "your-google-client-secret"
    }
  },
  "GoogleCloud": {
    "ProjectId": "your-google-cloud-project-id",
    "Credentials": {
      "Path": "path-to-your-service-account-key.json"
    }
  }
}
```

2. Place your Google Cloud service account key file in a secure location and update the path in the configuration.

### Running the Application

1. Clone the repository
2. Navigate to the project directory
3. Run the application:

```
dotnet run --project BeerContest.Web
```

4. Open your browser and navigate to `https://localhost:5001`

## User Roles

- **Participant**: Can register beers for contests (up to 3 per contest) and view their own registered beers
- **Judge**: Can view and rate beers assigned to them (without seeing brewer personal information)
- **Administrator**: Has full access to manage contests, beers, and users

## License

This project is licensed under the MIT License - see the LICENSE file for details.