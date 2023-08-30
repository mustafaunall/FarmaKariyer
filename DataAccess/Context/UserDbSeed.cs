using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class UserDbSeed
    {
        private readonly ModelBuilder modelBuilder;
        public UserDbSeed(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }

        public void Seed()
        {
            // TODO: DbSeed
        }
    }
}
