using Market.Abstract;
using Market.Models;
using Market.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CategoryController: ControllerBase
    {
        private readonly ICategoryRepo _categoryRepo;

        public CategoryController(ICategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [HttpPost("addCategory")]
        public IActionResult AddCategory([FromBody] CategoryDto categoryDto)
        {
            var result = _categoryRepo.AddCategory(categoryDto);
            return Ok(result);
        }

        [HttpGet("getCategories")]
        public IActionResult GetCategories()
        {
            var result = _categoryRepo.GetCategories();
            return Ok(result);
        }

        [HttpDelete("deleteCategory")]
        public IActionResult DeleteCategory([FromQuery] int id)
        {
            try
            {
                using (var context = new MarketContext())
                {
                    if (!context.Categories.Any(x => x.Id == id))
                    {
                        return NotFound();
                    }

                    Category category = context.Categories.FirstOrDefault(x => x.Id == id)!;
                    context.Categories.Remove(category);
                    context.SaveChanges();

                    return Ok();
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("GetCategoriesCSV")]
        public FileContentResult GetCSV()
        {
            var content = _categoryRepo.GetCategoriesCSV();

            return File(new System.Text.UTF8Encoding().GetBytes(content), "text/csv", "Categories.csv");
        }

        [HttpGet("GetCacheCSV")]
        public ActionResult<string> GetCacheCSV()
        {
            var result = _categoryRepo.GetСacheStatCSV();

            if (result is not null)
            {
                var fileName = $"Categories_{DateTime.Now.ToBinary()}.csv";

                System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName), result);

                return "https://" + Request.Host.ToString() + "/static/" + fileName;
            }
            return StatusCode(500);
        }
    }
}
