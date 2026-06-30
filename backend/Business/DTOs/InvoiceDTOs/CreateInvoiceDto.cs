using Business.DTOs.InvoiceItemDTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.InvoiceDTOs
{
    public class CreateInvoiceDto
    {
        [Required]
        public string Serial { get; set; } = string.Empty;
        public string? Note { get; set; }

        public int StoreId { get; set; }
        public int CustomerId { get; set; }

        [Required]
        [MinLength(1)]
        public List<CreateInvoiceItemDto> Items { get; set; } = new();
    }
}
