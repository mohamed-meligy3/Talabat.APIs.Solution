using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extentions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
   
    public class AccountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountsController(UserManager<AppUser>userManager,SignInManager<AppUser> signInManager,ITokenService tokenService,IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }
        [HttpPost("login")] //api/accounts/login
        public async Task<ActionResult<UserDto>>Login(LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiErrorResponse(401));
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password,false);
            if(!result.Succeeded) return Unauthorized(new ApiErrorResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token =await tokenService.CreateTokenAsync(user, userManager)
            });
        }
        [HttpPost("register")] // api/accounts/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
                
            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(400));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user, userManager)
            });
        }

        //Current User => Login=> Authorize => Token
        [Authorize]
        [HttpGet("currentuser")]//api/Accounts/currentuser
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);

            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user,userManager)
            });
        }

        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await userManager.FindUserWithAddressAsync(User);

            var address = mapper.Map<Address, AddressDto>(user.Adress);
            return Ok(address);
        }


      


    }
}
