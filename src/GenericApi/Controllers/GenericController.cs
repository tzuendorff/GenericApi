using GenericApi.BusinessLogic;
using GenericApi.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace GenericApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenericController : ControllerBase
    {
        private readonly ILogger<GenericController> _logger;
        private readonly IBusinessLogic<Order> _businessLogic;

        public GenericController(ILogger<GenericController> logger, IBusinessLogic<Order> businessLogic)
        {
            _logger = logger;
            _businessLogic = businessLogic;
        }

        private readonly Dictionary<HttpStatusCode, string> ErrorText = new Dictionary<HttpStatusCode, string>()
        {
            { HttpStatusCode.InternalServerError, "Internal server error" },
            { HttpStatusCode.NotFound, "Could not find order" },
        };

        // Example Order. Delete later.
        private Order exampleOrder = new Order
        {
            Id = 123,
            CustomerFirstName = "John",
            CustomerLastName = "Smith",
            Approved = true,
            Items = new List<Item> {
                new Item {
                    Id = 456,
                    Amount = 5 }
            }
        };

        [HttpPost ("/orders")]
        public IActionResult CreateOrder(Order order)
        {
            try
            {
                return Ok(_businessLogic.CreateEntity(order));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not create order. {exception}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorText[HttpStatusCode.InternalServerError]);
            }
        }


        [HttpGet("/orders")]
        public IActionResult GetOrders(int? orderId)
        {
            if (!orderId.HasValue)
            {
                try
                {
                    return Ok(_businessLogic.ReadAllEntity());
                }
                catch (Exception exception)
                {
                    _logger.LogError($"Could not read all orders. {exception}");
                    return StatusCode((int)HttpStatusCode.InternalServerError, ErrorText[HttpStatusCode.InternalServerError]);
                }
            }
            try
            {
                var resultOrder = _businessLogic.ReadOneEntity(orderId.Value);

                if(resultOrder == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorText[HttpStatusCode.NotFound]);
                }

                return Ok(_businessLogic.ReadOneEntity(orderId.Value));
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not read order with id {orderId}. {exception}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorText[HttpStatusCode.InternalServerError]);
            }
        }

        [HttpPut("/orders")]
        public IActionResult UpdateOrder(Order order)
        {
            try
            {
                _businessLogic.UpdateEntity(order);
                return Ok();
            }
            catch (KeyNotFoundException exception)
            {
                _logger.LogError($"Could not find order with id {order.Id}. {exception}");
                return StatusCode((int)HttpStatusCode.NotFound, ErrorText[HttpStatusCode.NotFound]);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not update order. {exception}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorText[HttpStatusCode.InternalServerError]);
            }
        }

        [HttpDelete("/orders")]
        public IActionResult DeleteOrders(int orderId)
        {
            try
            {
                _businessLogic.DeleteEntity(orderId);
                return Ok();
            }
            catch (KeyNotFoundException exception)
            {
                _logger.LogError($"Could not find order with id {orderId}. {exception}");
                return StatusCode((int)HttpStatusCode.NotFound, ErrorText[HttpStatusCode.NotFound]);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not delete order with id {orderId}. {exception}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorText[HttpStatusCode.InternalServerError]);
            }
        }
    }
}
