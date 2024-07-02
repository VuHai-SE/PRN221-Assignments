using Microsoft.EntityFrameworkCore;
using SalesBOs;
using SalesBOs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SalesDAOs
{
    public class OrderDAO
    {
        private readonly PRN_AssignmentContext _context;

        public OrderDAO(PRN_AssignmentContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderDetailDto> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.Member)
                .Select(o => new OrderDetailDto
                {
                    OrderId = o.OrderId,
                    CompanyName = o.Member.CompanyName,
                    Email = o.Member.Email,
                    Country = o.Member.Country,
                    OrderDate = o.OrderDate,
                    RequiredDate = o.RequiredDate,
                    ShippedDate = o.ShippedDate,
                    Freight = o.Freight
                }).ToList();
        }

        public int FindOrderIdByMemberId (int memberId)
        {
            return _context.Orders.SingleOrDefault(m => m.MemberId == memberId).OrderId;
        }

        public OrderDetailDto GetOrderById(int OrderId)
        {
            var Ord = _context.Orders.SingleOrDefault(o => o.OrderId == OrderId);
            var Mem = _context.Members.SingleOrDefault(m => m.MemberId == Ord.MemberId);
            var OrderDto = new OrderDetailDto();
            OrderDto.OrderId = Ord.OrderId;
            OrderDto.CompanyName = Mem.CompanyName;
            OrderDto.Email = Mem.Email;
            OrderDto.Country = Mem.Country;
            OrderDto.OrderDate = Ord.OrderDate;
            OrderDto.RequiredDate = Ord.RequiredDate;
            OrderDto.ShippedDate = Ord.ShippedDate;
            OrderDto.Freight = Ord.Freight;

            return OrderDto;
        }

        public IEnumerable<OrderDetailDto> SearchOrders(string keyword)
        {
            int.TryParse(keyword, out int id);
            return _context.Orders
                .Include(o => o.Member)
                .Where(o => o.OrderId == id || o.Member.CompanyName.Contains(keyword))
                .Select(o => new OrderDetailDto
                {
                    OrderId = o.OrderId,
                    CompanyName = o.Member.CompanyName,
                    Email = o.Member.Email,
                    Country = o.Member.Country,
                    OrderDate = o.OrderDate,
                    RequiredDate = o.RequiredDate,
                    ShippedDate = o.ShippedDate,
                    Freight = o.Freight
                }).ToList();
        }

        public IEnumerable<OrderDetailItemDto> GetOrderDetails(int orderId)
        {
            return _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Product)
                .Select(od => new OrderDetailItemDto
                {
                    ProductId = od.ProductId,
                    CategoryId = (int)od.Product.CategoryId,
                    ProductName = od.Product.ProductName,
                    UnitPrice = od.UnitPrice,
                    Quantity = od.Quantity,
                    Discount = (float)od.Discount
                }).ToList();
        }

        public int GetNextOrderId()
        {
            return _context.Orders.Any() ? _context.Orders.Max(o => o.OrderId) + 1 : 1;
        }

        public void AddOrder(Order order, List<OrderDetail> orderDetails)
        {
            // Đảm bảo OrderId được tạo tự động
            order.OrderId = GetNextOrderId();

            _context.Orders.Add(order);
            foreach (var detail in orderDetails)
            {
                detail.OrderId = order.OrderId;
                _context.OrderDetails.Add(detail);
            }
            _context.SaveChanges();
        }


        public void UpdateOrder(Order order, List<OrderDetail> orderDetails)
        {
            var existingOrder = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.OrderId == order.OrderId);
            if (existingOrder != null)
            {
                _context.Entry(existingOrder).CurrentValues.SetValues(order);

                // Xóa các OrderDetails cũ
                foreach (var detail in existingOrder.OrderDetails.ToList())
                {
                    _context.OrderDetails.Remove(detail);
                }

                // Thêm các OrderDetails mới
                foreach (var detail in orderDetails)
                {
                    existingOrder.OrderDetails.Add(detail);
                }

                _context.SaveChanges();
            }
        }


        public void DeleteOrder(int orderId)
        {
            var order = _context.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                // Xóa các OrderDetails liên quan trước khi xóa Order
                foreach (var detail in order.OrderDetails.ToList())
                {
                    _context.OrderDetails.Remove(detail);
                }

                // Xóa Order
                _context.Orders.Remove(order);

                // Lưu thay đổi
                _context.SaveChanges();
            }
        }




        public Order GetOrder(int orderId)
        {
            return _context.Orders.Find(orderId);
        }


        public IEnumerable<OrderDetailDto> GetOrdersByMemberId(int memberId)
        {
            return _context.Orders
                .Where(o => o.MemberId == memberId)
                .Include(o => o.Member)
                .Select(o => new OrderDetailDto
                {
                    OrderId = o.OrderId,
                    CompanyName = o.Member.CompanyName,
                    Email = o.Member.Email,
                    Country = o.Member.Country,
                    OrderDate = o.OrderDate,
                    RequiredDate = o.RequiredDate,
                    ShippedDate = o.ShippedDate,
                    Freight = o.Freight
                }).ToList();
        }

        public IEnumerable<OrderDetailDto> SearchOrdersByMemberId(string keyword, int memberId)
        {
            int.TryParse(keyword, out int id);
            return _context.Orders
                .Where(o => o.MemberId == memberId && (o.OrderId == id || o.Member.CompanyName.Contains(keyword)))
                .Include(o => o.Member)
                .Select(o => new OrderDetailDto
                {
                    OrderId = o.OrderId,
                    CompanyName = o.Member.CompanyName,
                    Email = o.Member.Email,
                    Country = o.Member.Country,
                    OrderDate = o.OrderDate,
                    RequiredDate = o.RequiredDate,
                    ShippedDate = o.ShippedDate,
                    Freight = o.Freight
                }).ToList();
        }

        //Dashboard
        public IEnumerable<SalesReportDto> GetSalesReportByPeriod(DateTime startDate, DateTime endDate)
        {
            // Truy vấn dữ liệu từ cơ sở dữ liệu
            var orderDetails = _context.OrderDetails
                .Where(od => od.Order.OrderDate >= startDate && od.Order.OrderDate <= endDate)
                .Select(od => new
                {
                    od.Order.OrderDate,
                    od.UnitPrice,
                    od.Quantity,
                    od.Discount
                })
                .ToList();

            // Tính toán tổng cộng sau khi dữ liệu đã được tải về
            var salesReport = orderDetails
                .GroupBy(od => od.OrderDate)
                .Select(g => new SalesReportDto
                {
                    Date = g.Key,
                    TotalSales = g.Sum(od => od.UnitPrice * od.Quantity * (1 - (decimal)od.Discount))
                })
                .OrderByDescending(r => r.TotalSales)
                .ToList();

            return salesReport;
        }


    }
}
