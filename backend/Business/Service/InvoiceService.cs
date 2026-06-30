using Business.DTOs.Common;
using Business.DTOs.InvoiceDTOs;
using Business.DTOs.InvoiceItemDTOs;
using Business.Entities;
using Business.Interfaces;

namespace Business.Service
{
    public class InvoiceService : IInvoiceServices
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        // method to get all invoices with pagination and return PagedResultDto<InvoiceListDto>
        public async Task<PagedResultDto<InvoiceListDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var result = await _invoiceRepository.GetAllAsync(pageNumber, pageSize);

            return new PagedResultDto<InvoiceListDto>
            {
                Items = result.Items.Select(x => new InvoiceListDto
                {
                    Id = x.Id,
                    Serial = x.Serial,
                    InvoiceDate = x.InvoiceDate,
                    CustomerName = x.Customer.Name,
                    StoreName = x.Store.Name,
                    TotalPrice = x.TotalPrice
                }).ToList(),

                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages
            };
        }
        // method to get an invoice by id and return InvoiceResponseDto
        public async Task<InvoiceResponseDto?> GetByIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice is null) return null;

            return new InvoiceResponseDto
            {
                Id = invoice.Id,
                Serial = invoice.Serial,
                Note = invoice.Note,
                InvoiceDate = invoice.InvoiceDate,
                StoreId = invoice.StoreId,
                StoreName = invoice.Store.Name,
                CustomerId = invoice.CustomerId,
                CustomerName = invoice.Customer.Name,
                TotalPrice = invoice.TotalPrice,
                Items = invoice.InvoiceItems.Select(i => new InvoiceItemResponseDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    DiscountPercent = i.DiscountPercent,
                    TaxPercent = i.TaxPercent,
                    Total = i.Total
                }).ToList()
            };
        }
        // method to create an invoice from CreateInvoiceDto and return InvoiceResponseDto
        public async Task<InvoiceResponseDto> CreateAsync(CreateInvoiceDto dto)
        {
            var invoice = new Invoice
            {
                Serial = dto.Serial,
                Note = dto.Note,
                StoreId = dto.StoreId,
                CustomerId = dto.CustomerId,
                InvoiceDate = DateTime.UtcNow,
                InvoiceItems = dto.Items.Select(i => new InvoiceItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    DiscountPercent = i.DiscountPercent,
                    TaxPercent = i.TaxPercent,
                    Total = CalculateItemTotal(i.Quantity, i.Price, i.DiscountPercent, i.TaxPercent)
                }).ToList()
            };

            invoice.TotalPrice = invoice.InvoiceItems.Sum(x => x.Total);

            var createdInvoice = await _invoiceRepository.AddAsync(invoice);

            // Map entity → DTO here in the service
            return new InvoiceResponseDto
            {
                Id = createdInvoice!.Id,
                Serial = createdInvoice.Serial,
                Note = createdInvoice.Note,
                InvoiceDate = createdInvoice.InvoiceDate,
                StoreId = createdInvoice.StoreId,
                StoreName = createdInvoice.Store.Name,
                CustomerId = createdInvoice.CustomerId,
                CustomerName = createdInvoice.Customer.Name,
                TotalPrice = createdInvoice.TotalPrice,
                Items = createdInvoice.InvoiceItems.Select(i => new InvoiceItemResponseDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    DiscountPercent = i.DiscountPercent,
                    TaxPercent = i.TaxPercent,
                    Total = i.Total
                }).ToList()
            };
        }
        // method to update an invoice by id and UpdateInvoiceDto
        public async Task<bool> UpdateAsync(int id, UpdateInvoiceDto dto)
        {
            var invoice = await _invoiceRepository.GetInvoiceWithItemsAsync(id);

            if (invoice == null)
                return false;

            invoice.Serial = dto.Serial;
            invoice.Note = dto.Note;
            invoice.StoreId = dto.StoreId;
            invoice.CustomerId = dto.CustomerId;

            _invoiceRepository.RemoveInvoiceItems(invoice.InvoiceItems);

            invoice.InvoiceItems = dto.Items.Select(i => new InvoiceItem
            {
                InvoiceId = invoice.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price,
                DiscountPercent = i.DiscountPercent,
                TaxPercent = i.TaxPercent,
                Total = CalculateItemTotal(
                    i.Quantity,
                    i.Price,
                    i.DiscountPercent,
                    i.TaxPercent)
            }).ToList();

            invoice.TotalPrice = invoice.InvoiceItems.Sum(x => x.Total);

            await _invoiceRepository.SaveChangesAsync();

            return true;
        }
        // method to delete an invoice by id
        public async Task<bool> DeleteAsync(int id)
        {
            var invoice = await _invoiceRepository.GetInvoiceWithItemsAsync(id);

            if (invoice == null)
                return false;

            _invoiceRepository.Delete(invoice);
            await _invoiceRepository.SaveChangesAsync();

            return true;
        }
        // method to calculate the total for an invoice item
        private static decimal CalculateItemTotal(
            int quantity,
            decimal price,
            decimal discountPercent,
            decimal taxPercent)
        {
            var subtotal = quantity * price;
            var discount = subtotal * discountPercent / 100;
            var afterDiscount = subtotal - discount;
            var tax = afterDiscount * taxPercent / 100;

            return afterDiscount + tax;
        }
    }
}
