using FruitSA.Model;
using System;

namespace FruitSA.API.Models
{
    public class UploadRepository : IUploadRepository
    {
        private readonly AppDbContext appDbContext;

        public UploadRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task AddProducts(List<Product> Products)
        {
            await appDbContext.Products.AddRangeAsync(Products);
            await appDbContext.SaveChangesAsync();

        }

    }
}
