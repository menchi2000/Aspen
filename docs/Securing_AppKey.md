# Escenarios comunes al trabajar con claves API

- Se debe proteger del mismo modo que una contraseña.

- Cada integración (aplicación) debe tener su propia clave API. Esta clave es generada dinámicamente por el sistema, asegurando su [unicidad](https://es.wikipedia.org/wiki/Unicidad).

- El valor de secret que genera de forma automática el sistema, es lo suficientemente complejo como para no ser descifrado fácil/rápidamente en un [ataque de fuerza bruta](https://es.wikipedia.org/wiki/Ataque_de_fuerza_bruta).

- Nunca exponga su clave API al público (como en capturas de pantalla).

- Nunca ["queme"](https://es.wikipedia.org/wiki/Hard_code) los valores de su clave API en el código fuente de su sistema. Recuerde que cualquier binario es susceptible  de ser [decompilado](https://es.wikipedia.org/wiki/Decompilador).

- Nunca envié una clave API por correo electrónico.

- Nunca integre en su [sistema de administración de versiones](https://es.wikipedia.org/wiki/Control_de_versiones) una clave API.

- En cualquier momento puede revocar el uso de una clave API estableciendo un nuevo valor para el secret. Si necesitará deshabilitar completamente su uso, póngase en contacto con nosotros.

