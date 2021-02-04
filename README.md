# mock-it-up
***Mock It Up*** is a mock server via configuration files.


```docker 
docker pull edentidus/mockitup
```

## Config the mock server
When start, ***Mock It Up*** reads settings via configuration file. Please ref to [config.yml](https://github.com/vincent-scw/mock-it-up/blob/main/test/MockItUp.IntegrationTest/mockitup.d/conf.yml)
| Property | Required? | Avaliable Options   | Notes                                                                    |
|----------|-----------|---------------------|--------------------------------------------------------------------------|
| host     | optional  |                     | Default: &ast;. (use 'localhost' in Windows for debugging)                   |
| specDirectory | required  |                | The url path to spec directory |
| payloadDirectory | optional  |             | The url path to payload (definition of response bodies) directory |

***Mock It Up*** reads **Spec** to setup rules for *request* and expected *response*.
The **Spec** is a ```yaml``` file looks like
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
      body: |
        {
            "id": 1000,
            "customerId": "${b.customer.id}"
        }
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

Rather than static response body, you can also use ```variable```. 
| Object     | VariableName      | Request Example         | Usage                   | Replaced with                       |
|------------|-------------------|-------------------------|-------------------------|-------------------------------------|
| url path   | p/path            | url ```http://localhost/api/values/1000``` |${p.id}, ${path.id}| 1000            |
| request body | b/body          | body ```{ "customer": {"id": "C100", name: "somebody" }}``` | ${b.customer.id} | C100        |
| request header | h/header/headers | header ```Accept:application/json``` | ${h.accept} | application/json |

Please reference to [test](https://github.com/vincent-scw/mock-it-up/blob/main/test/MockItUp.IntegrationTest/mockitup.d) for some examples.

## Run test
* docker-compose: ```docker-compose up --abort-on-container-exit --exit-code-from integrationtest```
