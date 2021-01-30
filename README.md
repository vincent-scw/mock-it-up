# mock-it-up
Mock It Up is a mock server via configuration files.

## Config the mock server
Mock It Up reads configuration files (called 'Spec') when start.
The Spec is a ```yaml``` file looks like
```yml
version: v1
type: restful
rules:
  - request:
      method: get
      path: api/orders/{id}
    response:
      body: /etc/mockitup.d/payloads/order.json
      delay: 100
  - request:
      method: put
      path: api/orders
    response:
      statusCode: 201
```

You can add ```rules``` to setup inputs ```request``` and expected result ```response```.

```request``` properties includes:
| Property | Required? | Avaliable Options   | Notes                                                                    |
|----------|-----------|---------------------|--------------------------------------------------------------------------|
| method   | required  | get/post/put/delete | Http method.                                                             |
| path     | required  |                     | The Url path template of http request. The template follows [HTML RFC6570](https://tools.ietf.org/html/rfc6570) |
| headers  | optional  |                     | A dictionary of http headers                                             |

```response``` properties includes:
| Property   | Required? | Avaliable Options | Notes                                                            |
|------------|-----------|-------------------|------------------------------------------------------------------|
| statusCode | optional  |                   | The status code of http response. Default: 200                   |
| body       | optional  | string/file path  | The body of http response. It can be direct string or file path. |
| bodyType   | optional  | direct/file/auto  | Default: auto                                                    |
| headers    | optional  |                   | A dictionary of http headers                                     |

## Run test
1. Use ```docker-compose```. It will run both the mock server and tests.
