using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CategoryController : ControllerBase
    {
        private IMemoryCache _cache;
        private readonly CategoryRepository categoryRepository;
        public CategoryController(IMemoryCache cache)
        {
            categoryRepository = new CategoryRepository();
            _cache = cache;
        }
        [HttpGet]
        public IEnumerable<Category> GetAll()
        {
            return categoryRepository.GetAll();
        }
        [HttpPost("create")]
        public void Add([FromBody] Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Add(category);
            }
        }
        [HttpGet("{id}")]
        public ActionResult<Product> FindById(int id)
        {
            Category data;
            string cacheKey = "singleData";
            if (!_cache.TryGetValue(cacheKey, out data))
            {
                data = categoryRepository.FindById(id);
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30)).SetAbsoluteExpiration(TimeSpan.FromMinutes(1)).SetPriority(CacheItemPriority.Normal);
                _cache.Set(cacheKey, data, cacheEntryOptions);
            }
            return Ok(data);
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            categoryRepository.Delete(id);
        }
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Category category)
        {
            category.id = id;
            if (ModelState.IsValid)
            {
                categoryRepository.Update(category);
            }
            return NoContent();
        }
    }
}