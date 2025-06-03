using KomputerowoE_commerce.Models;
using KomputerowoE_commerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
namespace KomputerowoE_commerce.Controllers
{
        [Route("api/Orders")]
        [ApiController]
        public class OrderController : ControllerBase
        {
            private readonly AppDbContext _context;

            public OrderController(AppDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult> GetProducts()
            {
                var orders = await _context.orders
                    .Select(o => new
                    {
                        o.id,
                        o.customername,
                        ProductNames = o.OrderProducts.Select(op => op.Product.name).ToList()
                    })
                    .ToListAsync();

                return Ok(orders);
            }

        [HttpGet("id/{id}")]
            public async Task<ActionResult<Orders>> GetSignleProductsById(int id)
            {
                var order = await _context.orders
                               .Where(o => o.id == id)
                                .Select(o => new
                                {
                                    o.id,
                                    o.customername,
                                    ProductNames = o.OrderProducts.Select(op => op.Product.name).ToList()
                                })
                                .FirstOrDefaultAsync();
                    if (order == null)
                    {
                        return NotFound();
                    }

                    return Ok(order);
            }
            [HttpGet("customername/{name}")]
            public async Task<ActionResult<List<Orders>>> GetSignleProductsByName(string name)
            {
                var order = await _context.orders
                                                .Where(o => o.customername == name)
                                                .Select(o => new
                                                {
                                                    o.id,
                                                    o.customername,
                                                    ProductNames = o.OrderProducts.Select(op => op.Product.name).ToList()
                                                })
                                                .ToListAsync();
                if (!order.Any())
                {
                    return NotFound();
                }

            return Ok(order);

            }
        }
    }

