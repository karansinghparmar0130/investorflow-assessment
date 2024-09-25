## Contact Management API
A simple Contact Management API for Investment Funds that allows users to perform basic operations such as creating Contacts and assigning them to Funds.
This solution is built using .Net 8 and integrates with Microsoft SQL Server using Entuty Framewoerk.

### Features
* Create, read, update, and delete contact records
* Assign contacts to funds
* API adheres to best practices for exception handling and validation

### Technologies Used
* .NET 8: Backend framework for API development 
* Entity Framework Core: ORM for database interactions 
* SQL Server: Database management
* xUnit + Moq: Unit testing framework.
* AutoMapper: Object-Object mapping

### API Specification
[Contact Management API](/documentation/Api/contact-management-api.yaml)

### Configuration
You can configure the API through the appsettings.json file
```{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=ContactManagementDb;User Id=your_username;Password=your_password;"
  }
}
```

### Out of Scope
* Managing Funds
* User Management/Authentication/Authorization
