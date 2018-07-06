# Generar un token de autenticación

## Prerequistos

El código que se muestra a continuación se construyó utilizando

- [Visual Studio Community 2017](https://visualstudio.microsoft.com/downloads/)
- [C# 7.1](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-7-1)

Y los paquetes [Nuget](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console)

- Newtonsoft.Json (<https://www.nuget.org/packages/Newtonsoft.Json/11.0.2)>
- JWT (<https://www.nuget.org/packages/JWT/4.0.0)>
- RestSharp (<https://www.nuget.org/packages/RestSharp/106.3.1)>

## Licencia

Este código sé publica cómo material de referencia "TAL CUAL" sin garantías o condiciones de cualquier tipo, ya sean expresas o implícitas.

## Preguntas

¿Tiene alguna pregunta o inquietud con respecto a los ejemplos? Utilice [este formulario](https://github.com/RD-Processa/Aspen/issues)

## Implementación demostrativa

Definamos algunas interfaces para facilitar la compresión e implementación de algunos conceptos:

Necesitaremos un generador de valores [Nonce](JWT-Request/#conceptos-generales), así que definamos una interfaz para esta operación.

```csharp
public interface INonceProvider
{
    string GetNonce();
}
```

Y una clase que genere este valor a partir de un [GUID](https://msdn.microsoft.com/en-us/library/system.guid.aspx).

```csharp
public class GuidNonceProvider : INonceProvider
{
    public string GetNonce()
    {
        return Guid.NewGuid().ToString();
    }
}
```

También necesitaremos calcular el número de segundos transcurridos desde el 1 de enero de 1970 00:00:00 hasta la fecha y hora actual, así que definamos una interfaz para esta operación.

```csharp
public interface IEpochProvider
{
    double GetSeconds();
}
```

Y una clase que implemente dicha operación.

```csharp
public class UnixEpochProvider : IEpochProvider
{
    public double GetSeconds()
    {
        return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
    }
}
```

También necesitaremos los valores de `AppKey` y `AppSecret` que obtuvimos en el [registro de aplicaciones](App_Register) y que nos permiten utilizar las operaciones en **Aspen**, así que definamos una interfaz para esto:

```csharp
public interface ICredentials
{
    string AppKey { get; }
    string AppSecret { get; }
}
```

Estos valores deberían estar almacenados de forma segura, pero para simplificar este ejemplo, vamos a dejarlos ['quemados'](https://es.wikipedia.org/wiki/Hard_code) en una clase (NO HAGA ESTO):

```csharp
public class HardCodedCredentials : ICredentials
{
    public string AppKey => "MyApiKey";
    public string AppSecret => "MyApiSecret";
}
```

La primera operación que se debe invocar en el servicio es solicitar un token de autenticación. Definamos una clase que represente los valores que va a retornar **Aspen**. Utilizaremos algunos atributos en esta clase para relacionar las claves de los elementos JSON de respuesta, con las propiedades de la clase. También haremos uso de un conversor personalizado para las fechas de emisión y expiración del token, ya que **Aspen** las retorna como un número de segundos en formato UTC. El conversor también las ajusta a la fecha y hora local. La hora local es igual al tiempo de hora universal coordinada (UTC) más el desplazamiento UTC.

¿Por qué **Aspen** no retorna estos valores en formato de fecha y hora local? Porque las aplicaciones cliente que invocan las operaciones de **Aspen**, podrían estar ubicadas en una zona horaria diferente a la del servidor que aloja **Aspen**.

```csharp
public class AuthContext
{
    [JsonProperty("jti")]
    public string Token { get; set; }

    [JsonConverter(typeof(UnixTimeJsonConverter))]
    [JsonProperty("exp")]
    public DateTime ExpiresAt { get; set; }

    [JsonConverter(typeof(UnixTimeJsonConverter))]
    [JsonProperty("iat")]
    public DateTime IssueAt { get; set; }

    [JsonIgnore]
    public bool Expired
    {
        get { return DateTime.Now >= this.ExpiresAt; }
    }
}

public class UnixTimeJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime);
    }

    public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
    {
        return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(reader.Value)).DateTime.ToLocalTime();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
```

Ahora, definamos una interfaz para las operaciones que resolverá el servicio. Inicialmente la única operación será `Signin` pero más adelante se podrán agregar otras sin demasiado esfuerzo.

```csharp
public interface IAspenService
{
    AuthContext AuthContext {get;}
    void Signin();
}
```

Finalmente implementemos una clase que invoque el servicio **Aspen** para resolver las operaciones en `IAspenService`.

```csharp
public class AspenService : IAspenService
{
    // Reemplace este valor por la URL pública del servicio
    private const string ServiceEndpoint = "http://localhost/api";
    private string appKey;
    private string appSecret;

    private RestClient restClient;
    private IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
    private IJsonSerializer serializer = new JsonNetSerializer();
    private IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
    private IDateTimeProvider datetimeProvider = new UtcDateTimeProvider();
    private INonceProvider nonceProvider = new GuidNonceProvider();
    private IEpochProvider epochProvider = new UnixEpochProvider();

    private IJwtEncoder encoder;
    private IJwtValidator validator;
    private IJwtDecoder decoder;
    public AuthContext AuthContext { get; private set; }

    public AspenService(ICredentials credentials)
    {
        this.appKey = credentials.AppKey;
        this.appSecret = credentials.AppSecret;
        this.restClient = new RestClient(ServiceEndpoint);
        this.encoder = new JwtEncoder(this.algorithm, this.serializer, this.urlEncoder);
        this.validator = new JwtValidator(this.serializer, this.datetimeProvider);
        this.decoder = new JwtDecoder(this.serializer, this.validator, this.urlEncoder);
    }

    public void Signin()
    {
        var payload = new Dictionary<string, object>
        {
            { "Nonce", this.nonceProvider.GetNonce() },
            { "Epoch", this.epochProvider.GetSeconds() }
        };

        IRestRequest request = new RestRequest("/app/auth/signin", Method.POST);
        request.AddHeader("X-PRO-Auth-App", this.appKey);
        request.AddHeader("X-PRO-Auth-Payload", this.encoder.Encode(payload, this.appSecret));
        IRestResponse response = this.restClient.Execute(request);
        if (response.IsSuccessful)
        {
            string jsonResponse = this.decoder.Decode(response.Content, this.appSecret, true);
            this.AuthContext = JsonConvert.DeserializeObject<AuthContext>(jsonResponse);
            return;
        }

        throw new Exception(response.StatusDescription);
        // throw new AspenException(response);
    }
}
```

Ahora podríamos hacer uso de esta clase así:

```csharp
    IAspenService client = new AspenService(new HardCodedCredentials());
    client.Signin();
    //client.AuthContext;
```

Y en el objeto `client` tendríamos acceso al token de autenticación a través de la propiedad `AuthContext`.

Para facilitar el procesamiento de errores (si se presentaran), extendamos la clase [`Exception`](https://msdn.microsoft.com/en-us/library/system.exception.aspx) con una propia que nos permita interpretar fácilmente la información de errores (cuando se presenten). Esta clase es opcional, aunque también es muy conveniente.

```csharp
public class AspenException : HttpException
{
    public HttpStatusCode StatusCode { get; private set; }
    public string StatusDescription { get; private set; }
    public string EventId { get; private set; }
    private string helpLink;
    private IRestResponse response;

    public AspenException(IRestResponse response)
    {
        this.response = response;
        this.StatusCode = response.StatusCode;
        this.StatusDescription = response.StatusDescription;
        this.helpLink = this.GetResponseHeader(response, "X-PRO-Response-Help");
        this.Data["RequestedUri"] = response.ResponseUri.ToString();

        var responseTime = this.GetResponseHeader(response, "X-PRO-Response-Time");
        if (!string.IsNullOrWhiteSpace(responseTime))
        {
            this.Data["ResponseTime"] = responseTime;
        }
        this.Data["ResponseContentType"] = response.ContentType;
        var match = Regex.Match(response.StatusDescription, @".*EventId\:?\s+\((?<EventId>\d+)\).*");
        {
            if (match.Success)
            {
                this.EventId = match.Groups["EventId"].Value;
            }
        }
    }

    public override string Message => this.StatusDescription;
    public override string HelpLink { get => this.helpLink; set => this.HelpLink = value; }
    public override Exception GetBaseException()
    {
        return this.response.ErrorException;
    }

    private string GetResponseHeader(IRestResponse response, string name)
    {
        return response
            .Headers
            .SingleOrDefault(h => h.Name.Equals(name, StringComparison.OrdinalIgnoreCase))?.Value?
            .ToString();
    }
}
```

Ahora en la clase `AspenService` en lugar de lanzar una simple excepción se puede lanzar una excepción de tipo `AspenException` que tiene mucha más información relevante. Basta con cambiar:

```csharp
throw new Exception(response.StatusDescription);
```

por

```csharp
throw new AspenException(response);
```

Y eso es todo por ahora. En este momento tiene acceso al token de autenticación a través de la propiedad `AuthContext` del objeto en la variable `client`. Ya puede almacenarlo en un lugar seguro, para ser utilizado en las subsecuentes operaciones. Lo haremos en la siguiente sección.
