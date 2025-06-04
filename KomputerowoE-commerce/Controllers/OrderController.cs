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
        //Get Methods
        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            var orders = await _context.orders
                .Select(o => new
                {
                    o.id,
                    o.customername,
                    ProductNames = o.OrderProducts.Select(op => new
                    {
                        Name = op.Product.name,
                        Quantity = op.quantity
                    }).ToList(),
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
                                ProductNames = o.OrderProducts.Select(op => new 
                                { 
                                    Name = op.Product.name,
                                    Quantity = op.quantity
                                }).ToList(),
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
                                                ProductNames = o.OrderProducts.Select(op => new
                                                {
                                                    Name = op.Product.name,
                                                    Quantity = op.quantity
                                                }).ToList(),
                                            })
                                            .ToListAsync();
            if (!order.Any())
            {
                return NotFound();
            }

            return Ok(order);

        }
        //Post method
        [HttpPost("createorder")]
        public async Task<ActionResult<Orders>> CreateAOrder([FromBody] Orders order)
        {
            try
            {
                if (order.orderdate == default)
                {
                    order.orderdate = DateTime.UtcNow;
                }

                //Firstly adding order
                _context.orders.Add(order);
                await _context.SaveChangesAsync();

                //Deleting a duplicates
                order.OrderProducts = order.OrderProducts
                    ?.GroupBy(op => op.productid)
                    .Select(g => g.First())
                    .ToList();

                //Adding connection
                if (order.OrderProducts != null && order.OrderProducts.Any())
                {
                    foreach (var op in order.OrderProducts)
                    {
                        op.orderid = order.id;

                        // if exist
                        bool exists = await _context.orderProduct
                            .AnyAsync(x => x.orderid == op.orderid && x.productid == op.productid);

                        if (!exists)
                        {
                            _context.orderProduct.Add(op);
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                return CreatedAtAction(nameof(GetSignleProductsById), new { id = order.id }, order);
            }
            catch (Exception e)
            {
                // Returning a code 500
                return StatusCode(500, new { message = "Błąd przy tworzeniu zamówienia", details = e.Message });
            }
        }
        //Patch Method
        [HttpPatch("updateorder/id/{id}")]
        public async Task<ActionResult<Orders>> EditProducts(int id, [FromBody] Orders order)
        {
            var existingOrder = await _context.orders
                                                   .Include(o => o.OrderProducts)
                                                   .FirstOrDefaultAsync(o => o.id == id);
            if (existingOrder == null)
                return NotFound();
            if (!string.IsNullOrWhiteSpace(order.customername))
            {
                existingOrder.customername = order.customername;
                await _context.SaveChangesAsync();
            }

            if (order.OrderProducts != null && order.OrderProducts.Any())
            {
                // Delete existing connections
                _context.orderProduct.RemoveRange(existingOrder.OrderProducts);

                // Ad new connections
                foreach (var op in order.OrderProducts)
                {
                    var newOrderProduct = new OrderProduct
                    {
                        orderid = id,
                        productid = op.productid
                    };
                    _context.orderProduct.Add(newOrderProduct);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(existingOrder);
        }
        //Delete Methods
        [HttpDelete("deleteorder/id/{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var existingOrder = await _context.orders
                .Include(o => o.OrderProducts) 
                .FirstOrDefaultAsync(o => o.id == id);

            if (existingOrder == null)
                return NotFound();

            if (existingOrder.OrderProducts != null && existingOrder.OrderProducts.Any())
            {
                _context.orderProduct.RemoveRange(existingOrder.OrderProducts);
            }

            _context.orders.Remove(existingOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("deleteproductorder/id/{id}/productid/{productid}")]
        public async Task<ActionResult> DeleteProductFromOrder(int id,int productid)
        {
            var existingOrder = await _context.orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.id == id);

            if (existingOrder == null)
                return NotFound();

            var productToRemove = existingOrder.OrderProducts
                                                            .FirstOrDefault(op => op.productid == productid);

            if (productToRemove == null)
                return NotFound($"Product with id {productid} not found in order {id}.");

            _context.orderProduct.Remove(productToRemove);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

