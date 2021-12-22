using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UsersRepository : IUserRepository
    {
       private readonly DataContext _Context;
        private readonly IMapper _mapper;

        public UsersRepository( DataContext Context, IMapper mapper)
        {
            _Context = Context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AppUsers>> GetUsersAsync()
        {
           return await _Context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<AppUsers> GetUserByIdAsync(int id)
        {
            return await _Context.Users.FindAsync(id);
        }

        public async Task<AppUsers> GetUserByUsernameAsync(string username)
        {
           return await _Context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<bool> SaveAllAsync()
        {
           return await _Context.SaveChangesAsync() > 0;
        }

        public void Update(AppUsers user)
        {
           _Context.Entry(user).State = EntityState.Modified;
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await _Context.Users.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<MemberDTO> GetMemberAsync(string username)
        {
            return await _Context.Users.Where(x => x.UserName == username)
            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }
    }
}