using Eauction_Seller_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eauction_Seller_API.DataAccess
{
    public interface ICosmosSellerService
    {
        Task<List<Product>> GetAllProducts();
        Task AddProduct(Product product);
        Task<BidDetails> GetBids(string productId);
        Task DeleteProductAsync(string productId);
    }
}
