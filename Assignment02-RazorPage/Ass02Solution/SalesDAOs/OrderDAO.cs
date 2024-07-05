using SalesBOs.DTOs;
using SalesBOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SalesDAOs
{
    public class OrderDAO
    {
        private readonly PRN_Assignment02Context _context;
        private readonly ILogger<OrderDAO> _logger;

        public OrderDAO(PRN_Assignment02Context context, ILogger<OrderDAO> logger)
        {
            _context = context;
            _logger = logger;
        }

        //public IEnumerable<OrderDetailDto> GetAllOrders()
        //{
        //    return _context.Orders
        //        .Include(o => o.Member)
        //        .Select(o => new OrderDetailDto
        //        {
        //            OrderId = o.OrderId,
        //            CompanyName = o.Member.CompanyName,
        //            Email = o.Member.Email,
        //            Country = o.Member.Country,
        //            OrderDate = o.OrderDate,
        //            RequiredDate = o.RequiredDate,
        //            ShippedDate = o.ShippedDate,
        //            Freight = o.Freight
        //        }).ToList();
        //}

        public IEnumerable<OrderDetailDto> GetAllOrders()
        {
            _logger.LogInformation("Fetching all orders.");
            var orders = _context.Orders
                .Include(o => o.Member)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                .Select(o => new OrderDetailDto
                {
                    OrderId = o.OrderId,
                    MemberId = o.MemberId,
                    CompanyName = o.Member.CompanyName,
                    Email = o.Member.Email,
                    Country = o.Member.Country,
                    OrderDate = o.OrderDate,
                    RequiredDate = o.RequiredDate,
                    ShippedDate = o.ShippedDate,
                    Freight = o.Freight,
                    OrderDetails = o.OrderDetails.Select(od => new OrderDetailItemDto
                    {
                        ProductId = od.ProductId,
                        CategoryId = od.Product.CategoryId ?? 0,
                        ProductName = od.Product.ProductName,
                        UnitPrice = od.UnitPrice,
                        Quantity = od.Quantity,
                        Discount = (float?)(od.Discount ?? 0f)
                    }).ToList()
                }).ToList();

            if (!orders.Any())
            {
                _logger.LogWarning("No orders found.");
            }

            return orders;
        }





        public IEnumerable<Member> GetAllMembers()
        {
            return _context.Members.ToList();
        }


        public int FindOrderIdByMemberId(int memberId)
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

        //public void AddOrder(Order order, List<OrderDetail> orderDetails)
        //{
        //    try
        //    {
        //        _context.Orders.Add(order);
        //        _context.SaveChanges(); // Save to get OrderId for OrderDetails

        //        foreach (var detail in orderDetails)
        //        {
        //            detail.OrderId = order.OrderId;
        //            _context.OrderDetails.Add(detail);
        //        }

        //        _context.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error
        //        Console.WriteLine("Error adding order: " + ex.Message);
        //        throw;
        //    }
        //}

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void AddOrderDetails(IEnumerable<OrderDetail> orderDetails)
        {
            _logger.LogInformation("Adding order details."); // Add this line for logging
            _context.OrderDetails.AddRange(orderDetails);
            _context.SaveChanges();
            _logger.LogInformation("Order details added."); // Add this line for logging
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

        public void DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public void DeleteOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Remove(orderDetail);
            _context.SaveChanges();
        }

        public Order GetOrderById2(int orderId)
        {
            return _context.Orders
                .Include(o => o.Member)
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderId == orderId);
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