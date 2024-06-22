﻿# HADotNet

[![Nuget](https://img.shields.io/nuget/v/Amadeo.HADotNet.Core?logo=nuget)](https://www.nuget.org/packages/Amadeo.HADotNet.Core/)

A simple, straightforward .NET Standard library for the [Home Assistant](https://github.com/home-assistant/home-assistant) API.

## Fork

This repository is the fork of original one by [qJake](https://github.com/qJake/HADotNet).

Aim of the fork is to provide maintenance & feature updates as part of [HASS.Agent](https://github.com/hass-agent) development.

## Features

* .NET 8 cross-platform library
* DI-friendly client initialization (suitable for ASP.NET Core)
* Home Assistant data is represented by strongly-typed, commented model classes

### Supported Home Assistant APIs

* Root API (*Verifies the HA API is responding*)
* Automation API
* Google Calendar API (*Unofficial*)
* Discovery API
* Config API
* Camera Proxy API
* Entity API
* Error Log API
* Events API
* History API
* Logbook API
* Services API
* States API
* Supervisor API (Supervisor-based installations only)
* Template API

## Getting Started

### From NuGet (Recommended)

[![Nuget](https://img.shields.io/nuget/dt/HADotNet.Core?color=%23004880&label=NuGet%20Downloads&logo=nuget)](https://www.nuget.org/packages/HADotNet.Core/)

Install **[Amadeo.HADotNet.Core](https://www.nuget.org/packages/Amadeo.HADotNet.Core/)** from NuGet:

`Install-Package Amadeo.HADotNet.Core`

### From Source

Clone this repo and either include the `Amadeo.HADotNet.Core` library in your project, 
or build the project and include the DLL as a reference.

## Examples

### Initializing The Client Factory

The `ClientFactory` class is reponsible for initializing all other clients in a 
reusable way, so you only have to define your instance URL and API key once.

To initialize the `ClientFactory`, pass in your base Home Assistant URL and a
long-lived access token that you created on your profile page.

```csharp
ClientFactory.Initialize("https://my-home-assistant-url/", "AbCdEf0123456789...");
```

### Integrating into an ASP.NET Core Site

First, call `ClientFactory.Initialize(...);` to ensure that your Home Assistant
instance is connected. You can do this in `ConfigureServices`, or using middleware.

Just be sure that the client factory has been initialized **before** any page/controller
requests are made.

You can add individual clients to the DI container like so:

```csharp
services.AddScoped(_ => ClientFactory.GetClient<EntityClient>());
services.AddScoped(_ => ClientFactory.GetClient<StatesClient>());
services.AddScoped(_ => ClientFactory.GetClient<ServiceClient>());
services.AddScoped(_ => ClientFactory.GetClient<DiscoveryClient>());
```

Since there are no interfaces, simply add the instance(s) directly into the container.
Add as few or as many clients as you need, and then simply retrieve each client via the
DI container as-needed.

```csharp
public class MyController : Controller
{
	private IOtherService SampleService { get; }
	private EntityClient EntityClient { get; }

	public MyController(IOtherService sampleService, EntityClient entityClient)
	{
		SampleService = sampleService;
		EntityClient = entityClient;
	}

	// ...
}
```

### Getting Home Assistant's Current Configuration

Get a `ConfigClient` and then call `GetConfiguration()`:

```csharp
var configClient = ClientFactory.GetClient<ConfigClient>();
var config = await configClient.GetConfiguration();
// config.LocationName: "Home"
// config.Version: 0.96.1
```

### Retrieving All Entities

Get an `EntityClient` and then call `GetEntities()`:

```csharp
var entityClient = ClientFactory.GetClient<EntityClient>();
var entityList = await entityClient.GetEntities();
```

### Retrieving Entities By Domain

Get an `EntityClient` and then call `GetEntities("domainName")`:

```csharp
var entityClient = ClientFactory.GetClient<EntityClient>();
var filteredEntityList = await entityClient.GetEntities("light");
```

### Retrieving All Entity States

Get a `StatesClient` and then call `GetStates()`:

```csharp
var statesClient = ClientFactory.GetClient<StatesClient>();
var allMyStates = await statesClient.GetStates();
```

### Retrieving State By Entity

Get a `StatesClient` and then call `GetState(entity)`:

```csharp
var statesClient = ClientFactory.GetClient<StatesClient>();
var state = await statesClient.GetState("sun.sun");
// state.EntityId: "sun.sun"
// state.State: "below_horizon"
```

### Retrieving All Service Definitions

Get a `ServiceClient` and then call `GetServices()`:

```csharp
var serviceClient = ClientFactory.GetClient<ServiceClient>();
var services = await serviceClient.GetServices();
```

### Retrieving Calendar Events

Get a `CalendarClient` and then call `GetEvents(calEntity)`:

```csharp
var calClient = ClientFactory.GetClient<CalendarClient>();
var myEvents = await calClient.GetEvents("calendar.my_calendar");
```

### Calling a Service

Get a `ServiceClient` and then call `CallService()`:

```csharp
var serviceClient = ClientFactory.GetClient<ServiceClient>();

var resultingState = await serviceClient.CallService("homeassistant", "restart");
// Or
var resultingState = await serviceClient.CallService("light", "turn_on", new { entity_id = "light.my_light" });
// Or
var resultingState = await serviceClient.CallService("light.turn_on", new { entity_id = "light.my_light" });
// Or
var resultingState = await serviceClient.CallService("light.turn_on", @"{""entity_id"":""light.my_light""}");
```

### Retrieving History for an Entity

Get a `HistoryClient` and then call `GetHistory(entityId)`:

```csharp
var historyClient = ClientFactory.GetClient<HistoryClient>();
var historyList = await historyClient.GetHistory("sun.sun");

// historyList.EntityId: "sun.sun"
// historyList[0].State: "above_horizon"
// historyList[0].LastUpdated: 2019-07-25 07:25:00
// historyList[1].State: "below_horizon"
// historyList[1].LastUpdated: 2019-07-25 20:06:00
```

### Rendering a Template

Get a `TemplateClient` and then call `RenderTemplate(templateBody)`:

```csharp
var templateClient = ClientFactory.GetClient<TemplateClient>();
var myRenderedTemplate = await templateClient.RenderTemplate("The sun is {{ states('sun.sun') }}");

// myRenderedTemplate: The sun is above_horizon
```

### Retrieving a Camera Image

Get a `CameraProxyClient` and then call `GetCameraImage`, or for a ready-to-go HTML Base64 string, `GetCameraImageAsBase64`:

```csharp
var cameraClient = ClientFactory.GetClient<CameraProxyClient>();
var myCameraImage = await cameraClient.GetCameraImageAsBase64("camera.my_camera");

// myCameraImage: "data:image/jpg;base64,AAAAAAAAA..."
```

## Testing

To run the unit tests, you must first set two environment variables:

* `HADotNet:Tests:Instance` = `https://my-home-assistant-url/`
* `HADotNet:Tests:ApiKey` = `AbCdEf0123456789...`
