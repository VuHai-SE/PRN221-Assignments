using SalesBOs;
using SalesBOs.DTOs;
using SalesDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepos
{
    public interface IOrderRepository
    {
        IEnumerable<OrderDetailDto> GetAllOrders();
        IEnumerable<OrderDetailDto> SearchOrders(string keyword);
        IEnumerable<OrderDetailItemDto> GetOrderDetails(int orderId);
        OrderDetailDto GetOrderById(int OrderId);
        void AddOrder(OrderDetailDto order);
        void UpdateOrder(OrderDetailDto order);
        void DeleteOrder(int orderId);
        int FindOrderIdByMemberId(int memberId);
        IEnumerable<OrderDetailDto> GetOrdersByMemberId(int memberId);
        IEnumerable<OrderDetailDto> SearchOrdersByMemberId(string keyword, int memberId);
        IEnumerable<SalesReportDto> GetSalesReportByPeriod(DateTime startDate, DateTime endDate);
    }
}
