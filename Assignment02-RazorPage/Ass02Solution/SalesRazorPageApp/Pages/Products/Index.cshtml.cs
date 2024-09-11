// Pages/Products/Index.cshtml.cs

using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesBOs;
using SalesRepositories;

namespace SalesRazorPageApp.Pages.Products
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IProductRepository _productRepository;

        public IndexModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IList<Product> Product { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public void OnGet()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                Product = _productRepository.GetAll().ToList();
            }
            else
            {
                Product = _productRepository.SearchProducts(SearchTerm).ToList();
            }
        }
    }
}
