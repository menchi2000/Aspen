# Activación de tarjeta y asignación de clave

Procesa una solicitud para la activación y asignación de clave de la tarjeta de un cliente.

| Verbo | Endpoint                                      | Requiere autenticación |
| :---: | --------------------------------------------- | :--------------------: |
| POST  | http://localhost/api/app/ext/tup/cards/activate |          [ Si ]        |

[^Segmentos de URL]: La información entre corchetes en la URL se denomina segmentos de URL y aplican solo para algunas operaciones. Cuando aparezcan en un ejemplo, deben ser reemplazados por sus valores correspondientes omitiendo los corchetes. Por ejemplo, sin en la URL de ejemplo apareciera http://localhost/api/operation/value/{value}, para establecer el valor de  `value` en la solicitud a la cadena `abc`, la URL final se vería de la siguiente forma: http://localhost/api/operation/value/abc 

## Datos de la solicitud (body)

```json
{
  "CorrelationalId": "f8f14a9b-45b6-4cce-b45d-7353a99b1f65",
  "DocNumber": "35512889",
  "DocType": "1",
  "LastFourCardDigits": "0257",
  "Pin": "5ABB40E21DD968FA",
  "Kwp": "B21000000075",
  "VerificationCode": "CODE12345"
}
```

### Valores de la solicitud

Campo | Tipo de dato| Descripción | Requerido
:---: | :--------:| ------------ | :-----:
CorrelationalId | guid |Identificador de la transacción, debe ser único por cada solicitud enviada.| [Si]
DocNumber | string | Número de identificación del titular de la tarjeta que se va a activar. | [ Si ]
DocType | string | **[Tipo de identificación](#Tipos-de-identificación)** del titular de la tarjeta que se va a activar. | [ No ]
LastFourCardDigits | string | Cuatro últimos digitos de la tarjeta ha ser activada. | [ Si ]
Pin | string | Clave de la tarjeta que se asigna en el proceso de activación. Se debe enviar el pinblock como una cadena hexadecimal de longitud 16, ejemplo "5ABB40E21DD968FA". Ejemplo generación de **[PinBlock](https://github.com/RD-Processa/Pinblock-Helper)**. | [ Si ]
Kwp | string | Nombre de la llave KWP con la cual fue generado el pinblock, se debe enviar en este campo el nombre asociado a la llave KWP que fue registrada en la ceremonia de intercambio de llaves, sin incluir el prefijo.| [ Si ]
VerificationCode | string | Código entregado por el sistema que realiza la validación de identidad del tarjetahabiente. | [Si] 

## Datos de la respuesta

Código de estado de HTTP de acuerdo con la especificación **[RFC 2616](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html)** 

### Valores de respuesta más utilizados

<div id="httpStatusCodes"></div>

HttpStatus | Tipo | Descripción
:---: | :--------: | ------------
200 | int | Transacción exitosa. La transacción de activación de tarjeta y asignación de clave se realizó satisfactoriamente. 
404 | int | No se encontró una tarjeta asociada al cliente para ser activada.
