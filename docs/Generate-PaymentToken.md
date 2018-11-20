# Generación de un token transaccional

Permite al usuario generar un token, a través de una aplicación (una aplicación para dispositivos móviles, un portal transaccional, un IVR, etc.) del emisor.

> Esta operación es irrelevante para los comercios adquirientes, pero se describe aquí para facilitar la comprensión general del proceso.

Verbo | Endpoint | Requiere autenticación
:---: | -------- | :------------:
POST | http://localhost/api/me/tokens | [x]

## Datos de la solicitud

```json
{
  "DocType": "CC",
  "DocNumber": "123456",
  "Metadata": "RANDOM_DATA_BY_ACQUIRER",
  "PinNumber": "000000"
}
```

### Valores de la solicitud

Campo | Tipo de dato | Descripción | Requerido
:---: | :--------: | ------------ | :-----:
DocType | string | Tipo de documento del usuario para el que se genera el token | [x]
DocNumber | string | Número de documento del usuario para el que se genera el token | [x]
Metadata | string | Metadatos asociados personalizados para el [TPS](Tokenization.md#tps) | [x]
PinNumber | string | Pin asociado con el usuario para la generación del token | [x]

## Datos de la respuesta

```json
{
  "Token": "000000",
  "ExpiresAt": "2016-07-31T15:41:20.6787253+00:00",
}
```

### Valores de la respuesta

Campo | Tipo de dato | Descripción
:---: | :--------: | ------------
Token | string | Valor que representa el token de pago.
ExpirationMinutes | int | Duración en minutos del token transaccional.
ExpiresAt | datetime | Fecha y hora en formato [UTC](https://en.wikipedia.org/wiki/Coordinated_Universal_Time) en la que expira la validez del token.

## Información relacionada

- [Tokenización: Concepto general](Tokenization.md)

- [Proveedor de servicios de tokens (TSP)](Tokenization.md#tps)

- [Validación de un token transaccional](Redeem-PaymentToken.md)
