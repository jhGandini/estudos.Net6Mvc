using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class IDPDbContext : IdentityDbContext
{
    public IDPDbContext(DbContextOptions<IDPDbContext> options)
        : base(options)
    {
    }
}