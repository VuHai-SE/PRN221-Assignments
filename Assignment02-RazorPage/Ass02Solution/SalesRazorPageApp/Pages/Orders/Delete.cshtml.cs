using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesRepositories;
using Microsoft.Extensions.Logging;
using SalesBOs;
using Microsoft.AspNetCore.Authorization;

namespace SalesRazorPageApp.Pages.Orders
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(IOrderRepository orderRepository, ILogger<DeleteModel> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        [BindProperty]
        public Order Order { get; set; }

        public IActionResult OnGet(int id)
        {
            _logger.LogInformation("Fetching order with ID {OrderId} for deletion", id);
            Order = _orderRepository.GetOrderById2(id);
            if (Order == null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found", id);
                return NotFound();
            }
            return Page();
        }


        public IActionResult OnPost(int id)
        {
            try
            {
                _logger.LogInformation("Deleting order with ID {OrderId}", id);
                var order = _orderRepository.GetOrderById2(id);

                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found", id);
                    return NotFound();
                }

                _orderRepository.DeleteOrder(id);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order with ID {OrderId}", id);
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the order.");
                return Page();
            }
        }

    }
}
