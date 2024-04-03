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
        public async Task CreateProducts(List<Product> Products)
        {
            try
            {
                if (Products != null)
                {
                    await uploadRepository.AddProducts(Products);
                }

            }
            catch (Exception)
            {
                StatusCode(StatusCodes.Status500InternalServerError, "Error inserting the Products.");
            }
        }
    }
}
