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
    public class UploadsControllerTests
    {
        private readonly Fixture fixture;
        private readonly Mock<IUploadRepository> mockUploadRepository;
        public UploadsControllerTests()
        {
            fixture = new Fixture();
            mockUploadRepository = new Mock<IUploadRepository>();
        }

        [Fact]
        public async Task CreateProducts_ShouldReturnOkResponse_WhenProductsAddedSuccessfully()
        {
            // Arrange
            var products = fixture.CreateMany<Product>().ToList();
            mockUploadRepository
                .Setup(repo => repo.AddProducts(products))
                .Returns(Task.CompletedTask);

            var controller = new UploadsController(mockUploadRepository.Object);

            // Act
            var actionResult = await controller.CreateProducts(products);

            // Assert
            actionResult.Should().BeOfType<OkResult>();
            var result = actionResult as OkResult;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task CreateProducts_ShouldReturnBadRequest_WhenProductsIsNull()
        {
            // Arrange
            List<Product> products = null;

            var controller = new UploadsController(mockUploadRepository.Object);

            // Act
            var actionResult = await controller.CreateProducts(products);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
            var result = actionResult as BadRequestResult;
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateProducts_ShouldReturnInternalServerError_OnEcxception()
        {
            // Arrange
            var products = fixture.CreateMany<Product>().ToList();
            mockUploadRepository
                .Setup(repo => repo.AddProducts(products))
                .ThrowsAsync(new Exception("Simulated error"));

            var controller = new UploadsController(mockUploadRepository.Object);

            // Act
            var actionResult = await controller.CreateProducts(products);

            // Assert
            actionResult.Should().BeOfType<ObjectResult>();
            var result = actionResult as ObjectResult;
            result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
            result.Value.Should().Be("Error inserting the Products.");
        }
    }
}