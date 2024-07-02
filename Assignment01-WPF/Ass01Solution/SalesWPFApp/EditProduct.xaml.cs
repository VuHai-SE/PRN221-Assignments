using SalesBOs;
using SalesRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SalesWPFApp
{
    /// <summary>
    /// Interaction logic for EditProduct.xaml
    /// </summary>
    public partial class EditProduct : Window
    {
        private readonly IProductRepository _productRepository;
        private readonly Product _product;

        public EditProduct(IProductRepository productRepository, Product product)
        {
            InitializeComponent();
            _productRepository = productRepository;
            _product = product;
            LoadProductDetails();
        }

        private void LoadProductDetails()
        {
            ProductNameTextBox.Text = _product.ProductName;
            CategoryIdTextBox.Text = _product.CategoryId.ToString();
            WeightTextBox.Text = _product.Weight;
            UnitPriceTextBox.Text = _product.UnitPrice.ToString();
            UnitsInStockTextBox.Text = _product.UnitsInStock.ToString();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _product.ProductName = ProductNameTextBox.Text;
            _product.CategoryId = int.Parse(CategoryIdTextBox.Text);
            _product.Weight = WeightTextBox.Text;
            _product.UnitPrice = decimal.Parse(UnitPriceTextBox.Text);
            _product.UnitsInStock = int.Parse(UnitsInStockTextBox.Text);

            _productRepository.Update(_product);
            this.DialogResult = true;
            this.Close();
        }
    }
}
