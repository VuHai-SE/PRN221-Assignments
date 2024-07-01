using System.Collections.Generic;
using System.Linq;
using SalesBOs;
using Microsoft.EntityFrameworkCore;

namespace SalesDAOs
{
    public class ProductDAO
    {
        private readonly PRN_AssignmentContext _context;

        public ProductDAO(PRN_AssignmentContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.Find(id);
        }

        public void AddProduct(Product product)
        {
            product.ProductId = _context.Products.Any() ? _context.Products.Max(p => p.ProductId) + 1 : 1;
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Product> SearchProducts(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Equals(""))
            {
                return _context.Products.ToList();
            }

            int.TryParse(keyword, out int id);
            return _context.Products
                .Where(p => p.ProductId == id || p.ProductName.Contains(keyword))
                .ToList();
        }
    }
}
