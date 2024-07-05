using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SalesBOs
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }

        public int OrderId { get; set; }

        [Required(ErrorMessage = "Member is required.")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Order Date is required.")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? RequiredDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ShippedDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Freight must be a positive value.")]
        public decimal? Freight { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
