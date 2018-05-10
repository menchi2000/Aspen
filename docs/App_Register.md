# Registro de aplicaciones

Para poder hacer uso de esta API, se requiere el registro de una aplicación. Dicha aplicación solicitará la información de tarjetahabientes y cuentas y entregará los resultados a las partes interesadas (aplicaciones web, aplicaciones de escritorio, IVRs, archivos, etc). 

El proceso para solicitar la información es muy sencillo:

1. Identificarse como una aplicación reconocida por los sistemas de Processa.
2. Invocar la operación que deseada.

Para solicitar el registro de una aplicación debe comunicarse con el área comercial de Processa. Ellos le entregaran la información necesaria para generar los valores de identificación. Finalmente obtendrá dos valores que en adelante llamaremos `appKey` y `appSecret`. 

| Nombre        | Descripción |
| ------------- |-------------
| `appKey`      | Identificador univoco de su aplicación |
| `appSecret`   | Valor a utilizar en la [generación del JSON Web Token](JWT.md) |     


> Mantenga en un lugar seguro la información de identificación. Si en algún momento considera que la privacidad de la información se ha comprometido, solicite el reinicio de dicha información. Esto invalidará de forma inmediata la información comprometida. 
