using Business.Entities;
using DataAccess.Data;
using DataAccess.Repositories.CustomerRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
      
        private readonly ICustomerRepository _customerService;

        public CustomersController(ICustomerRepository customerservice)
        {
            _customerService = customerservice;
        }
        // get all customers 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }
    }
}
