using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.InvoiceDTOs
{
    public class InvoiceListDto
    {
        public int Id { get; set; }

        public string Serial { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string StoreName { get; set; } = string.Empty;

        public decimal TotalPrice
        {
            get; set;
        }
    }
}
