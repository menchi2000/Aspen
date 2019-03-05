# Restablecer la clave de una tarjeta

Procesa una solicitud para restablecer la clave de la tarjeta de un cliente en caso de olvido o pérdida de la clave actual.

| Verbo | Endpoint                                      | Requiere autenticación |
| :---: | --------------------------------------------- | :--------------------: |
| POST  | http://localhost/api/app/ext/tup/cards/resetpin |          [ Si ]        |

[^Segmentos de URL]: La información entre corchetes en la URL se denomina segmentos de URL y aplican solo para algunas operaciones. Cuando aparezcan en un ejemplo, deben ser reemplazados por sus valores correspondientes omitiendo los corchetes. Por ejemplo, sin en la URL de ejemplo apareciera http://localhost/api/operation/value/{value}, para establecer el valor de  `value` en la solicitud a la cadena `abc`, la URL final se vería de la siguiente forma: http://localhost/api/operation/value/abc 

## Datos de la solicitud (body)

```json
{
  "CorrelationalId": "g6t14a9b-42b1-4dde-a45d-4568a99b1f65",
  "DocNumber": "35512889",
  "DocType": "1",
  "LastFourCardDigits": "0257",
  "NewPin": "5ABB40E21DD968FA",
  "Kwp": "B21000000075",
  "VerificationCode" : "CODE12345"
}
```

### Valores de la solicitud

Campo | Tipo de dato| Descripción | Requerido
:---: | :--------:| ------------ | :-----:
CorrelationalId | guid | Identificador de la petición, debe ser único por cada solicitud (request) que se realice. | [ Si ]
DocNumber | string | Número de identificación del titular de la tarjeta que realiza el reestablecimiento de clave. | [ Si ]
DocType | string | **[Tipo de identificación](#Tipos-de-identificación)** del titular de la tarjeta que realiza el reestablecimiento de clave. | [ No ]
LastFourCardDigits | string | Últimos 4 dígitos del número de la tarjeta a la cual se le va a realizar el reestablecimiento de clave. | [ Si ]
NewPin | string | Clave nueva de la tarjeta que será asignada en la operación de reasignación de clave. Se debe enviar el pinblock como una cadena hexadecimal de longitud 16, ejemplo "5ABB40E21DD968FA". Ejemplo generación de **[PinBlock](https://github.com/RD-Processa/Pinblock-Helper)**. | [ Si ]
Kwp | string | Nombre de la llave KWP con la cual fue generado el pinblock, se debe enviar en este campo el nombre asociado a la llave KWP que fue registrada en la ceremonia de intercambio de llaves, sin incluir el prefijo.| [ Si ]
VerificationCode | string | Código entregado por el sistema que realiza la validación de identidad del tarjetahabiente. | [Si] 

## Datos de la respuesta

Código de estado de HTTP de acuerdo con la especificación **[RFC 2616](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html)** 

### Valores de respuesta más utilizados

HttpStatus | Tipo | Descripción
:---: | :--------: | ------------
200 | int | Transacción exitosa. La transacción de reasignación de clave de la tarjeta se realizó satisfactoriamente. 
404 | int | No se encontró una tarjeta asociada al cliente para realizar la reasignación de la clave.
