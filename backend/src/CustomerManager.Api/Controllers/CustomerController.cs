using CustomerManager.Models.Entities;
using CustomerManager.Models.Results;
using CustomerManager.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerManager.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("/api/v1/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary> Get customer by id </summary>
        /// <returns> The customer corresponding to the given Id </returns>
        /// <param name="id"> Represents the customer id </param>
        /// <response code="200"> A finded customer </response>
        /// <response code="400"> Invalid customer Id </response>
        /// <response code="401"> Invalid authorization </response>
        /// <response code="404"> Customer not found </response>
        /// <response code="500"> Server error </response>
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var customerResult = await _customerService.GetAsync(id);
            if (customerResult.Error != null)
                return new ProblemDetailsResult(customerResult.StatusCode, Request, customerResult.Error).GetObjectResult();

            return new OkObjectResult(customerResult.Value);
        }

        /// <summary> Get all customers </summary>
        /// <returns> All database clients </returns>
        /// <response code="200"> Success </response>
        /// <response code="204"> There are no customers </response>    
        /// <response code="401"> Invalid authorization </response>     
        /// <response code="500"> Server error </response>
        [ProducesResponseType(typeof(List<Customer>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var customersResult = await _customerService.GetAllAsync();
            if (customersResult.Error != null)
                return new ProblemDetailsResult(customersResult.StatusCode, Request, customersResult.Error).GetObjectResult();

            return new OkObjectResult(customersResult.Value);
        }

        /// <summary> Creates a new customer </summary>
        /// <param name="customer"> Represents the customer model </param>
        /// <response code="201"> Success </response>
        /// <response code="400"> Invalid customer </response>           
        /// <response code="401"> Invalid authorization </response>     
        /// <response code="500"> Server error </response>
        [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Customer customer)
        {
            var customerResult = await _customerService.CreateAsync(customer);
            if (customerResult.Error != null)
                return new ProblemDetailsResult(customerResult.StatusCode, Request, customerResult.Error).GetObjectResult();

            return CreatedAtRoute(new { id = customer.Id }, customer);
        }

        /// <summary> Creates a new customer </summary>
        /// <param name="id"> Represents the customer id </param>
        /// <param name="customer"> Represents the updated customer model </param>
        /// <response code="204"> Updated </response>
        /// <response code="400"> Invalid id or customer </response>           
        /// <response code="401"> Invalid authorization </response>     
        /// <response code="404"> Customer id does not exists </response>
        /// <response code="500"> Server error </response>  
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateAsync(string id, Customer customer)
        {
            var customerResult = await _customerService.UpdateAsync(id, customer);
            if (customerResult.Error != null)
                return new ProblemDetailsResult(customerResult.StatusCode, Request, customerResult.Error).GetObjectResult();

            return NoContent();
        }

        /// <summary> Deletes a customer </summary>
        /// <param name="id"> Customer id </param>
        /// <response code="204"> Removes the customer if it exists </response>
        /// <response code="400"> Invalid customer id </response>        
        /// <response code="401"> Invalid authorization </response>     
        /// <response code="500"> Server error </response>        
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var customerResult = await _customerService.DeleteAsync(id);
            if (customerResult.Error != null)
                return new ProblemDetailsResult(customerResult.StatusCode, Request, customerResult.Error).GetObjectResult();

            return NoContent();
        }
    }
}