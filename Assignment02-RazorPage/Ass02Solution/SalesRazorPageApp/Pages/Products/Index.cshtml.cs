using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SalesBOs;
using SalesDAOs;
using SalesRepositories;

namespace SalesRazorPageApp.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductRepository _productRepository;

        public IndexModel(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IList<Product> Product { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_productRepository.GetAll != null)
            {
                Product = _productRepository.GetAll().ToList();
            }
        }
    }
}
