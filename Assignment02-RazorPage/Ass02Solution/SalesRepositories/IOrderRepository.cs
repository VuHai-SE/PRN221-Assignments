using SalesBOs;
using SalesBOs.DTOs;
using SalesDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public interface IOrderRepository
    {
        IEnumerable<OrderDetailDto> GetAllOrders();
        IEnumerable<Member> GetAllMembers();
        IEnumerable<OrderDetailDto> SearchOrders(string keyword);
        IEnumerable<OrderDetailItemDto> GetOrderDetails(int orderId);
        OrderDetailDto GetOrderById(int OrderId);
        Order GetOrderById2(int orderId);
        //void AddOrder(OrderDetailDto order);
        void AddOrder(Order order);
        void Add(Order order, List<OrderDetail> orderDetails);
        void UpdateOrder(OrderDetailDto order);
        public void UpdateOrder(Order order);
        void DeleteOrder(int orderId);
        int FindOrderIdByMemberId(int memberId);
        IEnumerable<OrderDetailDto> GetOrdersByMemberId(int memberId);
        IEnumerable<OrderDetailDto> SearchOrdersByMemberId(string keyword, int memberId);
        IEnumerable<SalesReportDto> GetSalesReportByPeriod(DateTime startDate, DateTime endDate);
        IEnumerable<OrderDetailDto> GetAllOrderDetails();
    }
}
