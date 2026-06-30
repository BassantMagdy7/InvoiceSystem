using Business.DTOs.InvoiceItemDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.InvoiceDTOs
{
    public class InvoiceResponseDto
    {
        public int Id { get; set; }

        public string Serial { get; set; } = string.Empty;
        public string? Note { get; set; }

        public DateTime InvoiceDate { get; set; }

        public int StoreId { get; set; }
        public string StoreName { get; set; } = string.Empty;

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }

        public List<InvoiceItemResponseDto> Items { get; set; } = new();
    }
}
