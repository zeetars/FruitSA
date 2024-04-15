using System;
using Xunit;
using Moq;
using AutoFixture;
using FluentAssertions;
using FruitSA.API.Models;
using FruitSA.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using FruitSA.Model;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Http;

namespace FruitSA.API.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Fixture fixture;
        private readonly Mock<IProductRepository> mockProductRepository;
        public ProductsControllerTests()
        {
            fixture = new Fixture();
            mockProductRepository = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task GetProductCount_ShouldReturnOkResponse_WithProductCount()
        {
            // Arrange
            var expectedCount = fixture.Create<int>();

            mockProductRepository
                .Setup(repo => repo.GetProductCount())
                .ReturnsAsync(expectedCount);
            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.GetProductCount();

            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var result = actionResult.Result.As<OkObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().Be(expectedCount);
        }

        [Fact]
        public async Task GetProductCount_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            mockProductRepository
                .Setup(repo => repo.GetProductCount())
                .ThrowsAsync(new Exception("Simulated error"));
            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.GetProductCount();

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>();
            var result = actionResult.Result.As<ObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task GetProductByCode_ShouldReturnOkResponse_WithProduct()
        {
            // Arrange
            var productCode = fixture.Create<string>();
            var expectedResult = fixture.Create<bool>();

            mockProductRepository
                .Setup(repo => repo.GetProductByCode(productCode))
                .ReturnsAsync(expectedResult);
            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.GetProductByCode(productCode);

            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var result = actionResult.Result.As<OkObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().Be(expectedResult);
        }

        [Fact]
        public async Task GetProductByCode_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            var productCode = fixture.Create<string>();

            mockProductRepository
                .Setup(repo => repo.GetProductByCode(productCode))
                .ThrowsAsync(new Exception("Simulated error"));
            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.GetProductByCode(productCode);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>();
            var result = actionResult.Result.As<ObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOkResponse_WithProducts()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;
            var expectedProducts = fixture.CreateMany<Product>();

            mockProductRepository
                .Setup(repo => repo.GetProducts(pageNumber, pageSize))
                .ReturnsAsync(expectedProducts);
            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.GetProducts(pageNumber, pageSize);

            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var result = actionResult.Result.As<OkObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(expectedProducts);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            var pageNumber = 1;
            var pageSize = 10;

            mockProductRepository
                .Setup(repo => repo.GetProducts(pageNumber, pageSize))
                .ThrowsAsync(new Exception("Simulated error"));
            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.GetProducts(pageNumber, pageSize);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>();
            var result = actionResult.Result.As<ObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnOkResponse_WithProduct()
        {
            // Arrange
            var id = fixture.Create<int>();
            var expectedProduct = fixture.Create<Product>();

            mockProductRepository
                .Setup(repo => repo.GetProductById(id))
                .ReturnsAsync(expectedProduct);
            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.GetProductById(id);

            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var result = actionResult.Result.As<OkObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().Be(expectedProduct);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNotFound_WhenProductNotFound()
        {
            // Arrange
            var id = fixture.Create<int>();

            mockProductRepository
                .Setup(repo => repo.GetProductById(id))
                .ReturnsAsync((Product)null);
            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.GetProductById(id);

            // Assert
            actionResult.Result.Should().BeOfType<NotFoundResult>();
            var result = actionResult.Result.As<NotFoundResult>();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            var id = fixture.Create<int>();

            mockProductRepository
                .Setup(repo => repo.GetProductById(id))
                .ThrowsAsync(new Exception("Simulated error"));
            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.GetProductById(id);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>();
            var result = actionResult.Result.As<ObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnCreatedAtAction_WithProduct()
        {
            // Arrange
            var product = fixture.Create<Product>();
            var createdProduct = fixture.Create<Product>();

            mockProductRepository
                .Setup(repo => repo.AddProduct(product))
                .ReturnsAsync(createdProduct);
            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.CreateProduct(product);

            // Assert
            actionResult.Result.Should().BeOfType<CreatedAtActionResult>();
            var result = actionResult.Result.As<CreatedAtActionResult>();
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.ActionName.Should().Be(nameof(ProductsController.GetProductById));
            result.RouteValues["id"].Should().Be(createdProduct.ProductId);
            result.Value.Should().Be(createdProduct);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnBadRequest_WhenProductIsNull()
        {
            // Arrange
            Product product = null;
            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.CreateProduct(product);

            // Assert
            actionResult.Result.Should().BeOfType<BadRequestResult>();
            var result = actionResult.Result.As<BadRequestResult>();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            var product = fixture.Create<Product>();

            mockProductRepository
                .Setup(repo => repo.AddProduct(product))
                .ThrowsAsync(new Exception("Simulated error"));
            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.CreateProduct(product);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>();
            var result = actionResult.Result.As<ObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnOkResponse_WithUpdatedProduct()
        {
            // Arrange
            var product = fixture.Create<Product>();
            var productToUpdate = fixture.Create<Product>();

            mockProductRepository
                .Setup(repo => repo.GetProductById(product.ProductId))
                .ReturnsAsync(productToUpdate);
            mockProductRepository
                .Setup(repo => repo.UpdateProduct(product))
                .ReturnsAsync(productToUpdate); // Return the updated product

            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.UpdateProduct(product);

            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var result = actionResult.Result.As<OkObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().Be(productToUpdate);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNotFound_WhenProductNotFound()
        {
            // Arrange
            var product = fixture.Create<Product>();

            mockProductRepository
                .Setup(repo => repo.GetProductById(product.ProductId))
                .ReturnsAsync((Product)null);

            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.UpdateProduct(product);

            // Assert
            actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
            var result = actionResult.Result.As<NotFoundObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Value.Should().Be($"There is no Product with ID: {product.ProductId}");
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            var product = fixture.Create<Product>();

            mockProductRepository
                .Setup(repo => repo.GetProductById(product.ProductId))
                .ThrowsAsync(new Exception("Simulated error"));

            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.UpdateProduct(product);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>();
            var result = actionResult.Result.As<ObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            result.Value.Should().Be("Cannot update this Product at the moment.");
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnDeletedProduct_WhenProductDeletedSuccessfully()
        {
            // Arrange
            var id = fixture.Create<int>();
            var productToDelete = fixture.Create<Product>();

            mockProductRepository
                .Setup(repo => repo.GetProductById(id))
                .ReturnsAsync(productToDelete);
            mockProductRepository
                .Setup(repo => repo.DeleteProduct(id))
                .ReturnsAsync(productToDelete);

            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.DeleteProduct(id);

            // Assert
            actionResult.Should().NotBeNull();
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var result = actionResult.Result.As<OkObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().Be(productToDelete);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNotFound_WhenProductNotFound()
        {
            // Arrange
            var id = fixture.Create<int>();

            mockProductRepository
                .Setup(repo => repo.GetProductById(id))
                .ReturnsAsync((Product)null);

            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.DeleteProduct(id);

            // Assert
            actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
            var result = actionResult.Result.As<NotFoundObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Value.Should().Be($"You cannot delete Product with ID: {id}");
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            var id = fixture.Create<int>();

            mockProductRepository
                .Setup(repo => repo.GetProductById(id))
                .ThrowsAsync(new Exception("Simulated error"));

            var controller = new ProductsController(mockProductRepository.Object);

            // Act
            var actionResult = await controller.DeleteProduct(id);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>();
            var result = actionResult.Result.As<ObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            result.Value.Should().Be($"You cannot delete Product with ID: {id}");
        }


    }
}