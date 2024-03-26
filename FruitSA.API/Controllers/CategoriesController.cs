using FruitSA.API.Models;
using FruitSA.Model;
using Microsoft.AspNetCore.Mvc;

namespace FruitSA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            try
            {
                return Ok(await categoryRepository.GetCategories());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving Categories.");
            }
        }

        [HttpGet("id:int")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            try
            {
                var result = await categoryRepository.GetCategoryById(id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving the Category.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateCategory([FromForm] Category Category)
        {
            try
            {
                if (Category == null)
                {
                    return BadRequest();
                }
                var createdCategory = await categoryRepository.AddCategory(Category);
                return CreatedAtAction(nameof(createdCategory), new { id = createdCategory.CategoryId }, createdCategory);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error inserting the Category.");
            }
        }

        [HttpPut("id:int")]
        public async Task<ActionResult<Category>> UpdateCategory(int id, Category Category)
        {
            try
            {
                if (id != Category.CategoryId)
                {
                    return BadRequest("ID Mismatch.");
                }

                var categoryToUpdate = await categoryRepository.GetCategoryById(id);

                if (categoryToUpdate == null)
                {
                    return NotFound($"There is no Category with ID: {id}");
                }

                return await categoryRepository.UpdateCategory(Category);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot update this Category at the moment.");
            }
        }

        [HttpDelete("id:int")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            try
            {
                var categoryToDelete = await categoryRepository.GetCategoryById(id);
                if (categoryToDelete == null)
                {
                    return NotFound($"You cannot delete Category with ID: {id}");
                }

                return await categoryRepository.DeleteCategory(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"You cannot delete Category with ID: {id}");

            }
        }
    }
}
