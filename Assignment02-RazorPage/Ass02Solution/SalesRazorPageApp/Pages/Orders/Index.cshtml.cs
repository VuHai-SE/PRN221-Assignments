using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalesBOs;
using SalesBOs.DTOs;
using SalesDAOs;
using SalesRepositories;

namespace SalesRazorPageApp.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;

        public IndexModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IList<OrderDetailDto> Order { get; set; } = new List<OrderDetailDto>();

        public void OnGet()
        {
            Order = _orderRepository.GetAllOrderDetails().ToList();
        }
    }
}
