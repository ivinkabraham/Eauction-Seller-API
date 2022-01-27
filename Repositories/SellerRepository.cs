using Eauction_Seller_API.Models;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eauction_Seller_API.Repositories
{
    public class SellerRepository :ISellerRepository
    {
        private Container container, buyerContainer;
        public SellerRepository(CosmosClient client, string databaseName, string containerName)
        {
            container = client.GetContainer(databaseName, containerName);
            buyerContainer = client.GetContainer(databaseName, "eauction-buyer-collection");
        }

        public async Task<List<Product>> GetAllProducts()
        {
            IQueryable<Product> queryable = container.GetItemLinqQueryable<Product>(true);
            return await Task.FromResult(queryable.ToList());
        }

        public async Task<Product> GetProduct(string productId)
        {
            IQueryable<Product> queryable = container.GetItemLinqQueryable<Product>(true);
            queryable = queryable.Where(item => item.Id == productId);
            return await Task.FromResult(queryable.ToArray().FirstOrDefault());
        }

        public async Task AddProduct(Product product)
        {
            await container.CreateItemAsync(product, new PartitionKey(product.Id));
        }

        public async Task DeleteProductAsync(string productId)
        {
            await container.DeleteItemAsync<SellerInfo>(productId, new PartitionKey(productId));
        }

        public async Task<List<Bid>> GetBids(string productId)
        {
            IQueryable<Bid> queryable = buyerContainer.GetItemLinqQueryable<Bid>(true);
            queryable = queryable.Where(item => item.ProductId == productId);
            return await Task.FromResult(queryable.ToList());
        }
    }
}
