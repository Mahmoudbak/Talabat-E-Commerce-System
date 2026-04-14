
namespace Talabat.APIs.Error
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A Bad Requst, You have made",
                401 => "Authorized, You are not Avalid",
                404=>"resorce was not found",
                500=>"Errors are the path to the dark side",
                _ => null
             };
        }
    }

}
