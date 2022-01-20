using Eauction_Seller_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eauction_Seller_API.Repositories
{
    public interface ISellerRepository
    {
        Task<List<Product>> GetAllProducts();

        Task<Product> GetProduct(string productId);

        Task AddProduct(Product product);

        Task<List<Bid>> GetBids(string productId);

        Task DeleteProductAsync(string productId);
    }
}
