using Business.DTOs.Common;
using Business.DTOs.InvoiceDTOs;
using Business.Entities;
using Business.Interfaces;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.InvoiceRepo
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }
        // Get all invoices with pagination
        public async Task<PagedResultDto<Invoice>> GetAllAsync(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 5 : pageSize;

            var query = _context.Invoices
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Store)
                .OrderByDescending(x => x.InvoiceDate);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDto<Invoice>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }
        // Get invoice by id with related entities
        public async Task<Invoice?> GetByIdAsync(int id)
        {
            return await _context.Invoices
                .AsNoTracking()
                .Include(x => x.Customer)
                .Include(x => x.Store)
                .Include(x => x.InvoiceItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        // Add a new invoice
        public async Task<Invoice?> AddAsync(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(invoice.Id);
        }
        
        public async Task<Invoice?> GetInvoiceWithItemsAsync(int id)
        {
            return await _context.Invoices
                .Include(x => x.InvoiceItems)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        
        public void RemoveInvoiceItems(IEnumerable<InvoiceItem> items)
        {
            _context.InvoiceItems.RemoveRange(items);
        }

        public void Delete(Invoice invoice)
        {
            _context.Invoices.Remove(invoice);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
