using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Text;
using Talabat.Core.Services.Content;

namespace Talabat.APIs.Helper
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;

        public CachedAttribute( int timeToLiveInSeconds )
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //Ask CLR for creating object from "Response Cache Service"  Excplicitly
             var responseCacheService= context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var response = await responseCacheService.GetCachedResponseAsync(cacheKey); 

            if (!string.IsNullOrEmpty(response)) 
            {
                var result= new ContentResult()
                {
                    Content=response,
                    ContentType="application/json",
                    StatusCode=200
                };
                context.Result = result;
                return;
            } //Response is not cached

            var exeutedActionContext = await next.Invoke();  // Will Execute Next Action Filter OR  The Action Itself.

            if (exeutedActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null) // return form object ressult --->controller
            {
                await responseCacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
           // { { url} }/ api / products ? pageIndex = 1 & pageSize = 5 & sort = name

            var keyBuilder = new StringBuilder(); 
            keyBuilder.Append(request.Path);//  /api/products


            // pageIndex = 1
            // pageSize = 5
            // sort = name
            foreach (var  (key,value) in request.Query.OrderBy(x=>x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
                //api/products|pageIndex-1
                //api/products|pageIndex-1|pageSize-5
                //api/products|pageIndex-1|pageSize-5|sort-name
                
            }
                return keyBuilder.ToString();


        }
    }
}
