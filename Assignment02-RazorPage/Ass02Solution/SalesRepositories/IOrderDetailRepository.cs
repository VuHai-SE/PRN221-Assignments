using SalesBOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public interface IOrderDetailRepository
    {
        void AddOrderDetail(OrderDetail orderDetails);
        void UpdateOrderDetail(OrderDetail orderDetail);
    }
}
