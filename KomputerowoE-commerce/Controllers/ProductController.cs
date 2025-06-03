using KomputerowoE_commerce.Models;
using KomputerowoE_commerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KomputerowoE_commerce.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.product.ToListAsync();
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<Product>> GetSignleProductsById(int id)
        {
            var product = await _context.product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return product;
            }
        }
        [HttpGet("name/{name}")]
        public async Task<ActionResult<List<Product>>> GetSignleProductsByName(string name)
        {
            var product = await _context.product
                                            .Where(p => p.name == name)
                                            .ToListAsync();
            if (!product.Any())
            {
                return NotFound();
            }
           
                return product;
            
        }
    }
}
