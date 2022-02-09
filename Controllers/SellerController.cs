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
        private readonly ILogger<SellerController> _logger;
        ICosmosSellerService _service;
        public SellerController(ICosmosSellerService adapter,ILogger<SellerController> logger)
        {
            _service = adapter;
            _logger = logger;
        }

        //[HttpGet("get-data")]
        //public async Task<ActionResult> GetData()
        //{
        //    //_logger.LogInformation("Start fetching Products");

        //    //var result = await _adapter.GetAllProducts();
        //    var result = "Test Data";
        //    return Ok(result);
        //}

        [HttpGet("get-products")]
        public async Task<ActionResult> GetProducts()
        {
            _logger.LogInformation("Start fetching Products");

            var result = await _service.GetAllProducts();
            return Ok(result);
        }

        [HttpGet]
        [Route("show-bids/{productId}")]
        public async Task<ActionResult> Get(string productId)
        {
            _logger.LogInformation("Start fetching Products by id:" + productId);

            var result = await _service.GetBids(productId);
            return Ok(result);
        }

        [HttpPost]
        [Route("add-product")]
        public async Task<ActionResult> Post([FromBody] Product product)
        {
            await _service.AddProduct(product);
            return Created("add-product", product);
        }

        [HttpDelete("delete/{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            await _service.DeleteProductAsync(productId);
            return NoContent();
        }
    }
}
