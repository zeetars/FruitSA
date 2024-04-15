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
    public class CategoriesControllerTests
    {
        private readonly Fixture fixture;
        private readonly Mock<ICategoryRepository> mockCategoryRepository;
        public CategoriesControllerTests()
        {
            fixture = new Fixture();
            mockCategoryRepository = new Mock<ICategoryRepository>();
        }

        [Fact]
        public async Task GetCategories_ShouldReturnOkResponse_WithCategories()
        {
            // Arrange
            var categories = fixture.CreateMany<Category>().ToList();
            mockCategoryRepository
                .Setup(repo => repo.GetCategories())
                .ReturnsAsync(categories);

            var controller = new CategoriesController(mockCategoryRepository.Object);

            // Act
            var actionResult = await controller.GetCategories();

            // Assert
            actionResult.Should().BeOfType<OkObjectResult>();
            var result = actionResult as ObjectResult;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<List<Category>>().And.BeEquivalentTo(categories);
        }

        [Fact]
        public async Task GetCategories_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            mockCategoryRepository
                .Setup(repo => repo.GetCategories())
                .ThrowsAsync(new Exception("Simulated error"));

            var controller = new CategoriesController(mockCategoryRepository.Object);

            // Act
            var actionResult = await controller.GetCategories();

            // Assert
            actionResult.Should().BeOfType<ObjectResult>();
            var result = actionResult as ObjectResult;
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            result.Value.Should().Be("Error retrieving Categories.");
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnOkResponse_WithCategory()
        {
            // Arrange
            var id = fixture.Create<int>();
            var expectedCategory = fixture.Create<Category>();

            mockCategoryRepository
                .Setup(repo => repo.GetCategoryById(id))
                .ReturnsAsync(expectedCategory);
            var controller = new CategoriesController(mockCategoryRepository.Object);

            // Act
            var actionResult = await controller.GetCategoryById(id);

            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var result = actionResult.Result.As<OkObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().Be(expectedCategory);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnNotFound_WhenCategoryNotFound()
        {
            // Arrange
            var id = fixture.Create<int>();

            mockCategoryRepository
                .Setup(repo => repo.GetCategoryById(id))
                .ReturnsAsync((Category)null);
            var controller = new CategoriesController(mockCategoryRepository.Object);

            // Act
            var actionResult = await controller.GetCategoryById(id);

            // Assert
            actionResult.Result.Should().BeOfType<NotFoundResult>();
            var result = actionResult.Result.As<NotFoundResult>();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            var id = fixture.Create<int>();

            mockCategoryRepository
                .Setup(repo => repo.GetCategoryById(id))
                .ThrowsAsync(new Exception("Simulated error"));
            var controller = new CategoriesController(mockCategoryRepository.Object);

            // Act
            var actionResult = await controller.GetCategoryById(id);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>();
            var result = actionResult.Result.As<ObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task CreateCategory_ShouldReturnCreatedAtAction_WithCategory()
        {
            // Arrange
            var Category = fixture.Create<Category>();
            var createdCategory = fixture.Create<Category>();

            mockCategoryRepository
                .Setup(repo => repo.AddCategory(Category))
                .ReturnsAsync(createdCategory);
            var controller = new CategoriesController(mockCategoryRepository.Object);

            // Act
            var actionResult = await controller.CreateCategory(Category);

            // Assert
            actionResult.Result.Should().BeOfType<CreatedAtActionResult>();
            var result = actionResult.Result.As<CreatedAtActionResult>();
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.ActionName.Should().Be(nameof(CategoriesController.GetCategoryById));
            result.RouteValues["id"].Should().Be(createdCategory.CategoryId);
            result.Value.Should().Be(createdCategory);
        }

        [Fact]
        public async Task CreateCategory_ShouldReturnBadRequest_WhenCategoryIsNull()
        {
            // Arrange
            Category Category = null;
            var controller = new CategoriesController(mockCategoryRepository.Object);

            // Act
            var actionResult = await controller.CreateCategory(Category);

            // Assert
            actionResult.Result.Should().BeOfType<BadRequestResult>();
            var result = actionResult.Result.As<BadRequestResult>();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateCategory_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            var Category = fixture.Create<Category>();

            mockCategoryRepository
                .Setup(repo => repo.AddCategory(Category))
                .ThrowsAsync(new Exception("Simulated error"));
            var controller = new CategoriesController(mockCategoryRepository.Object);

            // Act
            var actionResult = await controller.CreateCategory(Category);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>();
            var result = actionResult.Result.As<ObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnOkResponse_WithUpdatedCategory()
        {
            // Arrange
            var Category = fixture.Create<Category>();
            var CategoryToUpdate = fixture.Create<Category>();

            mockCategoryRepository
                .Setup(repo => repo.GetCategoryById(Category.CategoryId))
                .ReturnsAsync(CategoryToUpdate);
            mockCategoryRepository
                .Setup(repo => repo.UpdateCategory(Category))
                .ReturnsAsync(CategoryToUpdate); // Return the updated Category

            var controller = new CategoriesController(mockCategoryRepository.Object);

            // Act
            var actionResult = await controller.UpdateCategory(Category);

            // Assert
            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var result = actionResult.Result.As<OkObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().Be(CategoryToUpdate);
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnNotFound_WhenCategoryNotFound()
        {
            // Arrange
            var Category = fixture.Create<Category>();

            mockCategoryRepository
                .Setup(repo => repo.GetCategoryById(Category.CategoryId))
                .ReturnsAsync((Category)null);

            var controller = new CategoriesController(mockCategoryRepository.Object);

            // Act
            var actionResult = await controller.UpdateCategory(Category);

            // Assert
            actionResult.Result.Should().BeOfType<NotFoundObjectResult>();
            var result = actionResult.Result.As<NotFoundObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Value.Should().Be($"There is no Category with ID: {Category.CategoryId}");
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            var Category = fixture.Create<Category>();

            mockCategoryRepository
                .Setup(repo => repo.GetCategoryById(Category.CategoryId))
                .ThrowsAsync(new Exception("Simulated error"));

            var controller = new CategoriesController(mockCategoryRepository.Object);

            // Act
            var actionResult = await controller.UpdateCategory(Category);

            // Assert
            actionResult.Result.Should().BeOfType<ObjectResult>();
            var result = actionResult.Result.As<ObjectResult>();
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            result.Value.Should().Be("Cannot update this Category at the moment.");
        }
    }
}