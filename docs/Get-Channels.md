# Canales para tokens de pago

Los tokens además de reemplazar el número de una tarjeta, se pueden utilizar para aprovechar otros medios de aceptación. Por ejemplo, un token podría utilizarse como segundo factor de autenticación [2FA](https://en.wikipedia.org/wiki/Multi-factor_authentication),  o como una clave dinámica para retirar dinero en un cajero automático, por mencionar algunos usos, por lo que la tokenización no limita su uso únicamente a los terminales de puntos de venta o [POS](https://en.wikipedia.org/wiki/Point_of_sale).

Cuando el usuario genera un token, debe establecer el objetivo (canal) por donde planea utilizarlo, ya que una vez generado, solo se podrá hacer uso del token en dicho canal.

La operación para obtener la lista de canales, retorna una lista de los medios autorizados para la utilización de los tokens.

## Generalidades

Verbo | Endpoint | Requiere autenticación
:---: | -------- | :------------:
GET | /app/tokens/channels | [x]

## Datos de la solicitud

> Esta operación no requiere valores de entrada.

## Datos de la respuesta

```json
[
  {
    "Key": "ATM",
    "Name": "Retiro cajero automático"
  },
  {
    "Key": "PAG",
    "Name": "Pago comercios afiliados"
  },
  {
    "Key": "2FA",
    "Name": "Clave dinámica de acceso Web"
  }
]
```

### Valores de la respuesta

Campo | Tipo de dato | Descripción
:---: | :--------: | ------------
Key | string | Cadena que identifica de manera unívoca el canal
Name | string | Nombre descriptivo que identifica el canal

## Información relacionada

- [Tokenización: Concepto general](Tokenization.md)

- [Generación de un token de pago](Generate-PaymentToken.md)

- [Validación de un token de pago](Redeem-PaymentToken.md)