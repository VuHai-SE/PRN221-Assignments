using SalesBOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesDAOs
{
    public class OrderDetailDAO
    {
        private readonly PRN_Assignment02Context _context;

        public OrderDetailDAO(PRN_Assignment02Context context)
        {
            _context = context;
        }

        public void AddOrderDetail(OrderDetail orderDetails)
        {
            _context.OrderDetails.Add(orderDetails);
            _context.SaveChanges();
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            var existingOrderDetail = _context.OrderDetails
                .FirstOrDefault(od => od.OrderId == orderDetail.OrderId && od.ProductId == orderDetail.ProductId);

            if (existingOrderDetail != null)
            {
                _context.Entry(existingOrderDetail).CurrentValues.SetValues(orderDetail);
                _context.SaveChanges();
            }
        }

    }
}
