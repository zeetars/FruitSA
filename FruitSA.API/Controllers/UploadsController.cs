using FruitSA.API.Models;
using FruitSA.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FruitSA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UploadsController: ControllerBase
    {
        private readonly IUploadRepository uploadRepository;

        public UploadsController(IUploadRepository uploadRepository)
        {
            this.uploadRepository = uploadRepository;
        }

        [HttpPost]
        public async Task<ActionResult> CreateProducts(List<Product> Products)
        {
            try
            {
                if (Products == null)
                {
                    return BadRequest();
                }
                await uploadRepository.AddProducts(Products);
                return Ok();
            }
            catch (Exception)
            {
               return  StatusCode(StatusCodes.Status500InternalServerError, "Error inserting the Products.");
            }
        }
    }
}
