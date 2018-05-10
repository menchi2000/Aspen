# Pasos para crear un JWT 

## Creación del encabezado (haeder)

El encabezado es un objeto JSON en el siguiente formato. Su información se utilizará más adelante para determinar cómo calcular la firma del JWT.

```json
{"typ":"JWT"," alg":"HS256"}
```

En este JSON, el valor de la clave `typ` especifica que el objeto es un JWT, y el valor de la clave `alg` especifica el algoritmo hash utilizado para crear el componente de la firma JWT. Utilizaremos el algoritmo HMAC-SHA256 (un algoritmo de hash que usa una clave secreta (`appSecret`), para calcular la firma como los veremos de manera detallada a continuación).

## Creación de la carga (payload)

El componente de carga en un JWT es la información que está almacenada dentro del JWT (estos datos también se conocen como los `reclamos/claims` del JWT). En esta carga se pueden poner tantas reclamaciones/claims como se desee, es decir, que se pueden agregar tantas claves al JSON como se necesite. 
Tenga presente que existen algunas claves definidas en el estándar como `iss`, `sub` o `exp`. Estas claves pueden ser útiles al crear un JWT, pero son opcionales. Consulte la página de [Wikipedia](https://en.wikipedia.org/wiki/JSON_Web_Token) para obtener una lista completa de los campos estándar de un JWT.

En nuestro ejemplo, el servidor de autenticación creará un JWT con dos valores: El token de autenticación emitido para una aplicación y la fecha de vencimiento del mismo. 

```json
{"jti":"f963bc5b-1a64-4478-b98a-a46872d4f66a", "exp":1525966583}
```

| Clave | Descripción |
| ------------- |-------------
| `jti` | Token de autenticación emitido para la aplicación. En adelante, todas las solicitudes enviadas al API deben contener este valor en la cabecera de autenticación. Más adelante lo veremos con detalle |
| `exp` | Fecha vencimiento del token de autenticación. Se trata del número de segundos transcurridos desde la medianoche UTC del 1 de enero de 1970 (00:00). Es el valor conocido universalmente como [tiempo Unix](https://es.wikipedia.org/wiki/Tiempo_Unix) |

## Creación de la firma (signature)

La firma se calcula utilizando el pseudo-código:

```
data = base64urlEncode (header) + "." + base64urlEncode (payload)
signature = Hash (data, appSecret)
```
Lo que hace este algoritmo es codificar en [base64url](https://en.wikipedia.org/wiki/Base64#RFC_4648) el encabezado y la carga (Payload) creada en los pasos anteriores. El algoritmo luego concatena las cadenas codificadas resultantes con un punto (.) En el pseudo-código, esta cadena unida se asigna a "data". Para obtener la firma del JWT, se obtiene el Hash de la cadena en "data" utilizando la clave secreta y el algoritmo hash especificado en el encabezado del JWT.

En este ejemplo, tanto el encabezado como la carga son codificados en base64url así:

```
// header
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9
// payload 
eyJqdGkiOiJmOTYzYmM1Yi0xYTY0LTQ0NzgtYjk4YS1hNDY4NzJkNGY2NmEiLCJleHAiOjE1MjU5NjY1ODN9
```
Luego, utilizando los valores codificados del encabezado y la carga, y aplicando el algoritmo de firma especificado [HMAC-SHA256](https://en.wikipedia.org/wiki/HMAC) en la cadena de datos con la clave secreta establecida como la cadena `miclavesecreta`, obtendríamos la firma JWT:

```
// Token JWT
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJmOTYzYmM1Yi0xYTY0LTQ0NzgtYjk4YS1hNDY4NzJkNGY2NmEiLCJleHAiOjE1MjU5NjY1ODN9.ZHjwy3Lt9IT_CTMVrLVjNISYyMmWkZ30Wa-aTnOJfnw
```

En la página [jwt.io](https://jwt.io) puede intentar crear su propio JWT a través del navegador, sin necesidad de escribir programas, además de facilitar la compresión de estos conceptos. 

Volviendo a nuestro ejemplo, el servidor de autenticación (nuestra API) ahora puede retornar este JWT al cliente (su aplicación), para que extraiga el valor de la clave `jti` (token de autenticación) y lo envíe al API en las subsecuentes llamadas, siempre que no haya expirado. 

> En adelante debería reemplazar el valor de `miclavesecreta` con el valor de `appSecret` que obtuvo en la sección [Registro de aplicaciones](App_Register.md)

## ¿Cómo determinar si el token expiró?

Extraiga el valor de la clave `exp` (tiempo Unix) y conviértalo a formato de fecha y hora con el lenguaje de programación de su predilección. Recuerde que el valor es **UTC** y no GTM. 

Ejemplo C#

```c#
var exp = 1525985330;
DateTimeOffset.FromUnixTimeSeconds(exp);
// Output: 2018-05-10 8:48:50 pm +00:00
```

Es importante entender que el propósito de utilizar JWT no es ocultar los datos. 
El motivo por el que se utiliza JWT es para demostrar que los datos enviados y recibidos fueron creados por una fuente auténtica reconocida. Los datos de la firma permiten verificar al receptor (su aplicación y nuestra API) la autenticidad de la fuente de los datos.
