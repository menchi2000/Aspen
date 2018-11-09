# Tokenización

La tokenización permite a los emisores, adquirentes y comerciantes ofrecer servicios de pago más seguros al reemplazar los números de tarjeta con valores alternativos, desconectando el número de tarjeta o plástico y remplazándolo con un identificador único llamado `token de pago`. El 'mapeo' entre el PAN real (número de tarjeta o plástico) y un token de pago se almacena de manera segura en una bóveda electrónica, ocultando la información del PAN original, para eliminar este valor de los entornos donde los datos puedan ser vulnerables.

Un `token de pago` permite realizar operaciones sin la presentación de una tarjeta o plástico. Los pagos con token mejoran la seguridad del tarjetahabiente,  ya que los detalles del pago se guardan como un `token` evitando al usuario la presentación de un plástico o tarjeta.

La tokenización reduce en gran medida el riesgo de fraude, mediante la eliminación de los datos confidenciales de la tarjeta en la red de pago. Así, los números de tarjeta originales permanecen bajo control del emisor, mientras que los sistemas externos no tienen acceso a esta información.

Un `token de pago` está compuesto por números aleatorios, reemplazando los datos sensibles con datos no sensibles, haciendo que la información sea ilegible en sistemas intermedios.

## Cómo funciona la tokenización

Un token posee al menos los siguientes atributos:

- Está asociado con un usuario.
- Está asociado con un medio de pago (tarjeta/plástico).
- Tiene una fecha de vigencia.
- Solo se puede utilizar una vez (OTP).

### Paso a paso

- Paso 1: Se genera un token a partir de un número de cuenta para un uso único, como por ejemplo, para un retiro en un cajero automático, para el pago de un artículo o como un factor adicional de autenticación, por mencionar solo algunos de sus posibles usos.

- Paso 2: El token se almacena en una bóveda de tokens, un entorno compatible con [PCI DSS](https://en.wikipedia.org/wiki/Payment_Card_Industry_Data_Security_Standard) y se relaciona con el usuario que solicitó su generación.

- Paso 3: El token se muestra o entrega al usuario a través de un dispositivo, mensaje de texto o desde donde solicitó su generación.

- Paso 4: El usuario "presenta el token" en el comercio, utilizándolo en reemplazo de su número de tarjeta.

- Paso 5: El comercio envía el token al emisor a través de la red de pago.

- Paso 6: El emisor valida el token y, si coincide, aprueba la transacción.

- Paso 7: La respuesta del emisor se devuelve al comercio solicitante utilizando el token como referencia de la tarjeta.

## Cómo puedo utilizar la funcionalidad de tokens

<div id="tps"></div>

Para usar la tokenización, un emisor o comerciante debe convertirse en un _**proveedor de servicios de tokens (TSP)**_.

### Ejemplo

El usuario `U`, ingresa a la aplicación (una aplicación para dispositivos móviles, un portal transaccional, un IVR, etc.) del emisor utilizando sus credenciales y genera un token para efectuar una compra en el comercio `C`. La aplicación del emisor, solicita a `U` que ingrese el PIN transaccional, necesario para generar el token [2FA](https://en.wikipedia.org/wiki/Multi-factor_authentication). La aplicación solicita a **Aspen** la generación del token el cual queda almacenado de forma segura en una bóveda electrónica. La aplicación muestra el token a `U`. `U` se dirige a `C` y en el punto de pago menciona token como medio de pago. `C` solicita al usuario su número de identificación y token y envía esta información al emisor. El emisor verifica esta información contra la contenida en la bóveda y si coincide y se encuentra vigente, emite la autorización. 

## Información relacionada

- [Generación de un token transaccional](Generate-PaymentToken.md)

- [Validación de un token transaccional](Redeem-PaymentToken.md)
