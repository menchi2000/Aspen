# Mensajes de respuesta

**Aspen**, luego de recibir, interpretar y procesar un mensaje de solicitud, genera un mensaje de respuesta HTTP

El elemento `StatusCode` en la respuesta, es el código de resultado de la operación, que consiste en un grupo de 3 dígitos con el resultado de la solicitud.
Los códigos generados por **Aspen** acogen la definición del protocolo HTTP definido en el [RFC 2616](https://www.ietf.org/rfc/rfc2616.txt).
El valor del campo `StatusCode` intenta proporcionar una descripción textual del resultado de la operación dentro de un dominio de valores.
`StatusCode` se puede utilizar en procesos de automatización,  mientras que `ReasonPhrase` intenta proporcionar un mensaje descriptivo enfocado en el razonamiento humano.

El primer dígito en `StatusCode` define la clase de respuesta. Los últimos dos dígitos no tienen ninguna función de categorización. Hay 5 posibles valores para el primer dígito:

Grupo | Descripción | Acción necesaria
:---: | ----------- | -------------
2xx | OK | La acción se recibió y se procesó de forma exitosa. No se requiere ninguna acción adicional.
3xx | Redirección |  Se deben tomar medidas adicionales para completar la solicitud. El recurso (Url) solicitado se ha movido a otro lugar. Se debe redirigir la solicitud a la nueva Url.
4xx | Error de cliente | La solicitud contiene datos inválidos o faltan datos. Los datos enviados por el cliente no permiten procesar la solicitud. Se deben corregir los datos de la solicitud y volver a intentar.
5xx | Error de servidor | El servidor no pudo procesar la solicitud. Se requieren correcciones del lado del servidor.

Las aplicaciones cliente no tienen que comprender el significado de todos los códigos de estado `StatusCode`, aunque tal comprensión es deseable.
Sin embargo, las aplicaciones **DEBEN** procesar al menos el primer dígito de `StatusCode`, y tratar cualquier respuesta no reconocida como equivalente al código de estado x00 de ese dominio.
Por ejemplo, si el cliente recibe un código de `StatusCode` 431, puede asumir con seguridad que hubo un problema con su solicitud y tratar la respuesta como si hubiera recibido un `StatusCode` 400.

## Campos de encabezado personalizado en la respuesta

**Aspen** incluye en cada respuesta dos campos personalizados así:

Key | Value
--- | -----
**X&#x2011;PRO&#x2011;Response&#x2011;Help** | Contiene la Url donde se describe la razón por la que no se pudo procesar una solicitud. Se incluye en la respuesta para cualquier solicitud con un status diferente a 2xx.
**X&#x2011;PRO&#x2011;Response&#x2011;Time** | Contiene una cadena en formato minutos:segundos:milisegundos (mm:ss:fff) con el tiempo que tomó procesar la solicitud. No se incluye el tiempo de latencia.

## Mensaje de repuesta comunes

[Un código de respuesta HTTP](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes) es generado por **Aspen** en respuesta a una solicitud del cliente hecha al servidor.

Una respuesta en el grupo 2xx indica el procesamiento satisfactorio de la solicitud. Una respuesta en el grupo 4xx indica que los parámetros proporcionados para completar la operación no son válidos o están incompletos. Para estos casos, revise el mensaje en `ReasonPhrase`. Le ayudará a comprender el porqué de la situación.

## Identificadores en mensajes de respuesta

La cabecera personalizada de respuesta `X-PRO-Response-Help` puede contener alguno de los valores mencionados en el siguiente sección. Nuevamente el analisis del mensaje en `ReasonPhrase` le ayudará a comprender el porqué de la situación.

## AppKeyDoesNotExist

- StatusCode: 400
- Reason: Necesita utilizar el identificador de aplicación `AppKey` suministrado.
- EventId: 20005

## AppKeyIsDisabled

- StatusCode: 403
- Reason: El identificador de aplicación `AppKey` suministrado no está habilitado en el sistema.
- EventId: 20006

## BadGateway

- StatusCode: 502
- Reason: **Aspen** no ha podido redirigir la solicitud al sistema responsable de procesarla. Pónganse en contacto con nuestro equipo de monitoreo.
- EventId: 15859

## BadRequest

- StatusCode: 400
- Reason: La solicitud no pudo ser procesada por el servidor. Los datos enviados por el cliente no son válidos. El campo `ReasonPhrase` contiene un mensaje que describe de forma detallada los datos que no pudieron ser procesados.
- EventId: 20005

## DecodeAuthHeaderFailed

- StatusCode: 400
- Reason: La información suministrada en el Payload de la cabecera de autenticación `X-PRO-Auth-Payload` no es válida. Asegúrese de utilizar los valores de `AppKey` y `AppSecret` proporcionados
- EventId: 20005

## EpochNotSatisfiable

- StatusCode: 416
- Reason: El valor de `Epoch` en la solicitud está muy adelante en el futuro o muy atrás en el pasado. **Aspen** permite un 'desfase' de hasta 12 horas en este campo. Corrija el valor y vuelva a intentar.
- EventId: 15851

## ForbiddenScope

- StatusCode: 403
- Reason: Su AppKey ha sido deshabilitado o no tiene permisos para llevar a cabo la operación. Pónganse en contacto con nuestro equipo comercial.
- EventId: 1000478

## GatewayTimeout

- StatusCode: 504
- Reason: **Aspen** no ha podido redirigir la solicitud al sistema responsable de procesarla. Pónganse en contacto con nuestro equipo de monitoreo.
- EventId: 15859

## InternalServerError

- StatusCode: 500
- Reason: Algo ha fallado en el servidor, pero no podemos ser más específicos sobre cuál es el problema exacto. Pónganse en contacto con nuestro equipo de monitoreo.
- EventId: 15841

## ItemAlreadyExist

- StatusCode: 409
- Reason: Se está intentado procesar un elemento que ya existe en el sistema. El campo `ReasonPhrase` contiene un mensaje que describe el elemento.
- EventId: Dependiente del elemento.

## ItemNotFound

- StatusCode: 404
- Reason: Ninguno.
- EventId: Ninguno.

## MalformedVersion

- StatusCode: 400
- Reason: La cabecera personalizada en la solicitud `X-PRO-Auth-Version` contiene un valor no soportado.
- EventId: 99001.

## MissingCustomHeader

- StatusCode: 400
- Reason: Falta alguna de las cabeceras de autenticación. El campo `ReasonPhrase` contiene un mensaje que describe la cabecera faltante.
- EventId: 20002.

## NonceAlreadyProcessed

- StatusCode: 409
- Reason: Está intentando procesar el valor de un `Nonce` que ya fue procesado. Recuerde que cada solicitud debe llevar un identificador unívoco. Cambie el valor de `Nonce` por uno que no se haya procesado y vuelva a intentar.
- EventId: 99003.

## TokenExpired

- StatusCode: 401
- Reason: El valor del token suministrado para el proceso de autenticación ya expiro. Debe generar uno nuevo.
- EventId: 15847.

## Unauthorized

- StatusCode: 401
- Reason: El proveedor de autenticación del sistema no pudo procesar la solicitud.  El campo `ReasonPhrase` contiene un mensaje que describe el error que encontró.
- EventId: 10746.

## PinNumberNotAcceptable
- StatusCode: 406
- Reason: El Formato del PIN que se intenta establecer no cumple con alguna de las políticas. El campo `ReasonPhrase` contiene un mensaje que describe el error que encontró.
- EventId: 15860.

## UnsupportedRequestedVersion

- StatusCode: 400
- Reason: La cabecera personalizada en la solicitud `X-PRO-Auth-Version` contiene un valor no soportado.
- EventId: 99005.

## UpgradeSecretRequired

- StatusCode: 426
- Reason: Se requiere la actualización del secreto o clave de la aplicación que se utiliza para firmar las solicitudes.
- EventId: 20009.

## SecretNotAcceptable
- StatusCode: 406
- Reason: El formato del secreto que se intenta establecer para la aplicación, no cumple con alguna de las políticas. El campo `ReasonPhrase` contiene un mensaje que describe el error que encontró.
- EventId: 15864.
