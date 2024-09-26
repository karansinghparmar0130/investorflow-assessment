## Contact Management API
A simple Contact Management API for Investment Funds that allows users to perform basic operations such as creating Contacts and assigning them to Funds.

### Features
* Create, read, update, and delete contact records
* Assign and remove contacts to funds
* List contacts for a fund
* API adheres to best practices for exception handling and validation

### Technologies Used
* .NET 8: Backend framework for API development 
* Entity Framework Core: ORM for database interactions 
* SQL Server: Database management
* xUnit + Moq: Unit testing framework
* AutoMapper: Object-Object mapping

### API Specification
[Contact Management API](/assets/Api/Contact-Management-Api.yml)

### Configuration
Configure the API through the appsettings.json file to point to SQL Server.
```{
  "ConnectionStrings": {
    "Default": "Server=your_server;Database=ContactManagementDb;User Id=your_username;Password=your_password;"
  }
}
```

### Db Migration
Initial Database migration can be done either using Entity Framework tools or
[Migration Script](/assets/Migrations/Initial.sql).


### Out of Scope
* Managing Funds
* User Management/Authentication/Authorization
