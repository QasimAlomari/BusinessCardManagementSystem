README - How to Run the Project
===============================

Project: Business Card Management System 
Technology: .NET 8, ASP.NET Core Web API, Dapper, SQL Server, Jquery+Ajax

---

1. Prerequisites
----------------
- .NET 8 SDK
- SQL Server 2022
- Visual Studio 2022

---

2. Configuration
----------------
Update the connection string in:
BusinessCardManagementSystem/WebApi/appsettings.json

Example:
"AllowedHosts": "*",
"ConnectionStrings": {
  "SqlConn": "Data Source=;Initial Catalog=BusinessCardManagement;User ID=sa;Password=;TrustServerCertificate=True"
}

Data Source= "ServerName"
Password="the Password For Server"
---

3. Running the App
------------------
Using Visual Studio:
- Set BusinessCardManagementSystem.WebAPI as Startup Project
- Press CTRL+F5 to run The API
- Set BusinessCardManagementSystem.WebPortal as Startup Project
- Press F5 to run The System

---

4. API Access
-------------
- Swagger UI: https://localhost:51936/swagger  
- Use Postman or Swagger to test endpoints
  Note: the link of api (BusinessCardManagementSystem.WebAPI/Properties/launchSettings.json) 

"$schema": "http://json.schemastore.org/launchsettings.json",
"iisSettings": {
  "windowsAuthentication": false,
  "anonymousAuthentication": true,
  "iisExpress": {
    "applicationUrl": "http://localhost:51936",
    "sslPort": 0
  }
}
and copy "applicationUrl": "http://localhost:51936", and put it in the web portal to call api
in the BusinessCardManagementSystem.WebPortal/appsettings.json

"AllowedHosts": "*",
"Setting": {
  "UrlApi": "http://localhost:51936"
}

---

5. Notes
--------
- Bulk upload via Excel is supported large excel file using sqlbulk
---

Author: Qasim Alomari  
GitHub: https://github.com/QasimAlomari  
