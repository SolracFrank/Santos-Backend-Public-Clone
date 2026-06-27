using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Common.Security
{
    public interface ILoginUserRepository
    {
        Task<LoginUser?> GetByUsernameAsync(string username);

        Task<LoginUser?> GetByIdAsync(int idLoginUser);

        Task<List<LoginUser>> GetAllAsync();

        Task AddAsync(LoginUser user);

        Task UpdateAsync(LoginUser user);
    }
}