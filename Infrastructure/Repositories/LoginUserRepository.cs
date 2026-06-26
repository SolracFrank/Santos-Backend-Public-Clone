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

        public async Task<LoginUser?> GetByIdAsync(int idLoginUser)
        {
            return await _context.LoginUsers
                .FirstOrDefaultAsync(x => x.Idloginuser == idLoginUser);
        }

        public async Task<List<LoginUser>> GetAllAsync()
        {
            return await _context.LoginUsers
                .OrderBy(x => x.Username)
                .ToListAsync();
        }

        public async Task AddAsync(LoginUser user)
        {
            _context.LoginUsers.Add(user);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LoginUser user)
        {
            _context.LoginUsers.Update(user);

            await _context.SaveChangesAsync();
        }
    }
}