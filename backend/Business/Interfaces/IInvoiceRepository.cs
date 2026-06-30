using Business.DTOs.Common;
using Business.DTOs.InvoiceDTOs;
using Business.Entities;

namespace Business.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<PagedResultDto<Invoice>> GetAllAsync(int pageNumber, int pageSize);
        Task<Invoice?> GetByIdAsync(int id);
        Task<Invoice?> AddAsync(Invoice invoice);
        Task<Invoice?> GetInvoiceWithItemsAsync(int id);
        void RemoveInvoiceItems(IEnumerable<InvoiceItem> items);
        void Delete(Invoice invoice);
        Task SaveChangesAsync();
    }
}
