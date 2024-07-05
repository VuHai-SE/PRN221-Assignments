using SalesBOs;
using SalesDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly OrderDetailDAO _orderDetailDAO;

        public OrderDetailRepository(OrderDetailDAO orderDetailDAO)
        {
            _orderDetailDAO = orderDetailDAO;
        }

        public void AddOrderDetail(OrderDetail orderDetails)
        {
            _orderDetailDAO.AddOrderDetail(orderDetails);
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            _orderDetailDAO.UpdateOrderDetail(orderDetail);
        }
    }
}
