# mock-it-up
***Mock It Up*** is a mock server for testing purpose. It supports both ***static*** and ***dynamic*** stubs consist of http request/response.


```docker 
docker pull edentidus/mockitup
```

## Motivation
In microservice architecture, a serviec might denpendent to multiple other services by Restful Http requests. When doing acceptence/integration test, it is very annoying to connect to real downstream services. Trying to decoupe the dependencies, ***Mock It Up*** is a mock server to replace the real services.

![img](/design.png)

## Config the mock server
When start, ***Mock It Up*** reads settings via configuration file. Please ref to [config.yml](https://github.com/vincent-scw/mock-it-up/blob/main/test/MockItUp.IntegrationTest/mockitup.d/conf.yml)
| Property | Required? | Notes                                                                    |
|----------|-----------|--------------------------------------------------------------------------|
| host     | optional  | Default: &ast;. (use 'localhost' in Windows for debugging)               |
| controlPort | required for dynamic |Default: 30000 (dynamic only)                                            |
| specDirectory | required for static  | The url path to spec directory (static only)                             |
| payloadDirectory | optional  | The url path to payload (definition of response bodies) directory (static only) |

## Prepare for testing
* For ***static*** stub setting, please refer to [Static Stub Wiki](https://github.com/vincent-scw/mock-it-up/wiki/Static-Stub)
* For ***dynamic*** stub setting, please refer to [Dynamic Stub Wiki](https://github.com/vincent-scw/mock-it-up/wiki/Dynamic-Stub)
* Both ***dynamic*** and ***static*** stubs can work together. Mock It Up try to match the dynamic stub, then static.
* Before running docker containers, don't forget to copy your specs and payloads into container if you want to use ***static*** stubs.
* Idealy, running acceptence/integration test requires three containers -- Service, Tests and Mock Server-- working together. 
  * Run with docker-compose: ```docker-compose up --abort-on-container-exit --exit-code-from integrationtest```
