![CI/CD](https://github.com/vincent-scw/mock-it-up/actions/workflows/docker-image.yml/badge.svg)

# mock-it-up
***Mock It Up*** is a mock server for testing purpose. It supports both ***static*** and ***dynamic*** stubs consist of http request/response.

## Install
* Install server
```docker 
docker pull edentidus/mockitup
docker run -p 5000:5000 -p 30000:30000 edentidus/mockitup
```
* Install client (C#)
```
Install-Package MockItUp.Client
```

## Motivation
In microservice architecture, a serviec might denpendent to multiple other services by Restful Http requests. When doing acceptence/integration test, it is very annoying to connect to real downstream services. Trying to decoupe the dependencies, ***Mock It Up*** is a mock server to replace the real services.

![img](/design.png)

## Prepare for testing
* ***Static*** stub should be registered as spec files before server started. 
  ```yml
  version: v1
  stubs:
    - request:
        method: get
        path: api/orders/{id}
      response:
        body: order.json
        delay: 100
    - request:
        method: put
        path: api/orders
      response:
        statusCode: 201
        body: |
          {
              "id": 1000,
              "customerId": "${b.customer.id}"
          }
  ```
  For details, please refer to [Static Stub Wiki](https://github.com/vincent-scw/mock-it-up/wiki/Static-Stub).
* ***Dynamic*** stub should be registered within a test scenario. It will be removed automatically when scenario disposed.
  ```csharp
  using (var scenario = _client.BeginScenario())
  {
      var orderId = 215;
      // Register a dynamic stub to mock server
      var regResult = scenario.RegisterDynamicStub(stub =>
          stub.WhenRequest("GET", "api/orders/{id}")
              .RespondWith(JsonConvert.SerializeObject(new
              {
                  id = orderId,
                  title = "this is a test"
              }))
      );

      // Do your testing against real service
      var response = await httpClient.GetAsync($"{_orderUrl}/api/orders/{orderId}");
      var order = await ReadResponseAsync<dynamic>(response.Content);

      Assert.Equal(orderId, (int)order.id);
      Assert.Equal("this is a test", (string)order.title);

      // The stubs in current scenario will be removed in dispose

      // You can also manually remove any stubs within current scenario by
      // scenario.RemoveDynamicStubs(regResult.StubID);
  }
  ```
  For details, please refer to [Dynamic Stub Wiki](https://github.com/vincent-scw/mock-it-up/wiki/Dynamic-Stub). 
* Both ***dynamic*** and ***static*** stubs can work together. Mock It Up try to match the dynamic stub, then static. Please run tests synchronously to aviod collision.
* Before running docker containers, don't forget to copy your specs and payloads into container if you want to use ***static*** stubs.
* Idealy, running acceptence/integration test requires three containers -- Service, Tests and Mock Server-- working together. 
  * Run with docker-compose: ```docker-compose up --abort-on-container-exit --exit-code-from integrationtest```.
  * Run in K8S: Include ***Mock It Up*** as a sidecar together with api service.
* For more URL match examples, please refer to [Tests](https://github.com/vincent-scw/mock-it-up/blob/development/test/MockItUp.UnitTest/Core/StubItemTest.cs).
  
## Config the mock server
When start, ***Mock It Up*** reads settings via configuration file. Please ref to [config.yml](https://github.com/vincent-scw/mock-it-up/blob/main/test/MockItUp.IntegrationTest/mockitup.d/conf.yml)
| Property | Required? | Default Value |Notes                                                                    |
|----------|-----------|---------------|-----------------------------------------------------------|
| host     | required  | &ast;         | Use 'localhost' in Windows for debugging                |
| services | required  | { *: 5000 }   | Add services by key:value (name:port) pair               |
| controlPort | required for dynamic | 30000 | dynamic only                                        |
| specDirectory | required for static | /etc/mockitup.d/specs/ | The url path to spec directory (static only)   |
| payloadDirectory | optional | /etc/mockitup.d/payloads/ | The url path to payload (definition of response bodies) directory (static only) |
