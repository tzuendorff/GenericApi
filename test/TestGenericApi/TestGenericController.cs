using GenericApi.BusinessLogic;
using GenericApi.Classes;
using GenericApi.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;


namespace TestGenericApi
{
    public class GenericControllerTests
    {
        private readonly GenericController _controller;
        private MockOrderMongoConnector _mockOrderMongoConnector = new MockOrderMongoConnector();   

        public GenericControllerTests()
        {
            var businessLogicLogger = new LoggerFactory().CreateLogger<GenericController>();
            var controllerLogger = new LoggerFactory().CreateLogger<GenericController>();
            var businessLogic = new OrderBusinessLogic(businessLogicLogger, _mockOrderMongoConnector);
            _controller = new GenericController(controllerLogger, businessLogic);
        }

        [Fact]
        public async Task CreateOrder_Returns_Ok()
        {
            // ARRANGE
            var order = new Order {
                CustomerFirstName = "John",
                CustomerLastName = "Doe",
                Approved = true,
                Items = new List<Item> {
                    new Item { Id = "1", Amount = 20 },
                    new Item { Id = "2", Amount = 10 }
                }
            };

            // ACT
            var result = await _controller.CreateOrder(order);

            // ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateOrder_Returns_ExpectedValue()
        {
            // ARRANGE
            var order = new Order
            {
                CustomerFirstName = "John",
                CustomerLastName = "Doe",
                Approved = true,
                Items = new List<Item> {
                    new Item { Id = "1", Amount = 20 },
                    new Item { Id = "2", Amount = 10 }
                }
            };

            // ACT
            var result = await _controller.CreateOrder(order);

            // ASSERT
            var okResult = result as OkObjectResult;
            var returnedOrderId = okResult?.Value;

            Assert.Equal("1111", returnedOrderId);
        }

        [Fact]
        public async Task CreateOrder_Returns_BadRequest()
        {
            // ARRANGE
            // Simulate invalid model state
            _controller.ModelState.AddModelError("Name", "Required");
            var order = new Order { BaseId = "" };

            // ACT
            var result = await _controller.CreateOrder(order);
            
            // ARRANGE
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateOrder_Returns_UnprosessableEntity()
        {
            // ARRANGE
            var order = new Order
            {
                BaseId = "1111", // Orders should not have an Id set by the caller.
                CustomerFirstName = "John",
                CustomerLastName = "Doe",
                Approved = true,
                Items = new List<Item> {
                    new Item { Id = "1", Amount = 20 },
                    new Item { Id = "2", Amount = 10 }
                }
            };

            // ACT
            var result = await _controller.CreateOrder(order);

            // ASSERT
            Assert.IsType<UnprocessableEntityObjectResult>(result);
        }

        [Fact]
        public async Task CreateOrder_Returns_InternServerError()
        {
            // ARRANGE
            _mockOrderMongoConnector.shouldThrowGeneralException = true;

            var order = new Order
            {
                CustomerFirstName = "John",
                CustomerLastName = "Doe",
                Approved = true,
                Items = new List<Item> {
                    new Item { Id = "1", Amount = 20 },
                    new Item { Id = "2", Amount = 10 }
                }
            };

            // ACT
            var result = await _controller.CreateOrder(order) as ObjectResult;

            // ASSERT
            Assert.Equal(500, result.StatusCode);
        }



        [Fact]
        public async Task GetOrders_Returns_Ok()
        {
            // ARRANGE

            // ACT
            var result = await _controller.GetOrders(null);

            // ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetOrders_Returns_Expected_Value()
        {
            // ARRANGE
            var expectedOrder = new List<Order>
        {
            new Order
            {
                BaseId = "1111",
                CustomerFirstName = "Alice",
                CustomerLastName = "Smith",
                Approved = true,
                Items = new List<Item>
                {
                    new Item { Id = "1", Amount = 2 },
                    new Item { Id = "2", Amount = 1 }
                }
            },
            new Order
            {
                BaseId = "2222",
                CustomerFirstName = "Bob",
                CustomerLastName = "Johnson",
                Approved = false,
                Items = new List<Item>
                {
                    new Item { Id = "3", Amount = 5 }
                }
            }
        };

            // ACT
            var result = await _controller.GetOrders(null);

            // ASSERT
            var okResult = result as OkObjectResult;
            var returnedOrderId = okResult?.Value;

            // Comparing the two Lists compares the references, not the actual values.
            // Serializing their content and comparing the restulting string actually compare the content.
            Assert.Equal(JsonSerializer.Serialize(expectedOrder), JsonSerializer.Serialize(returnedOrderId)); 
        }

        [Fact]
        public async Task GetOrders_Returns_NotFound()
        {
            // ARRANGE

            // ACT
            var result = await _controller.GetOrders("notFoundId");

            // ASSERT
            Assert.IsType<NotFoundObjectResult>(result);
        }
        
        [Fact]
        public async Task GetOrders_Returns_InternServerError()
        {
            // ARRANGE
            _mockOrderMongoConnector.shouldThrowGeneralException = true;

            // ACT
            var result = await _controller.GetOrders("1111") as ObjectResult;

            // ASSERT
            Assert.Equal(500, result.StatusCode);
        }



        [Fact]
        public async Task UpdateOrder_EntityFound_ReturnsOk()
        {
            // ARRANGE
            var updatedOrder = new Order
            {
                BaseId = "2222",
                CustomerFirstName = "Alice",
                CustomerLastName = "Smith",
                Approved = true,
                Items = new List<Item>
                {
                    new Item { Id = "1", Amount = 2 },
                    new Item { Id = "2", Amount = 1 }
                }
            };

            // ACT
            var result = await _controller.UpdateOrder(updatedOrder);

            // ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateOrder_EntityFound_Returns_ExpectedValue()
        {
            // ARRANGE
            var updatedOrder = new Order
            {
                BaseId = "2222",
                CustomerFirstName = "Alice",
                CustomerLastName = "Smith",
                Approved = true,
                Items = new List<Item>
                {
                    new Item { Id = "1", Amount = 2 },
                    new Item { Id = "2", Amount = 1 }
                }
            };

            // ACT
            var result = await _controller.UpdateOrder(updatedOrder);

            // ASSERT
            var okResult = result as OkObjectResult;
            var numberModified = okResult?.Value;

            Assert.Equal("Number of modified entities: 1", numberModified);
        }

        [Fact]
        public async Task UpdateOrder_EntityNotFound_Returns_ReturnsNotFound1()
        {
            // ARRANGE
            var updatedOrder = new Order
            {
                BaseId = "someBadId",
                CustomerFirstName = "Alice",
                CustomerLastName = "Smith",
                Approved = true,
                Items = new List<Item>
                {
                    new Item { Id = "1", Amount = 2 },
                    new Item { Id = "2", Amount = 1 }
                }
            };

            // ACT
            var result = await _controller.UpdateOrder(updatedOrder);

            // ASSERT
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateOrder_EntityNotFound_Returns_ReturnsNotFound2()
        {
            // ARRANGE
            var updatedOrder = new Order
            {
                BaseId = "notFoundId",
                CustomerFirstName = "Alice",
                CustomerLastName = "Smith",
                Approved = true,
                Items = new List<Item>
                {
                    new Item { Id = "1", Amount = 2 },
                    new Item { Id = "2", Amount = 1 }
                }
            };

            // ACT
            var result = await _controller.UpdateOrder(updatedOrder);

            // ASSERT
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateOrder_InvalidModel_ReturnsBadRequest()
        {
            // ARRANGE
            _controller.ModelState.AddModelError("Name", "Required");
            var order = new Order { BaseId = "" };
            
            // ACT
            var result = await _controller.UpdateOrder(order);
            
            // ASSERT
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateOrder_Returns_InternServerError()
        {
            // ARRANGE
            _mockOrderMongoConnector.shouldThrowGeneralException = true;

            var order = new Order
            {
                BaseId = "1111",
                CustomerFirstName = "John",
                CustomerLastName = "Doe",
                Approved = true,
                Items = new List<Item> {
                    new Item { Id = "1", Amount = 20 },
                    new Item { Id = "2", Amount = 10 }
                }
            };

            // ACT
            var result = await _controller.UpdateOrder(order) as ObjectResult;

            // ASSERT
            Assert.Equal(500, result.StatusCode);
        }



        [Fact]
        public async Task DeleteOrder_EntityFound_ReturnsOk()
        {
            // ARRANGE

            // ACT
            var result = await _controller.DeleteOrders("1111");

            // ASSERT
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_EntityFound_Returns_ExpectedValue()
        {
            // ARRANGE

            // ACT
            var result = await _controller.DeleteOrders("1111");

            // ASSERT
            var okResult = result as OkObjectResult;
            var numberModified = okResult?.Value;

            Assert.Equal("Number of deleted entities: 1", numberModified);
        }

        [Fact]
        public async Task DeleteOrder_EntityNotFound_Returns_ReturnsNotFound1()
        {
            // ARRANGE

            // ACT
            var result = await _controller.DeleteOrders("notFoundId");

            // ASSERT
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_EntityNotFound_Returns_ReturnsNotFound2()
        {
            // ARRANGE


            // ACT
            var result = await _controller.DeleteOrders("someBadId");

            // ASSERT
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteOrder_Returns_InternServerError()
        {
            // ARRANGE
            _mockOrderMongoConnector.shouldThrowGeneralException = true;

            // ACT
            var result = await _controller.DeleteOrders("1111") as ObjectResult;

            // ASSERT
            Assert.Equal(500, result.StatusCode);
        }

    }
}