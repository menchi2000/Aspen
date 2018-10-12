# Cambiar el secreto de una aplicación

**Aspen** permite al propietario de una aplicación, cambiar o actualizar el secreto para firmar las solicitudes.

Cambiar el secreto de su aplicación al igual que en la [solicitud de un token de autenticación](JWT-Request.md) necesitará de las cabeceras de autenticación personalizadas junto con un parámetro que recibe el valor para el nuevo secreto.

Tendrá que invocar a la operación `Secret` del servicio de autenticación, haciendo un `POST` y agregando las dos [cabeceras personalizadas](JWT-Request.md#cabeceras-de-autenticacion-requeridas) usando su `AppKey` y `AppSecret` actual.

### Implementación demostrativa

Usando el [código de demostración](samples/Demo.cs) agregaremos a la interfaz `IAspenService` una nueva función con el nombre `UpdateSecret`

```c#
public interface IAspenService
{
    AuthContext AuthContext { get; }
    void Signin();
    IList<DocType> GetDocTypes();
    void UpdateSecret(string newSecret);
}
```

Y en la clase `AspenService` implementaremos la nueva función:

```c#
public void UpdateSecret(string newSecret)
{
    var payload = new Dictionary<string, object>
    {
        { "Nonce", this.nonceProvider.GetNonce() },
        { "Epoch", this.epochProvider.GetSeconds() }
    };

    IRestRequest request = new RestRequest("/app/auth/secret", Method.POST);
    request.AddHeader(AppHeaderKey, this.appKey);
    request.AddHeader(PayloadHeaderKey, this.encoder.Encode(payload, this.appSecret));
    request.AddParameter("NewValue", newSecret);
    IRestResponse response = this.restClient.Execute(request);
    if (response.IsSuccessful)
    {
        return;
    }

    throw new AspenException(response);
}
```

<div class="admonition warning">
   <p class="first admonition-title">Atención</p>
   <p class="last">Una vez que el cambio de secreto haya finalizado con éxito, toda nueva solicitud firmada usando las credenciales anteriores, automáticamente será rechazada.</p>
</div>
 
### Características de un secreto fuerte
El secreto debe cumplir con un alto nivel de complejidad para que sea poco o nada predecible y mitigar ataques de fuerza bruta.

Prepare nuevos secretos para su aplicación que incluyan las siguientes características:

* Letras minúsculas.
* Letras mayúsculas.
* Números.
* Caracteres especiales.
* Una longitud mínima 128 caracteres.

```c#
// Una forma sencilla de generar claves fuertes con .NET:
int minLength = 128;
int numberOfNonAlphanumericCharacters = 10;
System.Web.Security.Membership.GeneratePassword(minLength, numberOfNonAlphanumericCharacters);
```

O puede utilizar una herramienta en línea para generar claves seguras como esta: [Strong Random Password Generator](https://passwordsgenerator.net/)
