using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalesBOs;
using SalesDAOs;
using SalesRepositories;

namespace SalesRazorPageApp.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(IProductRepository productRepository, ILogger<DeleteModel> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [BindProperty]
        public Product Product { get; set; }
        public string ErrorMessage { get; set; }

        public IActionResult OnGet(int id)
        {
            try
            {
                Product = _productRepository.GetById(id);

                if (Product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found.", id);
                    return RedirectToPage("/Error");
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product with ID {ProductId}.", id);
                return RedirectToPage("/Error");
            }
        }

        public IActionResult OnPost()
        {
            if (Product.ProductId <= 0)
            {
                _logger.LogWarning("Invalid ProductId {ProductId} provided for delete.", Product.ProductId);
                return RedirectToPage("/Error");
            }

            try
            {
                var productFromDb = _productRepository.GetById(Product.ProductId);
                if (productFromDb == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for delete.", Product.ProductId);
                    return RedirectToPage("/Error");
                }

                if (_productRepository.IsProductInOrders(Product.ProductId))
                {
                    ErrorMessage = "Product cannot be deleted because it is being used in Orders.";
                    return Page();
                }

                _productRepository.Delete(Product.ProductId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product with ID {ProductId}.", Product.ProductId);
                return RedirectToPage("/Error");
            }

            return RedirectToPage("./Index");
        }
    }
}
