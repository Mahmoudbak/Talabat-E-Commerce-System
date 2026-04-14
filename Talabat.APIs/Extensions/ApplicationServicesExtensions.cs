using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.APIs.Error;
using Talabat.APIs.Helper;
using Talabat.Core;
using Talabat.Core.Entity.Identity;
using Talabat.Core.Repository.content;
using Talabat.Core.Services.Content;
using Talabat.Repsotiory;
using Talabat.Repsotiory._Identity;
using Talabat.Repsotiory.Data;
using Talabat.Services.OrderService;
using Talabat.Services.PaymentService;
using Talabat.Services.ProductService;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            // Add services to the container.

            #region Configer Services

            // عشان يفضل يكون ليا Access عليه
            services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheService));

            services.AddScoped(typeof(IPaymentServices), typeof(PaymentService));

            services.AddScoped(typeof(IOrderService), typeof(OrderService));

            services.AddScoped(typeof(IProductService), typeof(ProductService));

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

           // services.AddScoped(typeof(IGenaricrepository<>), typeof(GenericRepository<>));

            //more readable 
            //builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile));
            services.AddAutoMapper(typeof(MappingProfile));
            #region use all Project using for handiler the error and validaion
            services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (ActionContext) =>
                {       //هنا موجود كل الاخطاء يتاعتك علشان اطلع منه
                    var Error = ActionContext.ModelState.Where(p => p.Value.Errors.Count() < 0)
                                                       .SelectMany(p => p.Value.Errors)
                                                       .Select(e => e.ErrorMessage)
                                                       .ToArray();
                    var respose = new apiValidationErrorResponse()
                    {
                        Errors = Error
                    };
                    return new BadRequestObjectResult(respose);
                };
            });

            #endregion
            #endregion
            return services;// علشان نقدر نعملadd ل اي حاجه




        }

        public static IServiceCollection AddAuthServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(Options => {

                //password re
            })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>();


            services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                            .AddJwtBearer(Options =>
                            Options.TokenValidationParameters = new TokenValidationParameters()
                            {
                                ValidateIssuer = true,
                                ValidIssuer = configuration["JWT:ValidIssuer"],
                                ValidateAudience = true,
                                ValidAudience = configuration["JWT:ValidAudience"],
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:AuthKey"] ?? string.Empty)),
                                ValidateLifetime = true,
                                ClockSkew = TimeSpan.Zero
                            });
            return services;
        }
    }
}
