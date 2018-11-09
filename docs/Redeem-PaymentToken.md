# Validación de un token de pago

Permite a un adquirente o comercio, verificar la validez de un token de pago que fue generado a través de la aplicación de movilidad del emisor.

> Cuando la validación es exitosa, se hace un débito en la cuenta asociada con el token de pago.

Verbo | Endpoint | Requiere autenticación
:---: | -------- | :------------:
PUT | /app/tokens/{Token} | [x]

## Datos de la solicitud

```json
{
  "DocType": "CC",
  "DocNumber": "0000000000",
  "ChannelKey": "ATM",
  "Metadata": "RANDOM_DATA_BY_ACQUIRER"
}
```

### Valores de la solicitud

Campo | Tipo de dato | Descripción | Requerido
:---: | :--------: | ------------ | :-----:
{Token} | string | Token de pago que se desea verificar. Valor en la URL sin corchetes | [x]
DocType | string | Tipo de documento del usuario para el que se genera el token | [x]
DocNumber | string | Número de documento del usuario para el que se genera el token | [x]
ChannelKey | string | Cadena que identifica de manera unívoca [el canal](Get-Channels.md) para el que se generó el token | [x]
Metadata | string | Metadatos asociados personalizados para el [TPS](Tokenization.md#tps) | [x]

## Datos de la respuesta

Código de estado de HTTP de acuerdo con la especificación [RFC 2616](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html)

### Valores de respuesta más utilizados

HttpStatus | Tipo de dato | Descripción
:---: | :--------: | ------------
200 | int | El token de pago se validó satisfactoriamente
404 | int | El token de pago no existe, no es válido o ya expiró

## Información relacionada

- [Tokenización: Concepto general](Tokenization.md)

- [Generación de un token transaccional](Generate-PaymentToken.md)

- [Proveedor de servicios de tokens (TSP)](Tokenization.md#tps)
