using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.InvoiceItemDTOs
{
    public class InvoiceItemResponseDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal DiscountPercent { get; set; }
        public decimal TaxPercent { get; set; }

        public decimal Total { get; set; }
    }
}
