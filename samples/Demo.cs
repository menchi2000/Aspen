void Main()
{
	// LINQPad => https://www.linqpad.net/
	IAspenService client = new AspenService(autoSign:true);
	LINQPad.Extensions.Dump(client.GetDocTypes());
}

public class AspenService : IAspenService
{
	// Reemplace este valor por la URL pública del servicio
	private const string ServiceEndpoint = "http://localhost/api";
	private const string AppHeaderKey = "X-PRO-Auth-App";
	private const string PayloadHeaderKey = "X-PRO-Auth-Payload";
	
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

	public AspenService(ICredentials credentials = null, bool autoSign = false)
	{
		if (credentials == null)
		{
			credentials = new HardCodedCredentials();
		}
		
		this.appKey = credentials.AppKey;
		this.appSecret = credentials.AppSecret;
		this.restClient = new RestClient(ServiceEndpoint);
		this.encoder = new JwtEncoder(this.algorithm, this.serializer, this.urlEncoder);
		this.validator = new JwtValidator(this.serializer, this.datetimeProvider);
		this.decoder = new JwtDecoder(this.serializer, this.validator, this.urlEncoder);
		
		if (autoSign)
		{
			this.Signin();
		}
	}

	public void Signin()
	{
		var payload = new Dictionary<string, object>
		{
			{ "Nonce", this.nonceProvider.GetNonce() },
			{ "Epoch", this.epochProvider.GetSeconds() }
		};

		IRestRequest request = new RestRequest("/app/auth/signin", Method.POST);
		request.AddHeader(AppHeaderKey, this.appKey);
		request.AddHeader(PayloadHeaderKey, this.encoder.Encode(payload, this.appSecret));
		IRestResponse response = this.restClient.Execute(request);
		if (response.IsSuccessful)
		{
			string jsonResponse = this.decoder.Decode(response.Content, this.appSecret, true);
			this.AuthContext = JsonConvert.DeserializeObject<AuthContext>(jsonResponse);
			return;
		}

		throw new AspenException(response);
	}

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
		request.AddHeader(AppHeaderKey, this.appKey);
		request.AddHeader(PayloadHeaderKey, this.encoder.Encode(payload, this.appSecret));
		IRestResponse response = this.restClient.Execute(request);
		if (response.IsSuccessful)
		{
			return JsonConvert.DeserializeObject<List<DocType>>(response.Content);
		}

		throw new AspenException(response);
	}
}

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

internal class UnixTimeJsonConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(DateTime);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(reader.Value)).DateTime.ToLocalTime();
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

public class DocType
{
	public string ShortName { get; set; }
	public string Name { get; set; }
	public int Id { get; set; }
	public int Order { get; set; }
}

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
		this.Data["Uri"] = response.ResponseUri.ToString();

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

public interface ICredentials
{
	string AppKey { get; }
	string AppSecret { get; }
}

public interface IAspenService
{
	AuthContext AuthContext {get;}
	void Signin();
	IList<DocType> GetDocTypes();
}

public class HardCodedCredentials : ICredentials
{
	public string AppKey => "MyAppKey";
	public string AppSecret => "MyAppSecret";
}

public interface INonceProvider
{
	string GetNonce();
}

public interface IEpochProvider
{
	double GetSeconds();
}

public class GuidNonceProvider : INonceProvider
{
	public string GetNonce()
	{
		return Guid.NewGuid().ToString();
	}
}

public class UnixEpochProvider : IEpochProvider
{
	public double GetSeconds()
	{
		return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
	}
}
