using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public string Serial {  get; set; }= string.Empty;
        public string? Note { get; set; }

        public int StoreId { get; set; }
        public Store Store { get; set; } = null!;
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();



    }
}
