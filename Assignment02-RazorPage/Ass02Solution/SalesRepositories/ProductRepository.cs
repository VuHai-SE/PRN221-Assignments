using SalesBOs;
using SalesDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDAO _productDAO;

        public ProductRepository(ProductDAO productDAO)
        {
            _productDAO = productDAO;
        }

        public IEnumerable<Product> GetAll()
        {
            return _productDAO.GetAllProducts();
        }

        public Product GetById(int id)
        {
            return _productDAO.GetProductById(id);
        }

        public void Add(Product product)
        {
            _productDAO.AddProduct(product);
        }

        public void Update(Product product)
        {
            _productDAO.UpdateProduct(product);
        }

        public void Delete(int id)
        {
            _productDAO.DeleteProduct(id);
        }
        public IEnumerable<Product> SearchProducts(string keyword)
        {
            return _productDAO.SearchProducts(keyword);
        }
    }
}
