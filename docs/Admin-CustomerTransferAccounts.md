# Administrar cuentas para transferencias de un cliente

Expone las operaciones que facilitan la administración de las cuentas para transferencia de fondos de un cliente.

## Consultar cuentas registradas de un cliente

Obtiene la información de las cuentas vinculas a un cliente para realizar transferencia de fondos.

Verbo | Endpoint | Requiere autenticación
:---: | -------- | :------------:
GET | /app/transfers/accounts/{DocType}/{DocNumber} | [x]

### Valores de la solicitud

Campo | Tipo de dato | Descripción | Requerido
:---: | :----------: | ----------- | :-------:
{DocType} | `string` | Tipo de documento del cliente que tiene vinculadas las cuentas. Cualquier valor de la columna **Acrónimo** en la tabla de [Tipos de documento](Admin-CustomerTransferAccounts.md#attachedDocTypes). Valor esperado en la la URL sin corchetes. | [x]
{DocNumber} | `string` | Número de documento del cliente que tiene vinculadas las cuentas. Valor esperado en la la URL sin corchetes. | [x]

### Datos de la respuesta

```json
[
  {
    "Alias": "Cuenta-1234",
    "CardHolderName": "Katherine R. Bortz",
    "MaskedPan": "************1234"
  },
  {
    "Alias": "Cuenta-5678",
    "CardHolderName": "Doris J. Tadlock",
    "MaskedPan": "************5678"
  },
  {
    "Alias": "Cuenta-9101",
    "CardHolderName": "Peggy K. Montgomery",
    "MaskedPan": "************9101"
  }
]
```

<div class="admonition info">
   <p class="first admonition-title">Información adicional</p>
   <p class="last">Cuando el cliente no tiene cuentas registradas, la respuesta siempre será una lista vacía.</p>
</div>

### Valores de la respuesta

Campo | Tipo de dato | Descripción
:---: | :----------: | -----------
Alias | `string` | Nombre que identifica a la cuenta.
CardHolderName | `string` | Nombre del tarjetahabiente o titular de la cuenta.
MaskedPan | `string` | Número enmascarado de la cuenta.

## Registrar una cuenta para transferencias

Vincula la información de una cuenta a la lista de cuentas registradas para transferencia de fondos de un cliente.

Verbo | Endpoint | Requiere autenticación
:---: | -------- | :------------:
POST | /app/transfers/accounts/{DocType}/{DocNumber} | [x]

Campo | Tipo de dato | Descripción | Requerido
:---: | :----------: | ----------- | :-------:
{DocType} | `string` | Tipo de documento del cliente al cual se vinculará la cuenta. Cualquier valor de la columna **Acrónimo** en la tabla de [Tipos de documento](Admin-CustomerTransferAccounts.md#attachedDocTypes). Valor esperado en la la URL sin corchetes. | [x]
{DocNumber} | `string` | Número de documento del cliente al cual se vinculará la cuenta. Valor esperado en la la URL sin corchetes. | [x]
DocType | `string` | Tipo de documento del titular asociado con la cuenta. Cualquier valor de la columna **Acrónimo** en la tabla de [Tipos de documento](Admin-CustomerTransferAccounts.md#attachedDocTypes). | [x]
DocNumber | `string` | Número de documento del titular asociado con la cuenta. | [x]
Alias | `string` | Nombre con el que se identificará a la cuenta. | [x]
AccountNumber | `string` | Número de la cuenta. | [x]

### Datos de la respuesta

Código de estado de HTTP de acuerdo con la especificación [RFC 2616](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html)

## Eliminar una cuenta registrada

Desvincula la información de una cuenta de la lista de cuentas registradas para transferencias de un cliente.

Verbo | Endpoint | Requiere autenticación
:---: | -------- | :------------:
DELETE | /app/transfers/accounts/{DocType}/{DocNumber}/{Alias} | [x]

### Valores de la solicitud

Campo | Tipo de dato | Descripción | Requerido
:---: | :----------: | ----------- | :-------:
{DocType} | `string` | Tipo de documento del cliente que tiene vinculadas las cuentas. Cualquier valor de la columna **Acrónimo** en la tabla de [Tipos de documento](Admin-CustomerTransferAccounts.md#attachedDocTypes). Valor esperado en la la URL sin corchetes. | [x]
{DocNumber} | `string` | Número de documento ddel cliente que tiene vinculadas las cuentas. Valor esperado en la la URL sin corchetes. | [x]
{Alias} | `string` | Nombre con el que se identifica a la cuenta vinculada. Valor esperado en la la URL sin corchetes. | [x]

### Datos de la respuesta

Código de estado de HTTP de acuerdo con la especificación [RFC 2616](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html)

## Valores de respuesta más utilizados

HttpStatus | Tipo de dato | Descripción
:--------: | :----------: | -----------
200 | `int` | La solicitud finalizó satisfactoriamente.
404 | `int` | El cliente no existe en el sistema.
409 | `int` | No se encuentra información de la cuenta para vincular con los datos suministrados.

## Anexos

### Tipos de documento

<div id="attachedDocTypes"></div>

Acrónimo | Descripción
:------: | -----------
CC | Cédula de Ciudadanía
NIT | Número de Identificación Tributaria
TI | Tarjeta de Identidad
CE | Cédula de Extranjería
PAS | Pasaporte

## Información relacionada

- [Solicitar un token de autenticación](JWT-Request.md)