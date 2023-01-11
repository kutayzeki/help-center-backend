# .NET Backend Template

This repository contains a .NET backend template that includes the following features:

- Entities Framework (EF) for handling identity and authentication
- JWT for secure authentication
- Login and register functionality
- Swagger for API documentation
- Response models for formatting API responses
- Versioning for the API
- Pagination for handling large sets of data
- Internationalization and localization for supporting multiple languages
- Rate limiting for controlling the rate of API requests
- Email service for sending emails
- Welcome email template
- Serilog for logging

## Getting Started

To get started with this template, you will need to clone the repository and set up the necessary dependencies.

1. Clone the repository by running the following command:
```sh
git clone https://github.com/kutayzeki/backendtemplate.git
```
2. Open the solution file in Visual Studio and restore the NuGet packages by right-clicking on the solution and selecting "Restore NuGet Packages".
3. Build the solution to make sure everything is working correctly.
4. Run the application on the Visual Studio and check the API's on your localhost by using a tool like postman or browser extension.
5. Make sure to update the appsettings.json to include your settings.


## Customization

You can customize this template to fit your needs by editing the following files:

* appsettings.json: contains the configuration settings for the application, including the connection string for the database, credentials for the email service and JWT security settings.
* Controllers: contains the controllers for handling API requests. You can add new controllers or edit existing ones to implement the functionality you need.
* Models: contains the model classes for the application. You can add new model classes or edit existing ones to match your data structure.
* Core: contains the repository, context and other related classes for handling data access.
