using SalesBOs.DTOs;
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
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMemberRepository _memberRepository;

        public AddOrder(IOrderRepository orderRepository, IProductRepository productRepository, IMemberRepository memberRepository)
        {
            InitializeComponent();
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _memberRepository = memberRepository;

            // Load members into ComboBox
            MemberComboBox.ItemsSource = _memberRepository.GetAllMembers();
            // Initialize OrderDetailsDataGrid
            OrderDetailsDataGrid.ItemsSource = new List<OrderDetailItemDto>();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var newOrder = new OrderDetailDto
            {
                OrderDate = OrderDatePicker.SelectedDate ?? DateTime.Now,
                RequiredDate = RequiredDatePicker.SelectedDate,
                ShippedDate = ShippedDatePicker.SelectedDate,
                Freight = decimal.TryParse(FreightTextBox.Text, out var freight) ? freight : 0,
                MemberId = (int)MemberComboBox.SelectedValue,
                OrderDetails = OrderDetailsDataGrid.Items.OfType<OrderDetailItemDto>().Select(detail =>
                {
                    var product = _productRepository.GetById(detail.ProductId);
                    detail.UnitPrice = product.UnitPrice;
                    return detail;
                }).ToList()
            };

            _orderRepository.AddOrder(newOrder);
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }

}
