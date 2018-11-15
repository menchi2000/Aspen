# Consultas de productos financieros de un cliente

Expone las operaciones de consultas transaccionales disponibles para un usuario.

## Consultar productos disponibles de un cliente

Obtiene la información resumida de las cuentas o productos  asociados a un usuario.

Verbo | Endpoint | Requiere autenticación
:---: | -------- | :--------------------:
GET | /app/inquires/accounts/{DocType}/{DocNumber} | [x]

### Valores de la solicitud

Campo | Tipo de dato | Descripción | Requerido
:---: | :----------: | ----------- | :-------:
{DocType} | `string` | Tipo de documento del usuario. Cualquier valor de la columna **Acrónimo** en la tabla de [Tipos de documento](Inquiries-CustomerAccounts.md#attachedDocTypes). Valor esperado en la la URL sin corchetes. | [x]
{DocNumber} | `string` | Número de documento del usuario. Valor esperado en la la URL sin corchetes. | [x]

### Datos de la respuesta

```json
[
  {
    "Balance": 0,
    "Id": "1234113",
    "MaskedPan": "************9836",
    "Name": "Tarjeta Multibolsillo",
    "Order": 0,
    "Properties": [
      {
        "Label": "Última transacción",
        "Key": "LastTranName",
        "Value": "Consulta últimos movimientos"
      },
      {
        "Label": "Fecha",
        "Key": "LastTranDate",
        "Value": "nov 2 de 2018 a las 4:06 PM"
      },
      {
        "Label": "Lugar",
        "Key": "LastTranCardAcceptor",
        "Value": "Almacen Compensar Movilidad"
      }
    ],
    "Source": 0,
    "SourceAccountId": null
  }
]
```

<div class="admonition info">
   <p class="first admonition-title">Información adicional</p>
   <p class="last">Cuando el cliente no existe o no tiene productos asociados, la respuesta siempre será una lista vacía.</p>
</div>

### Valores de la respuesta

Campo | Tipo de dato | Descripción
:---: | :----------: | -----------
Balance | `decimal` | Valor del saldo actual de la cuenta.
Id | `string` | Identificador unívoco de la cuenta.
MaskedPan | `string` | Número enmascarado de la cuenta.
Order | `int` | Orden del elemento para visualizar en interfaz de usuario.
Properties | `list` | Es un conjunto de propiedades o atributos que representan información adicional de la cuenta. [Propiedades de una cuenta  ](Inquiries-CustomerAccounts.md#responseAccountProperties)
Source | `int` | Define los sistemas reconocidos desde donde se originaron los datos de la cuenta. [Tipos de sistemas](Inquiries-CustomerAccounts.md#responseEnumSubsystems)
SourceAccountId | `string` | Identificador de la cuenta que se utilizará en procesos transaccionales. Cuando es `null`, la cuenta no permite procesos transaccionales.

#### Propiedades de una cuenta

<div id="responseAccountProperties"></div>

Campo | Tipo de dato | Descripción
:---: | :----------: | -----------
Label | `string` | Nombre o etiqueta para visualizar el contenido del atributo en interfaz de usuario.
Key | `string` | Identificador interno del atributo.
Value | `string` | Valor asociado con el atributo.

<div class="admonition warning">
   <p class="first admonition-title">Atención</p>
   <p class="last">El conjunto de propiedades o atributos de una cuenta pueden variar de acuerdo con el sistema de origen del producto.</p>
</div>

#### Tipos de sistemas

<div id="responseEnumSubsystems"></div>

Valor | Nombre | Descripción
:---: | :----: | -----------
`0` | Tup | El origen de la información es del sistema **TUP**.
`1` | Bancor | El origen de la información es del sistema **BANCOR**.

## Consultar saldos de una cuenta

Obtiene los saldos (balances) detallados de una cuenta cuando sistema de información administra/implementa subtipos de cuenta como los bolsillos en TUP.

<div class="admonition warning">
   <p class="first admonition-title">Atención</p>
   <p class="last">La consulta de movimientos solo está disponible para el producto <b>Tarjeta Multibolsillo</b>, de lo contrario la respuesta siempre será una lista vacía.</p>
</div>

Verbo | Endpoint | Requiere autenticación
:---: | -------- | :--------------------:
GET | /app/inquires/accounts/{DocType}/{DocNumber}/{AccountId}/balances | [x]

### Valores de la solicitud

Campo | Tipo de dato | Descripción | Requerido
:---: | :----------: | ----------- | :-------:
{DocType} | `string` | Tipo de documento del usuario. Cualquier valor de la columna **Acrónimo** en la tabla de [Tipos de documento](Inquiries-CustomerAccounts.md#attachedDocTypes). Valor esperado en la la URL sin corchetes. | [x]
{DocNumber} | `string` | Número de documento del usuario. Valor esperado en la la URL sin corchetes. | [x]
{AccountId} | `string` | Identificador de la cuenta para la que se obtienen los saldos. Valor esperado en la la URL sin corchetes. | [x]

### Datos de la respuesta

```json
[
  {
    "Balance": 1000,
    "Number": "**************1280",
    "SourceAccountId": "845698|80",
    "TypeId": "80",
    "TypeName": "Monedero General"
  },
  {
    "Balance": 1000,
    "Number": "**************1281",
    "SourceAccountId": "845698|81",
    "TypeId": "81",
    "TypeName": "Subsidio familiar"
  },
  {
    "Balance": 1000,
    "Number": "**************1282",
    "SourceAccountId": "845698|82",
    "TypeId": "82",
    "TypeName": "Subsidio Educativo"
  },
  {
    "Balance": 1000,
    "Number": "**************1283",
    "SourceAccountId": "845698|83",
    "TypeId": "83",
    "TypeName": "Bonos"
  },
  {
    "Balance": 1000,
    "Number": "**************1284",
    "SourceAccountId": "845698|84",
    "TypeId": "84",
    "TypeName": "Viveres General"
  }
]
```

<div class="admonition info">
   <p class="first admonition-title">Información adicional</p>
   <p class="last">Cuando la cuenta no presenta saldos, la respuesta siempre será una lista vacía.</p>
</div>

### Valores de la respuesta

Campo | Tipo de dato | Descripción
:---: | :----------: | -----------
Balance | `decimal` | Valor del saldo actual de la cuenta.
Number | `string` | Número enmascarado de la cuenta.
SourceAccountId | `string` | Identificador de la cuenta que se utilizará en procesos transaccionales. Cuando es `null`, la cuenta no permite procesos transaccionales.
TypeId | `string` | Identificador del tipo de cuenta (bolsillo) en el sistema TUP.
TypeName | `string` | Nombre del tipo de cuenta (bolsillo) en el sistema TUP.

## Consultar movimientos de una cuenta

Obtiene la información de transacciones finacieras realizadas en una cuenta.

Verbo | Endpoint | Requiere autenticación
:---: | -------- | :--------------------:
GET | /app/inquires/accounts/{DocType}/{DocNumber}/{AccountId}/{AccountTypeId?}/statements | [x]

### Valores de la solicitud

Campo | Tipo de dato | Descripción | Requerido
:---: | :----------: | ----------- | :-------:
{DocType} | `string` | Tipo de documento del usuario. Cualquier valor de la columna **Acrónimo** en la tabla de [Tipos de documento](Inquiries-CustomerAccounts.md#attachedDocTypes). Valor esperado en la la URL sin corchetes. | [x]
{DocNumber} | `string` | Número de documento del usuario. Valor esperado en la la URL sin corchetes. | [x]
{AccountId} | `string` | Identificador de la cuenta para la que se obtienen los saldos. Valor esperado en la la URL sin corchetes. | [x]
{AccountTypeId?} | `string` | Identificador del tipo de cuenta. Cuando se establece, los movimientos corresponden a una cuenta (bolsillo) en el sistema TUP. Puede usar asterisco (`*`) para consultar los últimos movimientos de todo el prodcuto. Valor esperado en la la URL sin corchetes. |

### Datos de la respuesta

```json
[
  {
    "AccountTypeId": "80",
    "AccountTypeName": "Monedero General",
    "Amount": 1000,
    "CardAcceptor": "Almacen Compensar Movilidad",
    "Category": 0,
    "Date": "2018-11-14T17:22:15.4630000-05:00",
    "TranName": "Transferencia",
    "TranType": "40"
  },
  {
    "AccountTypeId": "80",
    "AccountTypeName": "Monedero General",
    "Amount": 1000,
    "CardAcceptor": "Almacen Compensar Movilidad",
    "Category": 0,
    "Date": "2018-11-14T17:22:00.8370000-05:00",
    "TranName": "Transferencia",
    "TranType": "40"
  },
  {
    "AccountTypeId": "80",
    "AccountTypeName": "Monedero General",
    "Amount": 1000,
    "CardAcceptor": "Almacen Compensar Movilidad",
    "Category": 0,
    "Date": "2018-11-14T17:05:58.3870000-05:00",
    "TranName": "Transferencia",
    "TranType": "40"
  }
]
```

<div class="admonition info">
   <p class="first admonition-title">Información adicional</p>
   <p class="last">Cuando la cuenta no presenta movimientos, la respuesta siempre será una lista vacía.</p>
</div>

### Valores de la respuesta

Campo | Tipo de dato | Descripción
:---: | :----------: | -----------
AccountTypeId | `string` | Identificador del tipo de cuenta que afectó el movimiento/transacción.
AccountTypeName | `string` | Nombre del tipo de cuenta que afectó el movimiento/transacción.
Amount | `decimal` | Valor por el que se realizó el movimiento/transacción.
CardAcceptor | `string` | Nombre del comercio donde se realizó el movimiento/transacción.
Category | `int` | Define la naturaleza contable de la transacción finaciera. [Tipos de categoria](Inquiries-CustomerAccounts.md#responseEnumCategory)
Date | `datetime` | Fecha y hora en que se realizó el movimiento/transacción.
TranName | `string` | Nombre que representa el tipo de movimiento/transacción.
TranType | `string` | Código que representa el tipo de movimiento/transacción.

#### Tipos de categoria

<div id="responseEnumCategory"></div>

Valor | Nombre | Descripción
:---: | :----: | -----------
`0` | Debit | La naturaleza del débito representa un valor que fue restado de la cuenta.
`1` | Credit | La naturaleza del crédito representa un valor a favor de la cuenta.

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