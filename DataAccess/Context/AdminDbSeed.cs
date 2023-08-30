using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public class AdminDbSeed
{
    private readonly ModelBuilder modelBuilder;
    public AdminDbSeed(ModelBuilder modelBuilder)
    {
        this.modelBuilder = modelBuilder;
    }

    public void Seed()
    {
        // TODO: DbSeed
    }
}
