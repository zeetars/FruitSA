using FruitSA.API.Models;
using FruitSA.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace FruitSA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ProductsController:ControllerBase
    {
        private readonly IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetProductCount()
        {
            try
            {
                var count = await productRepository.GetProductCount();
                return Ok(count);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving product count.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                //return Ok(await productRepository.GetProducts());
                var products = await productRepository.GetProducts(pageNumber, pageSize);
                return Ok(products);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving Products.");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                var result = await productRepository.GetProductById(id);
                if (result == null) 
                {                
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving the Product.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product Product)
        {
           
            try
            {
                if (Product == null)
                {
                    return BadRequest();
                }
                var createdProduct = await productRepository.AddProduct(Product);
                return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.ProductId }, createdProduct);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error inserting the Product.");
            }
        }

        

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(Product Product)
        {

            try
            {

                var productToUpdate = await productRepository.GetProductById(Product.ProductId);

                if (productToUpdate == null)
                {
                    return NotFound($"There is no Product with ID: {Product.ProductId}");
                }

                return await productRepository.UpdateProduct(Product);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Cannot update this Product at the moment.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            try
            {
                var productToDelete = await productRepository.GetProductById(id);
                if(productToDelete == null)
                {
                    return NotFound($"You cannot delete Product with ID: {id}");
                }

                return await productRepository.DeleteProduct(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"You cannot delete Product with ID: {id}");
                
            }
        }

        

    }
}
