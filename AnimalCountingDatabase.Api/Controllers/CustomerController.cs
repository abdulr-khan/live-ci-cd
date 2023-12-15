
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnimalCountingDatabase.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerContext _customerContext;

        public CustomerController(CustomerContext customerContext)
        {
            this._customerContext = customerContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> GetAll()
            => await _customerContext.Customers.ToArrayAsync();
        [HttpPost]
        public async Task<Customer> Add([FromBody] Customer c)
        {
            _customerContext.Customers.Add(c);
            await _customerContext.SaveChangesAsync();
            return c;
        }
    }
}
