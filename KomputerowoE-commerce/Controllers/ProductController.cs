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
        //Get methods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.product
                                            .OrderBy(p => p.id)
                                            .ToListAsync();

        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
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
        public async Task<ActionResult<List<Product>>> GetProductByName(string name)
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
        //Post method
        [HttpPost("createproduct")]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            _context.product.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProductById), new { id = product.id}, product);

        }
        //Patch method
        [HttpPatch("updateproduct/id/{id}")]
        public async Task<ActionResult<List<Product>>> UpdateProductOnId([FromBody] Product product, int id)
        {
            var updateProduct = await _context.product.FindAsync(id);
            if (updateProduct == null)
            {
                return NotFound($"Nieznaleziono produktu o id: {id}");
            }
            if (!string.IsNullOrEmpty(product.name))
                updateProduct.name = product.name;

            if (product.price.HasValue)
                updateProduct.price = product.price.Value;

            if (!string.IsNullOrEmpty(product.type))
                updateProduct.type = product.type;

            if (!string.IsNullOrEmpty(product.description))
                updateProduct.description = product.description;

            await _context.SaveChangesAsync();
            return Ok(updateProduct);
        }
        //Delete Method
        [HttpDelete("deleteproduct/id/{id}")]
        public async Task<ActionResult<List<Product>>> DeleteProductOnId(int id)
        {
            var deleteProduct = await _context.product.FindAsync(id);
            if (deleteProduct == null)
            {
                return NotFound($"Nieznaleziono produktu o id: {id}");
            }
            _context.Remove(deleteProduct);

            await _context.SaveChangesAsync();
            return Ok("Produkt został usunięty");
        }
    }
}
