using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesBOs;
using SalesRepositories;
using System.Linq;

namespace SalesRazorPageApp.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IProductRepository _productRepository;

        public CreateModel(IOrderRepository orderRepository,
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
        public Order Order { get; set; } = new Order();

        public SelectList Members { get; set; }
        public IList<Product> Products { get; set; }

        public IActionResult OnGet()
        {
            Members = new SelectList(_memberRepository.GetAllMembers(), "MemberId", "CompanyName");
            Products = _productRepository.GetAll().ToList();
            return Page();
        }

        public IActionResult OnPost()
        {
            //if (!ModelState.IsValid)
            //{
            //    Members = new SelectList(_memberRepository.GetAllMembers(), "MemberId", "CompanyName");
            //    Products = _productRepository.GetAll().ToList();
            //    return Page();
            //}

            _orderRepository.AddOrder(Order);

            foreach (var orderDetail in Request.Form["ProductId"].Select((_, i) => new OrderDetail
            {
                OrderId = Order.OrderId,
                ProductId = int.Parse(Request.Form["ProductId"][i]),
                UnitPrice = decimal.Parse(Request.Form["UnitPrice"][i]),
                Quantity = int.Parse(Request.Form["Quantity"][i]),
                Discount = string.IsNullOrEmpty(Request.Form["Discount"][i]) ? (double?)null : double.Parse(Request.Form["Discount"][i])
            }))
            {
                _orderDetailRepository.AddOrderDetail(orderDetail);
            }

            return RedirectToPage("./Index");
        }
    }
}
