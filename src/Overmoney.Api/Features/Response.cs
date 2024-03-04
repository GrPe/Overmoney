namespace Overmoney.Api.Features;

public sealed record Response(bool Success, string Message)
{
    public static implicit operator bool(Response response) => response.Success;

    public Response(bool success) : this(success, string.Empty) { }

    public static Response SuccessResponse => new(true);
    public static Response FailureResponse(string message) => new(false, message);
}
