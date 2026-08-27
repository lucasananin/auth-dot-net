namespace Auth.Api.Models;

public record MemberViewModel
{
    public bool CanViewReports { get; init; } = false;
}