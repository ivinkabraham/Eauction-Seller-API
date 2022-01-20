using Eauction_Seller_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eauction_Seller_API.DataAccess
{
    public interface ICosmosSellerAdapter
    {
        //Task<SellerInfo> UpsertSellerAsync(SellerInfo seller);

        //Task<bool> CreateDatabase(string name);
        //Task<bool> CreateCollection(string dbName, string name);
        //Task<bool> CreateDocument(string dbName, string name, SellerInfo sellerInfo);

        Task<List<Product>> GetAllProducts();
        Task AddProduct(Product product);
        Task<BidDetails> GetBids(string productId);
        Task DeleteProductAsync(string productId);
    }
}
