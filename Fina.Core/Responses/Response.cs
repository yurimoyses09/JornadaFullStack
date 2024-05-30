using System.Text.Json.Serialization;

namespace Fina.Core.Responses;

public class Response<TData>
{
    private readonly int _code = Configuration.DefaultStatusCode;
    public string? Message { get; set; }
    public TData? Data { get; set; }

    [JsonConstructor]
    public Response() => _code = Configuration.DefaultStatusCode;

    public Response(TData? data, int code = Configuration.DefaultStatusCode, string? message = null)
    {
        Data = data;
        Message = message;
        _code = code;
    }

    [JsonIgnore]
    public bool IsSuccess => _code >= 200 && _code <= 299; 
}
