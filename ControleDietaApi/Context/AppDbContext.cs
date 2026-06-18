using ControleDietaApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace ControleDietaApi.Context;

public class AppDbContext : IdentityDbContext<UserToken>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<MeatGoal> MeatGoals { get; set; }
}
