
# Introducción

<center>
![API unificada Processa](Aspen-Logo.png)
</center>
  
La API unificada Processa **Aspen** es un servicio de tipo REST con urls predecibles, orientadas a recursos, que utiliza códigos de respuesta HTTP para indicar los resultados de las operaciones. **Aspen** utiliza los verbos HTTP, comúnmente utilizados en clientes HTTP tradicionales en el mercado. **Aspen** soporta [cross-origin resource sharing o CORS](https://en.wikipedia.org/wiki/Cross-origin_resource_sharing), lo que le permite interactuar de forma segura con nuestra API desde casi cualquier aplicación del lado del cliente. **Aspen** utiliza JSON para los valores que representan entradas y salidas, incluidos los posibles errores.

**Aspen** valida que las solicitudes entrantes vengan _firmadas_ con claves de API asociadas con una aplicación. Si no incluye su clave al hacer una solicitud al API, o si utiliza una que sea incorrecta,  desactualizada o bloqueada, **Aspen** devolverá un error.

Entre otras operaciones **Aspen** permite:

- Consultar saldos y movimientos de una cuenta

- Actualizar la información del PIN transaccional de una cuenta

- Bloquear cuentas

- Y muchas otras más
