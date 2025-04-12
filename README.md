# CharacterManager
A .NET-based solution that fetches characters from the Rick and Morty show API and stores them in a SQL database. Includes a console scraper app and a web API for retrieving and adding characters.

Swagger starts on: /swagger/index.html

# Used technologies

.NET 9.0

SQL Server

EF Core 9.0 - ORM for data access

Polly (https://github.com/App-vNext/Polly) - For HTTP policies

AutoMapper (https://automapper.org/) - object-object mapper

Serilog (https://serilog.net/) - logger

MediatR (https://github.com/jbogard/MediatR) - mediator implementation in .NET

# How to run
1) Configure the connection string in appsettings.json for WebApi and Scraper.Runner
2) Using CLI, run the EF command "Update-Database" to create the database
3) Run Scraper.Runner to scrape the data from the Rick and Morty API. The app deletes all the previous data from the database.
4) Run WebApi project. Swagger with API description is available on /swagger/index.html.

# Trade-offs and expansion points
For now app uses memory cache. For the production-ready service, it's advisable to use a distributed cache, like Redis or Memcached.

With the memory cache, it's not possible to remove cache entries by part of the key, that's why when the new character is added via API, the command clears the full cache. With the distributed cache, only relevant cache entries should be removed.

It is supposed that the API service is used by our other internal services, so we trust them to choose the PageSize. Otherwise, we should set it by ourselves in the config. The default page size is 10.
