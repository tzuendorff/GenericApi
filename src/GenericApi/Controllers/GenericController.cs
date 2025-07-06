using GenericApi.BusinessLogic;
using GenericApi.Classes;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetOrders(string? orderId)
        {
            if (orderId == null)
            {
                orderId = "";
            }
            // Non empty filter. Return all orders whose Id contains the given orderId.
            try
            {
                var resultOrders = _businessLogic.ReadAllEntitiesByFilter(orderId);

                if (resultOrders == null)
                {
                    _logger.LogError($"No order found whose orderId containes {orderId}");
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorText[HttpStatusCode.NotFound]);
                }

                return Ok(resultOrders);
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
                var numberOfModifiedDocuments = _businessLogic.UpdateEntity(order);
                if (numberOfModifiedDocuments > 0)
                {
                    return Ok(numberOfModifiedDocuments);
                }
                _logger.LogError($"Could not uptade order/s with id {order.Id}.");
                return NotFound($"No order with Id {order.Id} found");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not update order. {exception}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorText[HttpStatusCode.InternalServerError]);
            }
        }

        [HttpDelete("/orders")]
        public IActionResult DeleteOrders(string orderId)
        {
            try
            {
                var numberOfDeletedDocuments = _businessLogic.DeleteEntity(orderId);
                if (numberOfDeletedDocuments > 0)
                {
                    return Ok(numberOfDeletedDocuments);
                }
                _logger.LogError($"Could not delete order with id {orderId}.");
                return NotFound($"No order with Id {orderId} found");
            }
            catch (Exception exception)
            {
                _logger.LogError($"Could not delete order with id {orderId}. {exception}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ErrorText[HttpStatusCode.InternalServerError]);
            }
        }
    }
}
