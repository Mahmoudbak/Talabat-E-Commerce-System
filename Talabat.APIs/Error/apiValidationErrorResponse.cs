using Microsoft.AspNetCore.Mvc;

namespace Talabat.APIs.Error
{
    public class apiValidationErrorResponse: ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public apiValidationErrorResponse()
            :base(400)
        {
            Errors = new List<string>();

        }

    }
}
