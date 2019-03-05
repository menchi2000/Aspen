# Bloqueo de una tarjeta

Procesa una solicitud para realizar el bloqueo de la tarjeta de un cliente.

| Verbo | Endpoint                                      | Requiere autenticación |
| :---: | --------------------------------------------- | :--------------------: |
| POST  | http://localhost/api/app/ext/tup/cards/lock   |          [ Si ]        |

[^Segmentos de URL]: La información entre corchetes en la URL se denomina segmentos de URL y aplican solo para algunas operaciones. Cuando aparezcan en un ejemplo, deben ser reemplazados por sus valores correspondientes omitiendo los corchetes. Por ejemplo, sin en la URL de ejemplo apareciera http://localhost/api/operation/value/{value}, para establecer el valor de  `value` en la solicitud a la cadena `abc`, la URL final se vería de la siguiente forma: http://localhost/api/operation/value/abc 

## Datos de la solicitud (body)

```json
{
  "CorrelationalId": "g6t14a9b-42b1-4dde-a45d-4568a99b1f65",
  "DocNumber": "35512889",
  "DocType": "1",
  "LastFourCardDigits": "0257",
  "Reason": 3001,
  "VerificationCode" : "CODE12345"
}
```

### Valores de la solicitud

Campo | Tipo de dato| Descripción | Requerido
:---: | :--------:| ------------ | :-----:
CorrelationalId | guid |Identificador de la transacción, debe ser único por cada solicitud enviada.| [Si]
DocNumber | string | Número de identificación del titular de la tarjeta que se va a bloquear. | [ Si ]
DocType | string | **[Tipo de identificación](#Tipos-de-identificación)** del titular de la tarjeta que se va a bloquear. | [ No ]
LastFourCardDigits | string | Cuatro últimos digitos de la tarjeta ha ser bloqueada. | [ Si ]
Reason | int | **[Código](#blockTypes)** de 4 dígitos que indica la razón por la cual será bloqueada la tarjeta. | [ Si ]
VerificationCode | string | Código entregado por el sistema que realiza la validación de identidad del tarjetahabiente. | [Si] 

## Datos de la respuesta

Código de estado de HTTP de acuerdo con la especificación **[RFC 2616](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html)** 

### Valores de respuesta más utilizados

HttpStatus | Tipo | Descripción
:---: | :--------: | ------------
200 | int | Transacción exitosa. La transacción de bloqueo de tarjeta se realizó satisfactoriamente. 
404 | int | No se encontró una tarjeta asociada al cliente para ser bloqueada.
