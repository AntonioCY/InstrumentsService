namespace InstrumentsService.Api.Errors;

public class ApiResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public ApiResponse(int StatusCode, string? message = null)
    {
        this.StatusCode = StatusCode;
        Message = message ?? DefaultStatusCodeMessage(StatusCode);
    }
    private static string DefaultStatusCodeMessage(int StatusCode)
    {
        return StatusCode switch
        {
            400 => "A bad request you have made",
            401 => "You have not Authorized",
            404 => "Resource wasn't Found",
            500 => "Interal Server errors",
            0 => "Some Thing Went Wrong",
            _ => throw new NotImplementedException()
        };
    }
}
