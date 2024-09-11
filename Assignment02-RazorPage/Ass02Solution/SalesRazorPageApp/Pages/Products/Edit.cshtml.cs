using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesBOs;
using SalesDAOs;
using SalesRepositories;

namespace SalesRazorPageApp.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<EditModel> _logger;

        public EditModel(IProductRepository productRepository, ILogger<EditModel> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [BindProperty]
        public Product Product { get; set; }

        public IActionResult OnGet(int id)
        {
            _logger.LogWarning("huhuuu");
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra nếu ProductId từ form có giá trị hợp lệ
            if (Product.ProductId <= 0)
            {
                _logger.LogWarning("Invalid ProductId {ProductId} provided for update.", Product.ProductId);
                return RedirectToPage("/Error");
            }

            try
            {
                var productFromDb = _productRepository.GetById(Product.ProductId);
                if (productFromDb == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found for update.", Product.ProductId);
                    return RedirectToPage("/Error");
                }

                _productRepository.Update(Product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductId))
                {
                    _logger.LogWarning("Product with ID {ProductId} no longer exists.", Product.ProductId);
                    return RedirectToPage("/Error");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product with ID {ProductId}.", Product.ProductId);
                return RedirectToPage("/Error");
            }

            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
            return _productRepository.GetById(id) != null;
        }
    }
}
