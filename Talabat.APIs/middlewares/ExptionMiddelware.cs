using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using Talabat.APIs.Error;

namespace Talabat.APIs.middlewares
{
   // by convertion Base
    public class ExptionMiddelware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExptionMiddelware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExptionMiddelware(RequestDelegate next, ILogger<ExptionMiddelware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                //take an action with  the request
                await _next.Invoke(httpContext);//Go to next middleware  

                //Take an action with the response
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);   //Development
                //log exception in(DataBase |files) //production Env

                //httpContext.Response.StatusCode = 500; //the is solve
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";


                var response = _env.IsDevelopment() ?
                        new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
                        :
                        new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace);

                var option = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                //convert file to Json        
                var json = JsonSerializer.Serialize(response, option);
                await httpContext.Response.WriteAsync(json);

            }
        }
    }
}
#region ways to create middle ware
//public class ExptionMiddelware:IMiddleware
//{

//    private readonly ILogger<ExptionMiddelware> _logger;
//    private readonly IWebHostEnvironment _env;

//    public ExptionMiddelware(ILogger<ExptionMiddelware> logger, IWebHostEnvironment env)
//    {

//        _logger = logger;
//        _env = env;
//    }


//        public Task InvokeAsync(HttpContext context, RequestDelegate next)
//        {
//            _logger.LogError(ex.Message);   //Development
//                                            //log exception in(DataBase |files) //production Env

//            //httpContext.Response.StatusCode = 500; //the is solve
//            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//            httpContext.Response.ContentType = "application/json";


//            var response = _env.IsDevelopment() ?
//                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
//                    :
//                    new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

//            var option = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

//            //convert file to Json        
//            var json = JsonSerializer.Serialize(response, option);
//            await httpContext.Response.WriteAsync(json);
//        }
//    }
//}
#endregion
