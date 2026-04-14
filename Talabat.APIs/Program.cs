using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Talabat.APIs.Error;
using Talabat.APIs.Extensions;
using Talabat.APIs.middlewares;
using Talabat.Application.AuthService;
using Talabat.Core.Entity.Identity;
using Talabat.Core.Repository.contrent;
using Talabat.Core.Services.Content;
using Talabat.Repsotiory._Identity;
using Talabat.Repsotiory.BasketRepository;
using Talabat.Repsotiory.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configer Services

            builder.Services.AddControllers().AddNewtonsoftJson(Options => 
            {
                Options.SerializerSettings.ReferenceLoopHandling=Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddApplicationServices();


            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //builder.Services.AddDbContext<StoreContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            //    // 👇 الإضافة هنا 👇
            //    sqlServerOptions =>
            //    {
            //        sqlServerOptions.EnableRetryOnFailure();
            //    });
            //});
            builder.Services.AddDbContext<ApplicationIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("redis");
                return ConnectionMultiplexer.Connect(connection);
            });

           

            #region This is ADD Auth service before move that of Extaion Method
            //builder.Services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //        .AddJwtBearer(Options =>
            //        Options.TokenValidationParameters = new TokenValidationParameters()
            //        {
            //            ValidateIssuer = true,
            //            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            //            ValidateAudience = true,
            //            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"] ?? string.Empty)),
            //            ValidateLifetime = true,
            //            ClockSkew = TimeSpan.Zero
            //        }); 
            #endregion


            builder.Services.AddAuthServices(builder.Configuration);

            builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepositoy));

            builder.Services.AddApplicationServices();

            builder.Services.AddScoped(typeof(IAuthService ),typeof(AuthService));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", policyOptions =>
                {
                    policyOptions.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseUrl"], builder.Configuration["FrontBaseUrlhttp"]);
                });
            });
            //ApplicationServicesExtensions.AddApplicationServices(builder.Services);//this include Below
            #region comment the services in here and transfare to class ApplicationServicesExetenation


            //emplement objct of generacRepo
            /*builder.Services.AddScoped<IGenaricrepository<Product>,GenericRepository<Product>>();
            builder.Services.AddScoped<IGenaricrepository<ProductBrand>, GenericRepository<ProductBrand>>();
            builder.Services.AddScoped<IGenaricrepository<ProductCategory>, GenericRepository<ProductCategory>>();*/

            //builder.Services.AddScoped(typeof(IGenaricrepository<>), typeof(GenericRepository<>));

            ////more readable 
            ////builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile));
            //builder.Services.AddAutoMapper(typeof(MappingProfile));
            #region use all Project using for handiler the error and validaion
            builder.Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (ActionContext) =>
                {       //هنا موجود كل الاخطاء يتاعتك علشان اطلع منهل
                    var Error = ActionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
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
            #endregion  


            #region Apply All pending Migration [update _database ]And Data Seeding
            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            //Dispose the scope after build-->using

            var services = scope.ServiceProvider;
            //ask CLR For create object From DBcontext explicitly
            var _dbcontext = services.GetRequiredService<StoreContext>();
            var _Identitydbcontext = services.GetRequiredService<ApplicationIdentityDbContext>();

            var loggerfactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbcontext.Database.MigrateAsync();//update Database
                await StoreContextSeed.SeedAsync(_dbcontext);//Data Seed

                await _Identitydbcontext.Database.MigrateAsync();//update Database
                var _UserManger = services.GetRequiredService<UserManager<ApplicationUser>>();
                await ApplicationIdentitycontextSeed.SeedUserAsync(_UserManger);//Data Seed

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                var logger = loggerfactory.CreateLogger<Program>();
                logger.LogError(ex, "an Error in migration ");
            }
            #endregion
            #region Ways of write middle ware
            //    app.Use(async(httpContext,_next)=>try
            //    {
            //        //take an action with  the request
            //        await _next.Invoke(httpContext);//Go to next middleware  

            //        //Take an action with the response
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.LogError(ex.Message);   //Development
            //        //log exception in(DataBase |files) //production Env

            //        //httpContext.Response.StatusCode = 500; //the is solve
            //        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //        httpContext.Response.ContentType = "application/json";


            //        var response = app.Eva.IsDevelopment() ?
            //                new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
            //                :
            //                new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);

            //        var option = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            //        //convert file to Json        
            //        var json = JsonSerializer.Serialize(response, option);
            //        await httpContext.Response.WriteAsync(json);

            //    }
            //})
            #endregion

            #region Configure kestrel middelewares

            
            // use if request the not found or not found that end point
            //this start success because the URL conten Error
            //app.UseStatusCodePagesWithRedirects("/Errors/{0}"); 
            app.UseStatusCodePagesWithReExecute("/Errors/{0}");
            app.UseMiddleware<ExptionMiddelware>();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
            }
                app.MapOpenApi();
                app.UseSwaggerUI(options => options.SwaggerEndpoint(url: "/openapi/v1.json", name: "talabat"));
            app.UseStaticFiles();

            app.UseHttpsRedirection();


            app.UseCors("MyPolicy");



            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();
            app.Run();
            #endregion
        }
    }
}