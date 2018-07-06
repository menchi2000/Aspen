# Enviar un token de autenticación

Ahora que tiene un token de autenticación debe enviarlo al invocar cualquier operación en **Aspen**. Lo único que debe hacer es agregarlo al Payload de la solicitud. Así de simple.
Veamos un ejemplo:

Primero creemos una clase que represente la información de un Tipo de identificación en el sistema:

```csharp
public class DocType
{
    public string ShortName { get; set; }
    public string Name { get; set; }
    public int Id { get; set; }
    public int Order { get; set; }
}
```

Agreguemos a las operaciones del servicio una para obtener la lista de tipos de documento del sistema.

```csharp
public interface IAspenService
{
    AuthContext AuthContext {get;}
    void Signin();
    IList<DocType> GetDocTypes();
}
```

Y finalmente agreguemos la operación en la clase `AspenService`.

<div class="admonition warning">
   <p class="first admonition-title">Payload/Token</p>
   <p class="last">Observe que estamos enviando en el Payload el valor del Token.</p>
</div>

```csharp
public IList<DocType> GetDocTypes()
{
    if (this.AuthContext.Expired)
    {
        throw new InvalidOperationException("Token has expired");
    }

    var payload = new Dictionary<string, object>
    {
        { "Nonce", this.nonceProvider.GetNonce() },
        { "Epoch", this.epochProvider.GetSeconds() },
        { "Token", this.AuthContext.Token }
    };

    IRestRequest request = new RestRequest("/app/resx/document-types", Method.GET);
    request.AddHeader("X-PRO-Auth-App", this.appKey);
    request.AddHeader("X-PRO-Auth-Payload", this.encoder.Encode(payload, this.appSecret));
    IRestResponse response = this.restClient.Execute(request);
    if (response.IsSuccessful)
    {
        return JsonConvert.DeserializeObject<List<DocType>>(response.Content);
    }

    throw new AspenException(response);
}
```

Eso es todo. Repita esto cada vez que necesite invocar una operación en el servicio.

```csharp
IAspenService client = new AspenService(new HardCodedCredentials());
client.Signin();
IList<DocType> docTypes = client.GetDocTypes();
```

## Código de demostración completo

<script src="https://gist.github.com/RD-Processa/fea28b6d790fbb06461fa542af24d3b6.js"></script>