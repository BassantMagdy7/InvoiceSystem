using Business.DTOs.InvoiceDTOs;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceServices _invoiceService;
        private readonly IHubContext<InvoiceHub> _hubContext;
        private readonly ILogger<InvoicesController> _logger;
        public InvoicesController(IInvoiceServices invoiceService, IHubContext<InvoiceHub> hubContext, ILogger<InvoicesController> logger)
        {
            _invoiceService = invoiceService;
            _hubContext = hubContext;
            _logger = logger;
        }
        //end point return paged list of invoices with page number and page size
        [HttpGet]
        public async Task<IActionResult> GetAll(
          [FromQuery] int pageNumber = 1,
          [FromQuery] int pageSize = 5)
        {
            _logger.LogInformation("Getting invoices page {PageNumber}", pageNumber);

            var invoices = await _invoiceService.GetAllAsync(pageNumber, pageSize);

            return Ok(invoices);
        }
        //end point return invoice by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Getting invoice with Id {InvoiceId}", id);
            var invoice = await _invoiceService.GetByIdAsync(id);

            if (invoice == null)
            {
                _logger.LogWarning("Invoice with Id {InvoiceId} was not found", id);
                return NotFound();
            }

            return Ok(invoice);
        }
        //end point create invoice , signalR hub to notify clients when invoic e is created
        [HttpPost]
        public async Task<IActionResult> Create(CreateInvoiceDto dto)
        {
            var invoice = await _invoiceService.CreateAsync(dto);

            _logger.LogInformation("Invoice created with Id {InvoiceId}", invoice.Id);

            await _hubContext.Clients.All.SendAsync("InvoiceChanged");

            return CreatedAtAction(
                nameof(GetById),
                new { id = invoice.Id },
                invoice);
        }
        // end point update invoice by id, signalR hub to notify clients when invoice is updated
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateInvoiceDto dto)
        {
            var result = await _invoiceService.UpdateAsync(id, dto);

            if (result == false)
            {
                _logger.LogWarning("Invoice with Id {InvoiceId} was not found for update", id);
                return NotFound();
            }

            _logger.LogInformation("Invoice updated with Id {InvoiceId}", id);

            await _hubContext.Clients.All.SendAsync("InvoiceChanged");

            return NoContent();
        }
        // end point delete invoice by id, signalR hub to notify clients when invoice is deleted
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _invoiceService.DeleteAsync(id);

            if (result == false)
            {
                _logger.LogWarning("Invoice with Id {InvoiceId} was not found for delete", id);
                return NotFound();
            }
            _logger.LogInformation("Invoice deleted with Id {InvoiceId}", id);
            await _hubContext.Clients.All.SendAsync("InvoiceChanged");

            return NoContent();
        }

  

    }
}
