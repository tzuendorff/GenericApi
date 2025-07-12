using GenericApi.BusinessLogic;
using GenericApi.Classes;
using GenericApi.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GenericApi.Controllers;

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

    [HttpPost("/orders")]
    public async Task<IActionResult> CreateOrder(Order order)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid order model: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
            return BadRequest(ModelState);
        }
        try
        {
            var orderId = await _businessLogic.CreateEntity(order);
            return Ok(orderId);
        }

        catch (ArgumentException exception)
        {
            _logger.LogError($"Could not create order. {exception}");
            return UnprocessableEntity (ErrorMessages.ErrorText[HttpStatusCode.UnprocessableContent]);  
        }
        catch (Exception exception)
        {
            _logger.LogError($"Could not create order. {exception}");
            return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.ErrorText[HttpStatusCode.InternalServerError]);
        }
    }


    [HttpGet("/orders")]
    public async Task<IActionResult> GetOrders(string? orderId)
    {
        if (orderId == null)
        {
            orderId = "";
        }

        try
        {
            var resultOrders = await _businessLogic.ReadAllEntitiesByFilter(orderId);

            if (resultOrders.Count == 0 )
            {
                _logger.LogError($"No order found whose orderId containes {orderId}");
                return NotFound(ErrorMessages.ErrorText[HttpStatusCode.NotFound]);
            }

            return Ok(resultOrders);
        }
        catch (Exception exception)
        {
            _logger.LogError($"Could not read order with id {orderId}. {exception}");
            return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.ErrorText[HttpStatusCode.InternalServerError]);
        }
    }

    [HttpPut("/orders")]
    public async Task<IActionResult> UpdateOrder(Order order)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid order model: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
            return BadRequest(ModelState);
        }
        try
        {
            var numberOfModifiedDocuments = await _businessLogic.UpdateEntity(order);
            if (numberOfModifiedDocuments > 0)
            {
                return Ok(numberOfModifiedDocuments);
            }

            _logger.LogError($"Could not update order. Not order with id {order.Id} found.");
                return NotFound(ErrorMessages.ErrorText[HttpStatusCode.NotFound]);
            }
        catch (FormatException exception)
        {
            _logger.LogError($"Could not update order. {exception.InnerException}");
                return NotFound(ErrorMessages.ErrorText[HttpStatusCode.NotFound]);
        }
        catch (Exception exception)
        {
            _logger.LogError($"Could not update order. {exception}");
            return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.ErrorText[HttpStatusCode.InternalServerError]);
        }
    }

    [HttpDelete("/orders")]
    public async Task<IActionResult> DeleteOrders(string orderId)
    {
        try
        {
            var numberOfDeletedDocuments = await _businessLogic.DeleteEntity(orderId);
            if (numberOfDeletedDocuments > 0)
            {
                return Ok(numberOfDeletedDocuments);
            }
            _logger.LogError($"Could not delete order with id {orderId}.");
            return NotFound($"No order with Id {orderId} found");
        }

        catch (FormatException exception)
        {
            _logger.LogError($"Could not delete order with id {orderId}. {exception}");
            return NotFound(ErrorMessages.ErrorText[HttpStatusCode.NotFound]);
        }
        catch (Exception exception)
        {
            _logger.LogError($"Could not delete order with id {orderId}. {exception}");
            return StatusCode(StatusCodes.Status500InternalServerError, ErrorMessages.ErrorText[HttpStatusCode.InternalServerError]);
        }
    }

}