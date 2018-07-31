# Registro de aplicaciones

Para poder hacer uso de esta API, se requiere el registro de una aplicación. Dicha aplicación solicitará la información de tarjetahabientes y cuentas y entregará los resultados a las partes interesadas (aplicaciones web, aplicaciones de escritorio, IVRs, archivos, etc).

El proceso para solicitar la información es muy sencillo:

1. Identificarse como una aplicación reconocida por los sistemas de Processa.
2. Invocar la operación deseada.

Para solicitar el registro de una aplicación debe comunicarse con el área comercial de Processa. Ellos le facilitaran la información necesaria para generar los valores de identificación de su aplicación. Finalmente obtendrá dos valores que en adelante llamaremos `appKey` y `appSecret`.

| Nombre        | Descripción |
| ------------- |-------------
| `appKey`      | Identificador unívoco de su aplicación |
| `appSecret`   | Valor a utilizar en la [generación del JSON Web Token](/JWT.md) |

Ejemplo de un valor `appKey`

```AsciiDoc
5f469578-995f-4ae1-8280-578c59afbb84
```

Ejemplo de un valor `appSecret`

```AsciiDoc
T+D526CiB1Un52uRMW2l2yt0vDXUVWCJ6yH1TFxRz8GXd4Byq4gbPy3lS4h4s9B8gbEMzohZLnskNs1xbMD3UwBhzSZXtbZefC25/o9rutkYMJA6Vpq3zzhmcNb8Uq0PVEegu6jcOIKeouchLzjH9zdvXTyHuXxWbpN4LnWE5H468z5UmQ5EX+379N0sWm4WYAO8XHGx+WRMDsLMDElFg9iEMhsqmebIe7aDPytAqczsRJOsKaanKRVS4ZPMmJpw
```

<div class="admonition warning">
   <p class="first admonition-title">Atención</p>
   <p class="last">Mantenga en un lugar seguro la información de identificación. Si en algún momento considera que la privacidad de la información se ha comprometido, solicite el reinicio de dicha información. Esto invalidará de forma inmediata la información comprometida.
   </p>
</div>
