using Business.DTOs.InvoiceItemDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.InvoiceDTOs
{
    public class UpdateInvoiceDto
    {
        public string Serial { get; set; } = string.Empty;
        public string? Note { get; set; }

        public int StoreId { get; set; }
        public int CustomerId { get; set; }

        public List<UpdateInvoiceItemDto> Items { get; set; } = new();
    }
}
