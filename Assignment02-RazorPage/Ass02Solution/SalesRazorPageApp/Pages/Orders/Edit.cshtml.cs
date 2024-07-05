using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesBOs;
using SalesRepositories;
using System.Linq;

namespace SalesRazorPageApp.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IProductRepository _productRepository;

        public EditModel(IOrderRepository orderRepository,
                         IOrderDetailRepository orderDetailRepository,
                         IMemberRepository memberRepository,
                         IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _memberRepository = memberRepository;
            _productRepository = productRepository;
        }

        [BindProperty]
        public Order Order { get; set; }

        public SelectList Members { get; set; }
        public SelectList Products { get; set; }

        public IActionResult OnGet(int id)
        {
            Order = _orderRepository.GetOrderById2(id);
            if (Order == null)
            {
                return NotFound();
            }

            Members = new SelectList(_memberRepository.GetAllMembers(), "MemberId", "CompanyName");
            Products = new SelectList(_productRepository.GetAll().ToList(), "ProductId", "ProductName");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Members = new SelectList(_memberRepository.GetAllMembers(), "MemberId", "CompanyName");
                Products = new SelectList(_productRepository.GetAll().ToList(), "ProductId", "ProductName");
                return Page();
            }

            _orderRepository.UpdateOrder(Order);

            foreach (var orderDetail in Request.Form["ProductId"].Select((_, i) => new OrderDetail
            {
                OrderId = Order.OrderId,
                ProductId = int.Parse(Request.Form["ProductId"][i]),
                UnitPrice = decimal.Parse(Request.Form["UnitPrice"][i]),
                Quantity = int.Parse(Request.Form["Quantity"][i]),
                Discount = string.IsNullOrEmpty(Request.Form["Discount"][i]) ? (double?)null : double.Parse(Request.Form["Discount"][i])
            }))
            {
                _orderDetailRepository.UpdateOrderDetail(orderDetail);
            }

            return RedirectToPage("./Index");
        }
    }
}
