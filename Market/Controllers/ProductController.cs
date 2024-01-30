using Market.Abstract;
using Market.Models;
using Market.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController: ControllerBase
    {
        private readonly IProductRepo _productRepo;

        public ProductController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet("getProduct")]
        public IActionResult GetProducts()
        {
            var products = _productRepo.GetProducts();
            return Ok(products);
        }

        [HttpPost("addProduct")]
        public IActionResult AddProduct([FromBody] ProductDto productDto)
        {
            var result = _productRepo.AddProduct(productDto);
            return Ok(result);
        }

        [HttpDelete("deleteProduct")]
        public IActionResult DeleteProduct([FromQuery] int id)
        {
            try
            {
                using (var context = new MarketContext())
                {
                    if (!context.Products.Any(x => x.Id == id))
                    {
                        return NotFound();
                    }

                    Product product = context.Products.FirstOrDefault(x => x.Id == id)!;
                    context.Products.Remove(product);
                    context.SaveChanges();

                    return Ok();
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("addProductPrice")]
        public IActionResult AddProductPrice([FromQuery] int id, int price)
        {
            try
            {
                using (var context = new MarketContext())
                {
                    if (!context.Products.Any(x => x.Id == id))
                    {
                        return NotFound();
                    }

                    Product product = context.Products.FirstOrDefault(x => x.Id == id)!;
                    product.Cost = price;
                    context.SaveChanges();

                    return Ok();
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetProductsCSV")]
        public FileContentResult GetCSV()
        {
            var content = _productRepo.GetProductsCSV();

            return File(new System.Text.UTF8Encoding().GetBytes(content), "text/csv", "Products.csv");
        }

        [HttpGet("GetCacheCSVUrl")]
        public ActionResult<string> GetCacheCSVUrl()
        {
            var result = _productRepo.GetСacheStatCSV();

            if (result is not null)
            {
                var fileName = $"products{DateTime.Now.ToBinary()}.csv";

                System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName), result);

                return "https://" + Request.Host.ToString() + "/static/" + fileName;
            }

            return StatusCode(500);
        }
    }
}
