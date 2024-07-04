using SalesBOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesDAOs
{
    public class ProductDAO
    {
        private readonly PRN_Assignment02Context _context;

        public ProductDAO(PRN_Assignment02Context context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == id);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = GetProductById(product.ProductId);
            if (existingProduct != null)
            {
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ProductName = product.ProductName;
                existingProduct.Weight = product.Weight;
                existingProduct.UnitPrice = product.UnitPrice;
                existingProduct.UnitsInStock = product.UnitsInStock;

                _context.SaveChanges();
            }
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
