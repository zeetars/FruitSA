﻿using FruitSA.API.Models;
using FruitSA.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FruitSA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet("{id:int}")]
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
        public async Task<ActionResult<Category>> CreateCategory(Category Category)
        {
            try
            {
                if (Category == null)
                {
                    return BadRequest();
                }
                var createdCategory = await categoryRepository.AddCategory(Category);
                return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.CategoryId }, createdCategory);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error inserting the Category.");
            }
        }

        [HttpPut]
        public async Task<ActionResult<Category>> UpdateCategory(Category Category)
        {
            try
            {
                
                var categoryToUpdate = await categoryRepository.GetCategoryById(Category.CategoryId);

                if (categoryToUpdate == null)
                {
                    return NotFound($"There is no Category with ID: {Category.CategoryId}");
                }

                var updatedCategory = await categoryRepository.UpdateCategory(Category);
                return Ok(updatedCategory);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot update this Category at the moment.");
            }
        }

    }
}
