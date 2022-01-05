using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
      private  DataContext _context {get; set;}
        private  ITokenService _tokenService { get; set;}
        public IMapper _mapper { get; }

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
           _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")] // httpPost salje podatke 
        public async Task<ActionResult<UserDto>> Register(RegisterDto RegisterDto) // sa ActionResult mozemo raditi neke http result funkcije kao npr BadRequest
        {
           if(await UserExists(RegisterDto.username)) return BadRequest("Username exists !");
            
            var user = _mapper.Map<AppUsers>(RegisterDto);

            using var hmc = new HMACSHA512();

            
             user.UserName = RegisterDto.username.ToLower();
             user.PasswordHash = hmc.ComputeHash(Encoding.UTF8.GetBytes(RegisterDto.password));
             user.PasswordSalt = hmc.Key;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();  // kada se radi sa thredovima uvjek ima ova async funkcija

            return new UserDto{
               username = user.UserName,
               Token = _tokenService.CreateToken(user),
               KnownAs = user.KnownAs     
            };
        }


      [HttpPost("login")] // httpPost salje podatke 
        public async Task<ActionResult<UserDto>> Login(loginDto loginDto)
        {
          var user = await _context.Users.Include(p => p.Photos)
          .SingleOrDefaultAsync(x => x.UserName == loginDto.username);

          if(user == null)
          return Unauthorized("Invalid user name !");
       
          var hmc = new HMACSHA512(user.PasswordSalt);
          var computerHash = hmc.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));

          for(int i = 0;i <computerHash.Length; i++)
        {
            if(computerHash[i] != user.PasswordHash[i]) return Unauthorized("invalid password");      
        }
             return new UserDto{
               username = user.UserName,
               Token = _tokenService.CreateToken(user),
               PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
               KnownAs = user.KnownAs
            };
        }

        private async Task<bool> UserExists (string username)
       {
          return await _context.Users.AnyAsync(x => x.UserName == username.ToLower()); //provjera postoji li vec to ime u bazi sa lambda funckijom
       }    
       
    }
}