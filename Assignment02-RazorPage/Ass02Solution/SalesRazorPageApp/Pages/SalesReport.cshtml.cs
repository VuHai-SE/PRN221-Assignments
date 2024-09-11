using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesRepositories;
using SalesBOs.DTOs;
using System;
using System.Collections.Generic;

namespace SalesRazorPageApp.Pages.Reports
{
    [Authorize (Roles = "Admin")]
    public class SalesReportModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;

        public SalesReportModel(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public IList<SalesReportDto> SalesReport { get; set; } = new List<SalesReportDto>();

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-30); // Mặc định là 30 ngày trước

        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; } = DateTime.Now; // Mặc định là ngày hiện tại

        public void OnGet()
        {
            if (StartDate != DateTime.MinValue && EndDate != DateTime.MinValue)
            {
                SalesReport = _orderRepository.GetSalesReportByPeriod(StartDate, EndDate).ToList();
            }
        }
    }
}
