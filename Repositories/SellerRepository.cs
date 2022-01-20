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
            buyerContainer = client.GetContainer(databaseName, "Buyers");
        }

        public async Task<List<Product>> GetAllProducts()
        {
            IQueryable<Product> queryable = container.GetItemLinqQueryable<Product>(true);
            return await Task.FromResult(queryable.ToList());
        }

        public async Task<Product> GetProduct(string productId)
        {
            // var response=   await container.ReadItemAsync<Product>("863ab1c4-5385-499f-b78e-183c9874ea1f", new PartitionKey(productId));
            //  return response.Resource;

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
