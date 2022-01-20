using Eauction_Seller_API.DataAccess;
using Eauction_Seller_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eauction_Seller_API.Controllers
{
    [ApiController]
    [Route("e-auction/api/v1/[controller]")]
    public class SellerController : ControllerBase
    {
        //private static readonly string[] Summaries = new[]
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        private readonly ILogger<SellerController> _logger;
        ICosmosSellerAdapter _adapter;
        public SellerController(ICosmosSellerAdapter adapter,ILogger<SellerController> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        //[HttpGet("createdb")]
        //public async Task<IActionResult> CreateDatabase()
        //{
        //    var result = await _adapter.CreateDatabase("cosmos-db-eauction");
        //    return Ok(result);
        //}

        //[HttpGet("createcollection")]
        //public async Task<IActionResult> CreateCollection()
        //{
        //    var result = await _adapter.CreateCollection("cosmos-db-eauction", "eauction-seller-collection");
        //    return Ok(result);
        //}

        //[HttpPost("CreateDocument")]
        //public async Task<IActionResult> CreateDocument([FromBody] SellerInfo seller)
        //{
        //    var result = await _adapter.CreateDocument("cosmos-db-eauction", "eauction-seller-collection", seller);
        //    return Ok(result);
        //}

        //[HttpGet("get-products")]
        //public async Task<ActionResult> GetProducts()
        //{
        //    _logger.LogInformation("Start fetching Products");

        //    var result = await _adapter.GetAllProducts();
        //    return Ok(result);
        //}

        [HttpGet("get-products")]
        public async Task<ActionResult> GetProducts()
        {
            _logger.LogInformation("Start fetching Products");

            var result = await _adapter.GetAllProducts();
            return Ok(result);
        }

        [HttpGet]
        [Route("show-bids/{productId}")]
        public async Task<ActionResult> Get(string productId)
        {
            _logger.LogInformation("Start fetching Products by id:" + productId);

            var result = await _adapter.GetBids(productId);
            return Ok(result);
        }

        [HttpPost]
        [Route("add-product")]
        public async Task<ActionResult> Post([FromBody] Product product)
        {
            await _adapter.AddProduct(product);
            return Created("add-product", product);
        }

        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            await _adapter.DeleteProductAsync(productId);
            return NoContent();
        }
    }
}
