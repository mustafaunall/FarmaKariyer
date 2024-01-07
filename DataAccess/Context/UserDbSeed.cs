using Domain.Model;
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
            modelBuilder.Entity<AdvertCategory>().HasData(new AdvertCategory()
            {
                Id = 1,
                QuotaCount = 1,
                Price = 500,
            });
            modelBuilder.Entity<AdvertCategory>().HasData(new AdvertCategory()
            {
                Id = 2,
                QuotaCount = 3,
                Price = 1000,
            });
            modelBuilder.Entity<AdvertCategory>().HasData(new AdvertCategory()
            {
                Id = 3,
                QuotaCount = 10,
                Price = 3000,
            });
        }
    }
}
