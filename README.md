# Description
This project is a web server for group chat client application. Please refer to [DistributedChatClient](https://github.com/yjyotshna1997/DistributedChatClient) for the implementation of chat UI.

## Technologies
- Chat server is a web API written using ASP.NET 
- Chat client is an Angular Single page application
- Uses `Microsoft.Identity.Web` and MSAL to authorize the user to use chat server
- Establishes a web socket connection between browser and server on loading the page

## Prerequisites
Requires an account with Azure AD tenant.
Create App registration for both chat server and chat client in Azure portal
### Chat server:
- Create a new App registration and enter a name for the application (Example: chat-server). In the Supported account types section, select Accounts in any organizational directory and personal Microsoft accounts (e.g. Skype, Xbox, Outlook.com).
- After it is created, click on Expose an API from the left menu
- Click on set Application ID URI
- Click on Add a scope and enter a name for the scope. Enter relevant text to display to user asking for consent.
- Replace the corresponding values in `DistributedChatServer/appsettings.json`
### Chat client:
- Create a new App registration and enter a name for the application (Example: chat-client). In the Supported account types section, select Accounts in any organizational directory and personal Microsoft accounts (e.g. Skype, Xbox, Outlook.com).
- Click on Redirect URIs and then Add a platform
- Since our chat client is in Angular, click on single-page application
- Enter redirect URI of the application as https://localhost:4200. Once the user is successfully authenticated in Microsoft page, it gets redirected to this URI where our chat client is running
- Add the same URL https://localhost:4200 as Front-channel logout URL
- Select the checkboxes Access tokens and ID tokens
- Replace with corresponding values in `DistributedChatClient/src/app/auth-config.ts`

# How to run
#### Server:
All the required dependencies are added as Nuget packages. <br> 
Build and run `DistributedChatServer.sln` in Visual studio.
#### Client:
- `cd DistributedChatClient`
- `npm install` to install the dependencies
- `npm start`

Open https://localhost:4200 in browser
