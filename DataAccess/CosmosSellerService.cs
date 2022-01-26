using Eauction_Seller_API.Models;
using Eauction_Seller_API.Models.Enum;
using Eauction_Seller_API.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Eauction_Seller_API.MessageSharer;

namespace Eauction_Seller_API.DataAccess
{
    public class CosmosSellerService: ICosmosSellerService
    {
        private readonly ISellerRepository _repository;
        private readonly ICacheService _cacheService;
        //private readonly IRabbitMQListener _rabbitMQListener;
        public CosmosSellerService(
         ISellerRepository sellerRepository, 
         ICacheService cacheService//, 
        // IRabbitMQListener rabbitMQListener
            )
        {
            _repository = sellerRepository;
            _cacheService = cacheService;
           // _rabbitMQListener = rabbitMQListener;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _repository.GetAllProducts();
        }

        public async Task AddProduct(Product product)
        {
            if (product.BidEndDate <= DateTime.Now)
            {
                throw new ArgumentException("Bid End Date should be future date");
            }
            else if (!Enum.IsDefined(typeof(ProductCategory), product.Category))
            {
                throw new ArgumentException("Product Category should be the one from the existing - (Painting, Sculptor, Ornament)");
            }
            product.Id = Guid.NewGuid().ToString();
            await _repository.AddProduct(product);
        }

        public async Task<BidDetails> GetBids(string productId)
        {
            var bidsDetails = new BidDetails();
            //Read from Redis Cache
            var prodInfo = await _cacheService.Get<Product>(productId);
            if (prodInfo == null)
            {
                prodInfo = await _repository.GetProduct(productId);
                if (prodInfo != null)
                    await _cacheService.Set<Product>(productId, JsonConvert.SerializeObject(prodInfo));
            }
            bidsDetails.ProductInfo = prodInfo;
            bidsDetails.BidList = await _repository.GetBids(productId);

            //Read from Rabbitmq queue
           // _rabbitMQListener.Receive();

            return bidsDetails;
        }

        public async Task DeleteProductAsync(string productId)
        {
            var product = await _repository.GetProduct(productId);
            if (product != null)
            {
                if (product.BidEndDate < DateTime.Now)
                {
                    throw new ArgumentException("Product cannot be deleted after the bid end date");
                }
            }
            await _repository.DeleteProductAsync(productId);
        }
    }
}
