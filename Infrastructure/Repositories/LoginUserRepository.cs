using Domain.Common.Security;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class LoginUserRepository : ILoginUserRepository
    {
        private readonly AppDbContext _context;

        public LoginUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoginUser?> GetByUsernameAsync(string username)
        {
            return await _context.LoginUsers
                .SingleOrDefaultAsync(x => x.Username == username);
        }

        public Task<LoginUser?> GetByIdAsync(int idLoginUser)
        {
            throw new NotImplementedException();
        }

        public Task<List<LoginUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(LoginUser user)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(LoginUser user)
        {
            _context.LoginUsers.Update(user);

            await _context.SaveChangesAsync();
        }
    }
}