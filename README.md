[![Build Status](https://travis-ci.org/vincent-scw/mock-it-up.svg?branch=main)](https://travis-ci.org/vincent-scw/mock-it-up)

# mock-it-up
***Mock It Up*** is a mock server for testing purpose. It supports both ***static*** and ***dynamic*** stubs consist of http request/response.


```docker 
docker pull edentidus/mockitup
```

## Motivation
In microservice architecture, a serviec might denpendent to multiple other services by Restful Http requests. When doing acceptence/integration test, it is very annoying to connect to real downstream services. Trying to decoupe the dependencies, ***Mock It Up*** is a mock server to replace the real services.

![img](/design.png)

## Prepare for testing
* ***Static*** stub should be registered before server started by spec files. 
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
      var regResult = scenario.RegisterDynamicStub(new DynamicStub
      {
          // Define request
          Request = new Request { Method = "GET", UriTemplate = "api/orders/{id}" },
          // Define response
          Response = new Response
          {
              StatusCode = 200,
              Body = JsonConvert.SerializeObject(new
              {
                  id = orderId,
                  title = "this is a test"
              })
           }
      });

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
* Both ***dynamic*** and ***static*** stubs can work together. Mock It Up try to match the dynamic stub, then static.
* Before running docker containers, don't forget to copy your specs and payloads into container if you want to use ***static*** stubs.
* Idealy, running acceptence/integration test requires three containers -- Service, Tests and Mock Server-- working together. 
  * Run with docker-compose: ```docker-compose up --abort-on-container-exit --exit-code-from integrationtest```.
  * Run in K8S: Include ***Mock It Up*** as a sidecar together with api service.
  
## Config the mock server
When start, ***Mock It Up*** reads settings via configuration file. Please ref to [config.yml](https://github.com/vincent-scw/mock-it-up/blob/main/test/MockItUp.IntegrationTest/mockitup.d/conf.yml)
| Property | Required? | Notes                                                                    |
|----------|-----------|--------------------------------------------------------------------------|
| host     | required  | Default: &ast;. (use 'localhost' in Windows for debugging)               |
| controlPort | required for dynamic |Default: 30000 (dynamic only)                                            |
| specDirectory | required for static  | The url path to spec directory (static only)                             |
| payloadDirectory | optional  | The url path to payload (definition of response bodies) directory (static only) |
