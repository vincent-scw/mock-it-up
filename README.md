# mock-it-up
***Mock It Up*** is a mock server via configuration files.

## Config the mock server
When start, ***Mock It Up*** reads settings via configuration file. Please ref to [config.yml](https://github.com/vincent-scw/mock-it-up/blob/main/test/MockItUp.IntegrationTest/mockitup.d/conf.yml)
| Property | Required? | Avaliable Options   | Notes                                                                    |
|----------|-----------|---------------------|--------------------------------------------------------------------------|
| host     | optional  |                     | Default: &ast;. (use 'localhost' in Windows for debugging)                   |
| specDirectory | required  |                | The url path to spec directory |
| payloadDirectory | optional  |             | The url path to payload (definition of response bodies) directory |

Mock It Up reads **Spec** to setup rules for *request* and expected *response*.
The Spec is a ```yaml``` file looks like
```yml
version: v1
type: restful
rules:
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
```

```rules``` should include *request* (condition) and *response* (expectation).

```request``` properties includes:
| Property | Required? | Avaliable Options   | Notes                                                                    |
|----------|-----------|---------------------|--------------------------------------------------------------------------|
| method   | required  | get/post/put/delete | Http method.                                                             |
| path     | required  |                     | The url path template of http request. The template follows [HTML RFC6570](https://tools.ietf.org/html/rfc6570) |
| headers  | optional  |                     | A dictionary of http headers                                             |

```response``` properties includes:
| Property   | Required? | Avaliable Options | Notes                                                            |
|------------|-----------|-------------------|------------------------------------------------------------------|
| statusCode | optional  |                   | The status code of http response. Default: 200                   |
| body       | optional  | string/file path  | The body of http response. It can be direct string or file path. |
| bodyType   | optional  | direct/file/auto  | Default: auto                                                    |
| headers    | optional  |                   | A dictionary of http headers                                     |

## Run test
* Use ```docker-compose```. It will run both mock server and tests.
