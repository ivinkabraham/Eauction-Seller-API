using Eauction_Seller_API.DataAccess.Utility;
using Eauction_Seller_API.Models;
using Eauction_Seller_API.Models.Enum;
using Eauction_Seller_API.Repositories;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace Eauction_Seller_API.DataAccess
{
    public class CosmosSellerAdapter: ICosmosSellerAdapter
    {
        //private readonly DocumentClient _client;
        //private readonly string _accountUrl;
        //private readonly string _primarykey;

        private readonly ISellerRepository _repository;
        private readonly ICacheAdapter _cacheService;
        public CosmosSellerAdapter(
         ICosmosConnection connection,
         IConfiguration config,
         ISellerRepository sellerRepository, ICacheAdapter cacheService)
        {
            _repository = sellerRepository;
            //_accountUrl = config.GetValue<string>("Cosmos:AccountURL");
            //_primarykey = config.GetValue<string>("Cosmos:AuthKey");
            //_client = new DocumentClient(new Uri(_accountUrl), _primarykey);
            _cacheService = cacheService;
        }


        //public async Task<bool> CreateDatabase(string name)
        //{
        //    try
        //    {
        //        await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = name });
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> CreateCollection(string dbName, string name)
        //{
        //    try
        //    {
        //        await _client.CreateDocumentCollectionIfNotExistsAsync
        //         (UriFactory.CreateDatabaseUri(dbName), new DocumentCollection { Id = name });
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> CreateDocument(string dbName, string name, SellerInfo sellerInfo)
        //{
        //    try
        //    {
        //        sellerInfo. = "d9e51c1e-1474-41d1-8f32-96deedd8f36a";
        //        await _client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(dbName, name), sellerInfo);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

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
            //Reading from Redis Cache
            var prodInfo = await _cacheService.Get<Product>(productId);
            if (prodInfo == null)
            {
                prodInfo = await _repository.GetProduct(productId);
                if (prodInfo != null)
                    await _cacheService.Set<Product>(productId, JsonConvert.SerializeObject(prodInfo));
            }
            bidsDetails.ProductInfo = prodInfo;
            bidsDetails.BidList = await _repository.GetBids(productId);

            //Read Rabbitmq queue
            //_rabbitMqListener.Receive();

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
