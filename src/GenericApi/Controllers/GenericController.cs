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
            // Empty filter. Return all orders in collection
            if (orderId == null || orderId == "")
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

        //[HttpPut("/orders")]
        //public IActionResult UpdateOrder(Order order)
        //{
        //    try
        //    {
        //        _businessLogic.UpdateEntity(order);
        //        return Ok();
        //    }
        //    catch (KeyNotFoundException exception)
        //    {
        //        _logger.LogError($"Could not find order with id {order.Id}. {exception}");
        //        return StatusCode((int)HttpStatusCode.NotFound, ErrorText[HttpStatusCode.NotFound]);
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError($"Could not update order. {exception}");
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ErrorText[HttpStatusCode.InternalServerError]);
        //    }
        //}

        //[HttpDelete("/orders")]
        //public IActionResult DeleteOrders(int orderId)
        //{
        //    try
        //    {
        //        _businessLogic.DeleteEntity(orderId);
        //        return Ok();
        //    }
        //    catch (KeyNotFoundException exception)
        //    {
        //        _logger.LogError($"Could not find order with id {orderId}. {exception}");
        //        return StatusCode((int)HttpStatusCode.NotFound, ErrorText[HttpStatusCode.NotFound]);
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.LogError($"Could not delete order with id {orderId}. {exception}");
        //        return StatusCode((int)HttpStatusCode.InternalServerError, ErrorText[HttpStatusCode.InternalServerError]);
        //    }
        //}
    }
}
