## **Clean Architecture**

The code follows Clean Architecture recommended here:
https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture

### Vintri.Beers.Api: 
This is top layer project should only include controllers, filters, attributes, startup configs etc. Ideally controller action method should be "thin" without too much business logic, all business logic should be included in the "service" classes from Vintri.Beers.Core project.
All public members should be properly xml commented as its xml docs will be used by Swagger. 

### Vintri.Beers.Core:
This is core project should include all business logic classes, interfaces, models, validators etc. There's no dependencies from other projects. 

The "Services" folder should have all business logic classes which should be named as "xxxService",
and it should be only used by controllers as "orchestrator" to call validator for validation, api client for api calls, repository for data access etc. 

The "Validators" folder should have all validation classes with [fluent validation](https://docs.fluentvalidation.net/).  

### Vintri.Beers.Infrastructure:

This project should have API clients, data access repositories, and any other infrastructure-specific services 


## **Unit Tests**

Keep separate unit tests project for each tested project with naming convention: {tested project name}.Tests, keep the same folder structure as tested project and keep test class name as: {tested class name}Tests.cs.
It uses test framework: [xUnit](https://xunit.net/), mocking: [NSubstitute](https://nsubstitute.github.io/), assertion: [Shouldly](https://github.com/shouldly/shouldly), test data: [AutoFixture](https://autofixture.github.io/)


## **Logging**
Its just simple file logging with Serilog to log every request, response and any uncaught exceptions, and it could be used for any layers like controller actions, services to log traces etc.

## **Get Started**
To run the web api locally, ensure .net framework 4.8 and IIS Express installed. Run http://localhost:5000/swagger locally to get Swagger UI for local test, and 
use http://localhost:5000/swagger.json to explore API.


## **Best Practice**

1. All sync calls follow with ConfigureAwait(false) to avoid possible deadlock issues. https://blog.stephencleary.com/2012/02/async-and-await.html#avoiding-context
2. Try OptionsPattern to access configuration settings: https://learn.microsoft.com/en-us/dotnet/core/extensions/options 
3. Use HttpClient for any external API calls and ensure it handles transient issues properly with Polly.
4. Use ./infrastructure folder for any Infrastructure as Code files, like sql database dacpac, Azure ARM template, Terraform scripts etc. The file vintri-beers-api.yaml/vintri-beers-api.json saved here for API specs.
5. Use ./pipeline folder for any CI/CD files, like how to build project, unit test etc.
6. Create web config file for each environment to transform any required app settings properly.
7. Coding conventions: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names, https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions