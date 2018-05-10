# Solicitar un token de autenticación

## Conceptos generales

El servicio de autenticación debe garantizar que una solicitud se procese, una y sola una vez, para evitar algunas situaciones como pueden ser (solo se mencionan algunas): 

- Procesamiento de  transacciones duplicadas

- Reprocesamiento de solicitudes por problemas en las comunicaciones

- Ataques del tipo [man-in-the-middle (MITM)](https://goo.gl/aB5bKX)

Así las cosas es importante familiarizarse con los siguientes dos conceptos:

---
#### Nonce

Es un número arbitrario que solo se puede utilizar una vez. Se trata de un número (generalmente aleatorio) emitido para garantizar que las comunicaciones antiguas no puedan reutilizarse en ataques de repetición. El algoritmo que lo genera debería asegurar una probabilidad casi nula de repetir un valor. Podría utilizar un identificador único universal como GUID o UUID o el algoritmo de su preferencia. 

[Consulte Wikipedia para más información](https://goo.gl/kY4tu0)

**Ejemplo C#**

```C#
var nonce = Guid.NewGuid().ToString("D");
```

---
<a name="Epoch"></a>
#### Epoch 

Corresponde con el número de segundos (valor positivo) transcurridos desde la medianoche UTC del 1 de enero de 1970 (00:00). Se utiliza como un punto de referencia a partir del cual se mide el tiempo con el fin de omitir ambigüedades, debido a la gran variedad de unidades de tiempo empleadas en sistemas informáticos. 

[Consulte Wikipedia para más información](https://goo.gl/fLCCsq)

**Ejemplo C#**

```C#
// Obtener el número de segundos trancurridos desde el 1 de enero de 1970 (00:00) UTC.
var epoch = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

// Obtener la fecha que representa un número de segundos.
var exp = 1525985330;
DateTimeOffset.FromUnixTimeSeconds(exp);
// Output: 2018-05-10 8:48:50 pm +00:00
```
---

### Creación de un JWT
Es bastante probable que su lenguaje de programación preferido incluya al menos una librería/clase/modulo que facilite la creación y verificación de tokens JWT de extremo a extremo. Para este ejemplo vamos a utilizar el paquete de librerías [Jwt.Net](https://www.nuget.org/packages/JWT) para [.NET Framework](https://en.wikipedia.org/wiki/.NET_Framework).

```C#
// Install-Package JWT
// https://www.nuget.org/packages/JWT

var payload = new Dictionary<string, object>
{
    { "Nonce", Guid.NewGuid().ToString("D") },
    { "Epoch", (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds }
};

var appSecret = "yourAppSecret";

IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
IJsonSerializer serializer = new JsonNetSerializer();
IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

var jwt = encoder.Encode(payload, appSecret);

```

## Cabeceras de autenticación requeridas

El servicio de autenticación requiere una [solicitud HTTP con dos cabeceras personalizadas](https://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html), como se muestra en el siguiente pseudo-código:

```
appKey = Valor generado en la sección Registro de aplicaciones
jwt = Token JWT que incluye las claves Nonce, Epoch en la carga/payload 

{"Nonce":nonce, "Epoch":epoch}

httpHeaders.Add("X-PRO-Auth-App", appKey) 
httpHeaders.Add("X-PRO-Auth-Payload", jwt ) 
```
## Solicitando un token de autenticación

Con estas dos cabeceras es hora de llamar a la operación que genera el token de autenticación para su aplicación:

```C#
// Install-Package RestSharp
// https://www.nuget.org/packages/RestSharp

var client = new RestClient("http://service-endpoint/api");
var request = new RestRequest("/auth/app/signin", Method.POST);
request.AddHeader("X-PRO-Auth-App", appKey);
request.AddHeader("X-PRO-Auth-Payload", jwt);
IRestResponse response = client.Execute(request);

if (response.StatusCode == HttpStatusCode.OK)
{
	var jwtResponse = response.Content;	
}
```

## Procesamiento de la respuesta

La respuesta del API será una cadena de caracteres como esta:

```
eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJqdGkiOiJiOGU0NTExYi1lY2U5LTQ2OWQtOGMzZC00OWI0OTdlZjUzODQiLCJleHAiOjE1MjYwNzU2MDEuMCwiaWF0IjoxNTI1OTg5MjAwLjg5OTcyNzh9.k-2te-7r40E3etwiJJ8hsPlqbe8FISuNeA73Dd38xCQ
```

Este valor es en realidad una cadena JSON codificada con el valor de `appSecret` que obtuvo en la sección [Registro de aplicaciones](App_Register.md). 

Utilizando el lenguaje de programación de su preferencia, convierta este resultado en un objeto que represente una entidad `Token` en su sistema. 

**Ejemplo C#**

```C#
// Install-Package JWT
// https://www.nuget.org/packages/JWT

var jwtResponse  = HttpClient.Post("...");

IJsonSerializer serializer = new JsonNetSerializer();
IDateTimeProvider provider = new UtcDateTimeProvider();
IJwtValidator validator = new JwtValidator(serializer, provider);
IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

var json = decoder.Decode(jwtResponse, appSecret, verify: true);
Console.WriteLine(json);
```
El resultado será una cadena ahora en formato JSON con los siguientes valores:

```json
{"jti":"b8e4511b-ece9-469d-8c3d-49b497ef5384","exp":1526075601,"iat":1525989200}
```
| Nombre  | Descripción |
| --------|-------------
| `jti`   | Token de autenticación |
| `exp`   | Representación de la fecha y hora [Epoch](#Epoch) en que expirará el token |  
| `iat`   | Representación de la fecha y hora [Epoch](#Epoch) en que se emitió el token |

Ahora, almacene el valor del token de autenticación en un lugar seguro y utilícelo en cada llamada subsecuente a las operaciones del API. Cuando el token expire, vuelva a solicitar uno. 

## ¿Cuál es el propósito de la fecha de expiración en el token?

Si puedo solicitar un token en cualquier momento, se podría estar preguntando ¿por qué que expira? 

Si en algún momento considera que la privacidad de la información de su aplicación se ha comprometido, puede solicitar el reinicio de dicha información simplemente generando un nuevo token. Cualquier valor emitido previamente será invalidado, evitando que usted tenga que desplegar una nueva versión de su aplicación.  Considere el token como una llave de acceso con fecha de vencimiento. Si sospecha que alguien tiene la llave, solo necesita cambiar la llave, nunca la cerradura completa. 

<div class="admonition warning">
   <p class="first admonition-title">&iquest;Privacidad comprometida?</p>
   <p class="last">Si alguien diferente a su aplicaci&oacute;n conoce su &lt;appSecret&gt; o &lt;su token de autenticaci&oacute;n&gt;, su informaci&oacute;n se ha visto comprometida
   </p>
</div>
