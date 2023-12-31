## Get up and running
##### Requirement : .NET 7 SDK, MySQL database

- Set the following environment variable. `.env` is supported. 
  - `MARIADB_CONNECTION_STRING`=`Server=yourserver;Port=your_port;User=your_user;Password=your_user;Database=your_db`
  - `JWT_KEY`=`your JWT key`
  - `JWT_ISSUER`=`your JWT issuer`
  - `JWT_AUDIENCE`=`your JWT audience`
  - `STATIC_PATH`=`absolute path; frontend will be served from here`
  - `DATA_PATH`=`absolute path; uploaded files will be served from here`
- Run `dotnet run --project int3306.Api`
  - Ensure port 5000 is available.