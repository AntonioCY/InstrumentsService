### Purpose of the Instruments Service

Instruments Service is intended to get the information about the instruments and prices from the external data sources.

## Technologies
- ASP.NET Core 8.0 WebApi
- REST Standards
- WebSockets
- XUnit
- Serilog

- ## Features
- Swagger UI
- API Versioning

Possible TODO:
- Access token validation for the endpoints when the Authentication service would be ready
- Healthchecks
- Jaeger for the tracing

#### Local setup
- The service could be instantinated with the pre-defined settings: run "docker compose up -d" in the root folder of the solution
- Configuration: 
	- appsettings.json config should already have proper settings for the Tiingo, the only value is need to be added is "token" for the Tiingo API, 
	  which could be obtained from the email or you can get your own on the https://api.tiingo.com/
	- do not forget to add the token value to the appsettings.Test.json in the InstrumentsService.Application.Tests project
- In postman folder you can find the tiny collection to test interactions with the third party services, i.e. Tiingo in the current case, it could be extended with Binance, etc..
 
## Getting Started
- Make InstrumentsService.Api as a Startup project
- Run (Or just navigate to the InstrumentsService.Api folder and run "dotnet run" in the console)
- Open the Swagger UI in the browser: http://localhost:5132/swagger/index.html and check the API documentation, try to get the instruments, providers and prices
- Go to the WebSocket client project InstrumentsService.WS.Client folder and run "dotnet run" in the console to subscribe gettings instruments details, 
  you would see the feed from the Tiingo as a default data provider
- Run the tests in the InstrumentsService.Application.Tests project

## Quick notes

This is the test solution for the Instruments Service. 
The service is intended to get the information about the instruments and prices from the external data sources. 
The service is implemented as a REST API that has a Swagger UI for the API documentation and WebSocket server for broadcasting the symbol details across the subscribers 
(it can easily support 1000+ subscriberts), plus subscription on the Tiingo WebSockets stream as a default data provider.
In addition to that, the service has a versioning mechanism for the API.
Binance data provider is also implemented, but it is not used in the current version of the service cause require detailed testing. 
As an improvement, it could be added as a separate part of the ws subscription mechanism.
Also, in the current solution it is used pre-defined available symbols "EURUSD", "USDJPY", "BTCUSD", as an improvement there could be retrieved the full list of available symbols from the data provider.

PS: The service is not intended to be used in the production environment, it is just a test solution.

Instruments API endpoint:
![instruments_api_endpoint](https://github.com/user-attachments/assets/2f7f9650-3f50-4f1b-8633-fc9512c96efa)

Providers API endpoint:
![providers_api_endpoint](https://github.com/user-attachments/assets/0c56486d-bd21-41a4-b337-497f7dfeb551)

Instrument price API endpoint:
![instrument_price_api_endpoint](https://github.com/user-attachments/assets/9b9147a8-5552-4ffe-b816-0c1b5a0522ca)

Instruments prices web sockets feed:
![intruments_prices_ws_client_feed](https://github.com/user-attachments/assets/d6c8624c-6d28-4824-a467-01c342d94ab8)
