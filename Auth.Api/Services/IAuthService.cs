namespace Auth.Api.Services;

public interface IAuthService
{
    Task<bool> RoleExistsAsync(string role);
    Task CreateRoleAsync(string role);
    Task AssignRoleToUser(string role);
    Task ToggleReportsAuthorization();
    Task<bool> CanViewReports();
}