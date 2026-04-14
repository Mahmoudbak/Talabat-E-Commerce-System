using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Error;
using Talabat.APIs.Extensions;
using Talabat.Core.Entity.Identity;
using Talabat.Core.Services.Content;

namespace Talabat.APIs.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signManager;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signManager, IAuthService authService,IMapper mapper)
        {
            _userManager = userManager; 
            _signManager = signManager;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("Login")] // Post : Api/account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiResponse(401, "Invalid Login"));

            var result = await _signManager.CheckPasswordSignInAsync(user, model.Password, false);


            if (!result.Succeeded) return Unauthorized(new ApiResponse(401, "Invalid Login"));

            return Ok(new UserDto()
            {
                DisplayName = user.DispalyName,
                Email = user.Email ?? string.Empty,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });

        }


        [HttpPost("Register")] //POST : Api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                DispalyName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.Phone
            };
            var result = await _userManager.CreateAsync(user, model.Password);


            if (!result.Succeeded) return BadRequest(new apiValidationErrorResponse() { Errors = result.Errors.Select(E => E.Description) });


            return Ok(new UserDto
            {
                DisplayName = user.DispalyName,
                Email = user.Email ?? string.Empty,
                Token = await _authService.CreateTokenAsync(user, _userManager)

            });
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

            var user = await _userManager.FindByEmailAsync(email);

            return Ok(new UserDto
            {
                DisplayName = user.DispalyName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }



        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {

            var user = await  _userManager.FindUserWithAddressAsync(User);


            return Ok(_mapper.Map<AddressDto>(user.Address));
        }




        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<Address>> UpdateUserAddress(AddressDto address)
        {

            var UpdateAddress = _mapper.Map<Address>(address);

            var user = await _userManager.FindUserWithAddressAsync(User);

            //UpdateAddress.Id = user.Address.Id;

            user.Address = UpdateAddress;


            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest(new apiValidationErrorResponse() { Errors = result.Errors.Select(e => e.Description) });

            return Ok(address);

        }



        //[Authorize]
        //[HttpPut("address")]
        //public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto newAddress)
        //{
        //    var email = User.FindFirstValue(ClaimTypes.Email);
        //    var user = await _userManager.FindByEmailAsync(email);

        //    user.Address = _mapper.Map<AddressDto, Address>(newAddress);

        //    var result = await _userManager.UpdateAsync(user);

        //    if (!result.Succeeded) return BadRequest(new apiValidationErrorResponse() { Errors = new[] { "An error occured during updating the address" } });

        //    return Ok(newAddress);
        //}
    }
}
