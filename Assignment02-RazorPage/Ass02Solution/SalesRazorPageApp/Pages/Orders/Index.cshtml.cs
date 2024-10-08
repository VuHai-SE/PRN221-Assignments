﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalesBOs;
using SalesBOs.DTOs;
using SalesDAOs;
using SalesRepositories;

namespace SalesRazorPageApp.Pages.Orders
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;

        public IndexModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public IList<OrderDetailDto> Order { get; set; } = new List<OrderDetailDto>();

        public void OnGet()
        {
            if (User.IsInRole("Admin"))
            {
                if (string.IsNullOrWhiteSpace(SearchTerm))
                {
                    Order = _orderRepository.GetAllOrderDetails().ToList();
                }
                else
                {
                    Order = _orderRepository.SearchOrders(SearchTerm).ToList();
                }
            }
            else
            {
                var memberId = int.Parse(User.FindFirst("MemberId").Value);
                if (string.IsNullOrWhiteSpace(SearchTerm))
                {
                    Order = _orderRepository.GetOrdersByMemberId(memberId).ToList();
                }
                else
                {
                    Order = _orderRepository.SearchOrdersByMemberId(SearchTerm, memberId).ToList();
                }
            }
        }
    }
}
