# Mensajes de respuesta

**Aspen**, luego de recibir, interpretar y procesar un mensaje de solicitud, genera un mensaje de respuesta HTTP

El elemento `StatusCode` en la respuesta, es el código de resultado de la operación, que consiste en un grupo de 3 dígitos con el resultado de la solicitud.
Los códigos generados por **Aspen** acogen la definición del protocolo HTTP definido en el [RFC 2616](https://www.ietf.org/rfc/rfc2616.txt).
El valor del campo `StatusCode` intenta proporcionar una descripción textual del resultado de la operación dentro de un dominio de valores.
`StatusCode` se puede utilizar en procesos de automatización,  mientras que `ReasonPhrase` intenta proporcionar un mensaje descriptivo enfocado en el razonamiento humano.

El primer dígito en `StatusCode` define la clase de respuesta. Los últimos dos dígitos no tienen ninguna función de categorización. Hay 5 posibles valores para el primer dígito:

Grupo | Descripción | Acción necesaria
:---: | ----------- | -------------
2xx | Éxito | La acción se recibió y se procesó de forma exitosa. No se requiere ninguna acción adicional.
3xx | Redirección |  Se deben tomar medidas adicionales para completar la solicitud. El recurso (Url) solicitado se ha movido a otro lugar. Se debe redirigir la solicitud a la nueva Url.
4xx | Error de cliente | La solicitud contiene datos inválidos o faltan datos. Los datos enviados por el cliente no permiten procesar la solicitud. Se deben corregir los datos de la solicitud y vlvel a intentar.
5xx | Error de servidor | El servidor no pudo procesar la solicitud. Se requieren correcciones del lado del servidor.

Las aplicaciones cliente no tienen que comprender el significado de todos los códigos de estado `StatusCode`, aunque tal comprensión es deseable.
Sin embargo, las aplicaciones **DEBEN** procesar al menos el primer dígito de `StatusCode`, y tratar cualquier respuesta no reconocida como equivalente al código de estado x00 de ese dominio.
Por ejemplo, si el cliente recibe un código de `StatusCode` 431, puede asumir con seguridad que hubo un problema con su solicitud y tratar la respuesta como si hubiera recibido un `StatusCode` 400.

## Campos de encabezado personalizado en la respuesta

**Aspen** incluye en cada respuesta dos campos personalizados así:

Key | Value
--- | -----
| **X\-PRO\-Response\-Help** | Contiene la Url donde se describe la razón por la que no se pudo procesar una solicitud. Se incluye en la respuesta para cualquier solicitud con un status diferente a 2xx.
| **X\-PRO\-Response\-Time** | Contiene una cadena en formato minutos:segundos:milisegundos (mm:ss:fff) con el tiempo que tomó procesar la solicitud. Por supuesto, no se incluye el tiempo de latencia.

## Mensaje de repuesta comunes

[Un código de respuesta HTTP](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes) es generado por **Aspen** en respuesta a una solicitud del cliente hecha al servidor.

## Identificadores en mensajes de respuesta

La cabecera personalizada de respuesta `X-PRO-Response-Help` puede contener alguno de los valores mencionados en el siguiente sección. `ReasonPhrase` puede darle pistas del porque **Aspen** está generando esa respuesta.

## AppKeyDoesNotExist

- StatusCode: 400
- ReasonPhrase: Necesita utilizar el identificador de aplicación `AppKey` suministrado.
- EventId: 20005

## AppKeyIsDisabled

- StatusCode: 403
- ReasonPhrase: El identificador de aplicación `AppKey` suministrado no está habilitado en el sistema.
- EventId: 20006

## BadGateway

- StatusCode: 502
- ReasonPhrase: **Aspen** no ha podido redirigir la solicitud al sistema responsable de procesarla. Pónganse en contacto con nuestro equipo de monitoreo.
- EventId: 15859

## BadRequest

- StatusCode: 400
- ReasonPhrase: La solicitud no pudo ser procesada por el servidor. Los datos enviados por el cliente no son válidos. El campo `ReasonPhrase` contiene un mensaje que describe de forma detallada los datos que no pudieron ser procesados.
- EventId: 20005

## DecodeAuthHeaderFailed

- StatusCode: 400
- ReasonPhrase: La información suministrada en el Payload de la cabecera de autenticación `X-PRO-Auth-Payload` no es válida. Asegúrese de utilizar los valores de `AppKey` y `AppSecret` proporcionados
- EventId: 20005

## EpochNotSatisfiable

- StatusCode: 416
- ReasonPhrase: El valor de `Epoch` en la solicitud está muy adelante en el futuro o muy atrás en el pasado. **Aspen** permite un 'desfase' de hasta 12 horas en este campo. Corrija el valor y vuelva a intentar.
- EventId: 15851

## ForbiddenScope

- StatusCode: 403
- ReasonPhrase: Su AppKey ha sido deshabilitado o no tiene permisos para llevar a cabo la operación. Pónganse en contacto con nuestro equipo comercial.
- EventId: 1000478

## GatewayTimeout

- StatusCode: 504
- ReasonPhrase: **Aspen** no ha podido redirigir la solicitud al sistema responsable de procesarla. Pónganse en contacto con nuestro equipo de monitoreo.
- EventId: 15859

## InternalServerError

- StatusCode: 500
- ReasonPhrase: Algo ha salido mal en el servidor, pero no podemos ser más específicos sobre cuál es el problema exacto. Pónganse en contacto con nuestro equipo de monitoreo.
- EventId: 15841

## ItemAlreadyExist

- StatusCode: 409
- ReasonPhrase: Se está intentado procesar un elemento que ya existe en el sistema. El campo `ReasonPhrase` contiene un mensaje que describe el elemento.
- EventId: Dependiente del elemento.

## ItemNotFound

- StatusCode: 404
- ReasonPhrase: Ninguno.
- EventId: Ninguno.

## MalformedVersion

- StatusCode: 400
- ReasonPhrase: La cabecera personalizada en la solicitud `X-PRO-Auth-Version` contiene un valor no soportado.
- EventId: 945410.

## MissingCustomHeader

- StatusCode: 400
- ReasonPhrase: Falta alguna de las cabeceras de autenticación. El campo `ReasonPhrase` contiene un mensaje que describe la cabecera faltante.
- EventId: 20002.

## NonceAlreadyProcessed

- StatusCode: 409
- ReasonPhrase: Está intentando procesar el valor de un `Nonce` que ya fue procesado. Recuerde que cada solicitud debe llevar un identificador univoco. Cambie el valor de `Nonce` por uno que no se haya procesado y vuelva a intentar.
- EventId: 978410.

## TokenExpired

- StatusCode: 401
- ReasonPhrase: El valor del token suministrado para el proceso de autenticación ya expiro. Debe generar uno nuevo.
- EventId: 15847.

## Unauthorized

- StatusCode: 401
- ReasonPhrase: El proveedor de autenticación del sistema no pudo procesar la solicitud.  El campo `ReasonPhrase` contiene un mensaje que describe el error que encontró.
- EventId: 10746.

## UnsupportedRequestedVersion

- StatusCode: 400
- ReasonPhrase: La cabecera personalizada en la solicitud `X-PRO-Auth-Version` contiene un valor no soportado.
- EventId: 20008.