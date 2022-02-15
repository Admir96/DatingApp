using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private  ITokenService _tokenService { get; set;}
        public IMapper _mapper { get; }
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly UserManager<AppUsers> _userManager;

        public AccountController(UserManager<AppUsers> userManager, SignInManager<AppUsers> SignInManager,
         ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = SignInManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")] // httpPost salje podatke 
        public async Task<ActionResult<UserDto>> Register(RegisterDto RegisterDto) // sa ActionResult mozemo raditi neke http result funkcije kao npr BadRequest
        {
           if(await UserExists(RegisterDto.username)) return BadRequest("Username exists !");
            
            var user = _mapper.Map<AppUsers>(RegisterDto);

            
             user.UserName = RegisterDto.username.ToLower();

            var result = await _userManager.CreateAsync(user,RegisterDto.password);
            
            if(!result.Succeeded) return BadRequest(result.Errors);
            
           var roleResult = await _userManager.AddToRoleAsync(user,"Member");
           
           if(!roleResult.Succeeded) return BadRequest(result.Errors);


            return new UserDto{
               username = user.UserName,
               Token = await _tokenService.CreateToken(user),
               KnownAs = user.KnownAs,
               Gender = user.Gender     
            };
        }


      [HttpPost("login")] // httpPost salje podatke 
        public async Task<ActionResult<UserDto>> Login(loginDto loginDto)
        {
          var user = await _userManager.Users
          .Include(p => p.Photos)
          .SingleOrDefaultAsync(x => x.UserName == loginDto.username.ToLower());

          if(user == null)
          return Unauthorized("Invalid user name !");
          
         var result = await _signInManager
         .CheckPasswordSignInAsync(user,loginDto.password, false);
 
          if(!result.Succeeded) return Unauthorized();

             return new UserDto{
               username = user.UserName,
               Token = await _tokenService.CreateToken(user),
               PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
               KnownAs = user.KnownAs,
               Gender = user.Gender
            };
        }

        private async Task<bool> UserExists (string username)
       {
          return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower()); 
       }    
       
    }
}