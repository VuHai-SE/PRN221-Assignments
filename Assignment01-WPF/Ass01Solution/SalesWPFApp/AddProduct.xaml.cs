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
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        private readonly IProductRepository _productRepository;

        public AddProduct(IProductRepository productRepository)
        {
            InitializeComponent();
            _productRepository = productRepository;
        }

        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Product Name" || textBox.Text == "Category ID" || textBox.Text == "Weight" || textBox.Text == "Unit Price" || textBox.Text == "Units In Stock")
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private void AddPlaceholderText(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox == ProductNameTextBox) textBox.Text = "Product Name";
                else if (textBox == CategoryIdTextBox) textBox.Text = "Category ID";
                else if (textBox == WeightTextBox) textBox.Text = "Weight";
                else if (textBox == UnitPriceTextBox) textBox.Text = "Unit Price";
                else if (textBox == UnitsInStockTextBox) textBox.Text = "Units In Stock";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new Product
            {
                ProductName = ProductNameTextBox.Text,
                CategoryId = int.Parse(CategoryIdTextBox.Text),
                Weight = WeightTextBox.Text,
                UnitPrice = decimal.Parse(UnitPriceTextBox.Text),
                UnitsInStock = int.Parse(UnitsInStockTextBox.Text)
            };

            _productRepository.Add(newProduct);
            this.DialogResult = true; // Đánh dấu rằng sản phẩm mới đã được thêm thành công
            this.Close();
        }
    }
}
