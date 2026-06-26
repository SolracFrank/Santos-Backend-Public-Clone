using Domain.Entities.Dtos;
using System.Threading.Tasks;
namespace Domain.Common.Security
{
    public interface IAuthenticationService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}