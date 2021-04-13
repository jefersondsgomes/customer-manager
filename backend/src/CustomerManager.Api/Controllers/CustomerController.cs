using CustomerManager.Model.Common;
using CustomerManager.Model.Result;
using CustomerManager.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CustomerManager.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var customerResult = await _customerService.GetAsync(id);
            if (customerResult.Error != null)
                return new ProblemDetailsResult(customerResult.StatusCode, Request, customerResult.Error).GetObjectResult();

            return new OkObjectResult(customerResult.Value);
        }

        [HttpGet]
        [Route("~/api/customers")]
        public async Task<IActionResult> GetAllAsync()
        {
            var customersResult = await _customerService.GetAllAsync();
            if (customersResult.Error != null)
                return new ProblemDetailsResult(customersResult.StatusCode, Request, customersResult.Error).GetObjectResult();

            return new OkObjectResult(customersResult.Value);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Customer customer)
        {
            var customerResult = await _customerService.CreateAsync(customer);
            if (customerResult.Error != null)
                return new ProblemDetailsResult(customerResult.StatusCode, Request, customerResult.Error).GetObjectResult();

            return CreatedAtRoute(new { id = customer.Id }, customer);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateAsync(string id, Customer customer)
        {
            var customerResult = await _customerService.UpdateAsync(id, customer);
            if (customerResult.Error != null)
                return new ProblemDetailsResult(customerResult.StatusCode, Request, customerResult.Error).GetObjectResult();

            return NoContent();
        }

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