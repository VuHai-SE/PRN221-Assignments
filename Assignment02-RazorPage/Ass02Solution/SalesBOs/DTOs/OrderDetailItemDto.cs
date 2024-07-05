using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesBOs.DTOs
{
    public class OrderDetailItemDto
    {
        public int ProductId { get; set; }
        public int? CategoryId { get; set; } // Allow nullable values
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public float? Discount { get; set; } // Allow nullable values
        public decimal Sum => UnitPrice * Quantity * (1 - (decimal)(Discount ?? 0));
    }

}
