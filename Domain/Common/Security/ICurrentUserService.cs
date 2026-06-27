namespace Domain.Common.Security
{
    public interface ICurrentUserService
    {
        int IdLoginUser { get; }

        string? Username { get; }

        string? UserRole { get; }

        bool IsAuthenticated { get; }
    }
}