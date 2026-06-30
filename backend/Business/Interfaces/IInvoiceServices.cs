using Business.DTOs.Common;
using Business.DTOs.InvoiceDTOs;

namespace Business.Interfaces
{
    public interface IInvoiceServices
    {
        Task<PagedResultDto<InvoiceListDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<InvoiceResponseDto?> GetByIdAsync(int id);
        Task<InvoiceResponseDto> CreateAsync(CreateInvoiceDto dto);
        Task<bool> UpdateAsync(int id, UpdateInvoiceDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
