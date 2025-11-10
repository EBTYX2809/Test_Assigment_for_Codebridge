<h1 align="center">Test Assigment for Codebridge - ASP.NET Web API project with dogs data base.</h1>

#### What has been done:
- Data base layer with configuration and Dog model.
- Service layer that allow Create and Read operations.
- Querying dogs with filtering and pagination.
- Ping Controller for checking API version and Dog Controller for main requests.
- Tests for all basic logic.
- All methods are asynchronous.
- Validation for Dog model and QueryDTO.
- Rate limitator for API.
- Global Exceptions Handler.

<h3>How to test:</h3>
Copy repo:

`git clone https://github.com/EBTYX2809/Test_Assigment_for_Codebridge`

Go to project:

`cd Test_Assigment_for_Codebridge`

Build and run application:

`dotnet restore`

`dotnet build`

`dotnet run`

Run tests:

`cd ../Tests`

`dotnet restore`

`dotnet build`

`dotnet test`

---

#### Future improvemetes:
- If will need to store some important information in appsettings.json it should be modified to .Development. version and will hiden from public.
- If new complex models with inner references will be created should to add DTO objects with mapping for avoid problems with serialization to JSON.
- For better working with errors must to add logging. 
- For improvement security JWT mechanism could be implemented.
- Dogs should be cached when amount of data will increased.

## Please note also my another pet project, which also is ASP.NET Web API application but much bigger and will say you more about me as Backend developer: [Check](https://github.com/EBTYX2809/Finance_Manager)
