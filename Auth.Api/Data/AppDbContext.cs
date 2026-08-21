using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext(options)
{
}