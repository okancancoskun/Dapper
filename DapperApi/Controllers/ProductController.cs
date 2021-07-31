using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DapperApi.Models;
using DapperApi.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace DapperApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductRepository productRepository;
        private IMemoryCache _cache;
        public ProductController(IMemoryCache cache)
        {
            productRepository = new ProductRepository();
            _cache = cache;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            IEnumerable<Product> data;
            string cacheKey = "data";
            if (!_cache.TryGetValue(cacheKey, out data))
            {
                data = productRepository.GetAll();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30)).SetAbsoluteExpiration(TimeSpan.FromMinutes(1)).SetPriority(CacheItemPriority.Normal);
                _cache.Set(cacheKey, data, cacheEntryOptions);
            }
            return Ok(data);
        }
        [HttpPost("create")]
        public void Add([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.Add(product);
            }
        }
        [HttpGet("{id}")]
        public ActionResult<Product> FindById(int id)
        {
            Product data;
            string cacheKey = "singleData";
            if (!_cache.TryGetValue(cacheKey, out data))
            {
                data = productRepository.FindById(id);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30)).SetAbsoluteExpiration(TimeSpan.FromMinutes(1)).SetPriority(CacheItemPriority.Normal);
                _cache.Set(cacheKey, data, cacheEntryOptions);
            }
            return Ok(data);
        }
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Product prodcut)
        {
            prodcut.id = id;
            if (ModelState.IsValid)
            {
                productRepository.Update(prodcut);
            }
            return NoContent();
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            productRepository.Delete(id);
        }
        [HttpGet("specific")]
        public IEnumerable<SpecificProductColumns> specific()
        {
            return productRepository.getSpecificField();
        }
        [HttpGet("apicall")]
        public async Task<List<Post>> CallRestApi()
        {
            return await productRepository.CallRestApi();
        }
        [HttpGet("combineTables")]
        public IEnumerable<CombinedTables> combineTables()
        {
            return productRepository.combineTables();
        }
        [HttpGet("expando")]
        public dynamic GetExpando()
        {
            return productRepository.PassDataToExpando();
        }
    }
}