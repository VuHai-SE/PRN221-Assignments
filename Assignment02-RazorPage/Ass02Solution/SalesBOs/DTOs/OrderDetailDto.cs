using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesBOs.DTOs
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public decimal? Freight { get; set; }
        public List<OrderDetailItemDto> OrderDetails { get; set; } = new List<OrderDetailItemDto>();
    }
}
