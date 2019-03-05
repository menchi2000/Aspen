# Cambio de clave de una tarjeta

Procesa una solicitud para realizar el cambio de clave de la tarjeta de un cliente.

| Verbo | Endpoint                                      | Requiere autenticación |
| :---: | --------------------------------------------- | :--------------------: |
| POST  | http://localhost/api/app/ext/tup/cards/changepin |          [ Si ]        |

[^Segmentos de URL]: La información entre corchetes en la URL se denomina segmentos de URL y aplican solo para algunas operaciones. Cuando aparezcan en un ejemplo, deben ser reemplazados por sus valores correspondientes omitiendo los corchetes. Por ejemplo, sin en la URL de ejemplo apareciera http://localhost/api/operation/value/{value}, para establecer el valor de  `value` en la solicitud a la cadena `abc`, la URL final se vería de la siguiente forma: http://localhost/api/operation/value/abc 

## Datos de la solicitud (body)

```json
{
  "CorrelationalId": "f8f14a9b-45b6-4cce-b45d-7353a99b1f65",
  "DocNumber": "35512889",
  "DocType": "1",
  "LastFourCardDigits": "0257",
  "OldPin": "5ABB40E21DD968FA",
  "NewPin": "5B4BB2AE1E485785",
  "Kwp": "B21000000075",
  "VerificationCode" : "CODE12345"
}
```

### Valores de la solicitud

Campo | Tipo de dato| Descripción | Requerido
:---: | :--------:| ------------ | :-----:
CorrelationalId | guid |Identificador de la transacción, debe ser único por cada solicitud enviada.| [Si]
DocNumber | string | Número de identificación del titular de la tarjeta que realiza el cambio de clave. | [ Si ]
DocType | string | **[Tipo de identificación](#Tipos-de-identificación)** del titular de la tarjeta que realiza el cambio de clave. | [ No ]
LastFourCardDigits | string | Cuatro últimos digitos de la tarjeta que le va ha realizar cambio de pin. | [ Si ]
OldPin | string | Clave actual de la tarjeta que envía el cliente en la operación para ser cambiada. Se debe enviar el pinblock como una cadena hexadecimal de longitud 16, ejemplo "5ABB40E21DD968FA". Ejemplo generación de **[PinBlock](https://github.com/RD-Processa/Pinblock-Helper)**.| [ Si ]
NewPin | string | Clave nueva de la tarjeta que envía el cliente para ser asignada a la tarjeta. Se debe enviar el pinblock como una cadena hexadecimal de longitud 16, ejemplo "5B4BB2AE1E485785". Ejemplo generación de **[PinBlock](https://github.com/RD-Processa/Pinblock-Helper)**.| [ Si ]
Kwp | string | Nombre de la llave KWP con la cual fue generado el pinblock, se debe enviar en este campo el nombre asociado a la llave KWP que fue registrada en la ceremonia de intercambio de llaves, sin incluir el prefijo.| [ Si ]
VerificationCode | string | Código entregado por el sistema que realiza la validación de identidad del tarjetahabiente. | [Si] 

## Datos de la respuesta

Código de estado de HTTP de acuerdo con la especificación **[RFC 2616](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html)** 

### Valores de respuesta más utilizados

HttpStatus | Tipo | Descripción
:---: | :--------: | ------------
200 | int | Transacción exitosa. La transacción de cambio de clave de la tarjeta se realizó satisfactoriamente. 
404 | int | No se encontró una tarjeta asociada al cliente para realizar el cambio de la clave.
