# Generación de un token de pago

Permite al usuario genera un token para un canal, a través de la aplicación de movilidad del emisor.

> Esta operación es irrelevante para los comercios adquirientes, pero se describe aquí para facilitar la comprensión general del proceso.

Verbo | Endpoint | Requiere autenticación
:---: | -------- | :------------:
POST | /me/tokens | [x]

## Datos de la solicitud

```json
{
  "ChannelKey": "ATM",
  "DocType": "CC",
  "DocNumber": "123456",
  "Metadata": "RANDOM_DATA_BY_ACQUIRER",
  "PinNumber": "000000"
}
```

### Valores de la solicitud

Campo | Tipo de dato | Descripción | Requerido
:---: | :--------: | ------------ | :-----:
ChannelKey | string | Cadena que identifica de manera unívoca [el canal](Get-Channels.md) para el que se genera el token | [x]
DocType | string | Tipo de documento del usuario para el que se genera el token | [x]
DocNumber | string | Número de documento del usuario para el que se genera el token | [x]
Metadata | string | Metadatos asociados personalizados para el [TPS](Tokenization.md#tps) | [x]
PinNumber | string | Pin asociado con el usuario para la generación del token | [x]

## Datos de la respuesta

```json
{
  "Token": "000000",
  "ExpiresAt": "2016-07-31T15:41:20.6787253+00:00",
  "ChannelKey": "ATM",
  "ChannelName": "Retiro cajero automático"
}
```

### Valores de la respuesta

Campo | Tipo de dato | Descripción
:---: | :--------: | ------------
Token | string | Valor que representa el token de pago.
ExpiresAt | datetime | Fecha y hora en formato [UTC](https://en.wikipedia.org/wiki/Coordinated_Universal_Time) en la que expira la validez del token.
ChannelKey | string | Identificador del canal para el que se generó el token.
ChannelName | string | Nombre descriptivo del canal para el que se generó el token..

## Información relacionada

- [Tokenización: Concepto general](Tokenization.md)

- [Canales para tokens de pago](Get-Channels.md)

- [Proveedor de servicios de tokens (TSP)](Tokenization.md#tps)

- [Validación de un token de pago](Redeem-PaymentToken.md)
