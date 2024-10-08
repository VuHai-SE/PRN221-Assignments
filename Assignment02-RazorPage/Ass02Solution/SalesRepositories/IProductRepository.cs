﻿using SalesBOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product GetById(int id);
        bool IsProductInOrders(int productId);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        IEnumerable<Product> SearchProducts(string keyword);
    }
}
