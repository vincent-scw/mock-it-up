# mock-it-up
***Mock It Up*** is a mock server for testing purpose. It supports both ***static*** and ***dynamic*** stubs consist of http request/response.


```docker 
docker pull edentidus/mockitup
```

## Config the mock server
When start, ***Mock It Up*** reads settings via configuration file. Please ref to [config.yml](https://github.com/vincent-scw/mock-it-up/blob/main/test/MockItUp.IntegrationTest/mockitup.d/conf.yml)
| Property | Required? | Avaliable Options   | Notes                                                                    |
|----------|-----------|---------------------|--------------------------------------------------------------------------|
| host     | optional  |                     | Default: &ast;. (use 'localhost' in Windows for debugging)                   |
| specDirectory | optional  |                | The url path to spec directory (static only) |
| payloadDirectory | optional  |             | The url path to payload (definition of response bodies) directory (static only) |

## Run test
* For ***static*** stub setting, please refer to [Static Stub Wiki](https://github.com/vincent-scw/mock-it-up/wiki/Static-Stub)
* For ***dynamic*** stub setting, please refer to [Dynamic Stub Wiki](https://github.com/vincent-scw/mock-it-up/wiki/Dynamic-Stub)
* Run with docker-compose: ```docker-compose up --abort-on-container-exit --exit-code-from integrationtest```
