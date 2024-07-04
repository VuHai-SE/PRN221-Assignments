﻿using SalesBOs.DTOs;
using SalesBOs;
using SalesDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDAO _orderDAO;

        public OrderRepository(OrderDAO orderDAO)
        {
            _orderDAO = orderDAO;
        }

        public IEnumerable<OrderDetailDto> GetAllOrders()
        {
            return _orderDAO.GetAllOrders();
        }

        public int FindOrderIdByMemberId(int memberId)
        {
            return _orderDAO.FindOrderIdByMemberId(memberId);
        }

        public OrderDetailDto GetOrderById(int OrderId)
        {
            return _orderDAO.GetOrderById(OrderId);
        }

        public void AddOrder(OrderDetailDto orderDto)
        {
            var order = new Order
            {
                OrderId = _orderDAO.GetNextOrderId(),  // Tạo OrderId mới
                MemberId = orderDto.MemberId,
                OrderDate = orderDto.OrderDate,
                RequiredDate = orderDto.RequiredDate,
                ShippedDate = orderDto.ShippedDate,
                Freight = orderDto.Freight
            };

            var orderDetails = orderDto.OrderDetails.Select(od => new OrderDetail
            {
                OrderId = order.OrderId,  // Thiết lập OrderId cho từng OrderDetail
                ProductId = od.ProductId,
                UnitPrice = od.UnitPrice,
                Quantity = od.Quantity,
                Discount = od.Discount
            }).ToList();

            _orderDAO.AddOrder(order, orderDetails);
        }


        public void UpdateOrder(OrderDetailDto orderDto)
        {
            var order = new Order
            {
                OrderId = orderDto.OrderId,
                MemberId = orderDto.MemberId,
                OrderDate = orderDto.OrderDate,
                RequiredDate = orderDto.RequiredDate,
                ShippedDate = orderDto.ShippedDate,
                Freight = orderDto.Freight
            };

            var orderDetails = orderDto.OrderDetails.Select(od => new OrderDetail
            {
                ProductId = od.ProductId,
                UnitPrice = od.UnitPrice,
                Quantity = od.Quantity,
                Discount = od.Discount
            }).ToList();

            _orderDAO.UpdateOrder(order, orderDetails);
        }

        public void DeleteOrder(int orderId)
        {
            _orderDAO.DeleteOrder(orderId);
        }

        public IEnumerable<OrderDetailDto> SearchOrders(string keyword)
        {
            return _orderDAO.SearchOrders(keyword);
        }

        public IEnumerable<OrderDetailItemDto> GetOrderDetails(int orderId)
        {
            return _orderDAO.GetOrderDetails(orderId);
        }

        public IEnumerable<OrderDetailDto> GetOrdersByMemberId(int memberId)
        {
            return _orderDAO.GetOrdersByMemberId(memberId);
        }

        public IEnumerable<OrderDetailDto> SearchOrdersByMemberId(string keyword, int memberId)
        {
            return _orderDAO.SearchOrdersByMemberId(keyword, memberId);
        }

        public IEnumerable<SalesReportDto> GetSalesReportByPeriod(DateTime startDate, DateTime endDate)
        {
            return _orderDAO.GetSalesReportByPeriod(startDate, endDate);
        }

        public IEnumerable<OrderDetailDto> GetAllOrderDetails()
        {
            return _orderDAO.GetAllOrders();
        }
    }
}
