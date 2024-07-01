using SalesBOs;
using SalesBOs.DTOs;
using SalesRepos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for EditOrder.xaml
    /// </summary>
    public partial class EditOrder : Window
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly OrderDetailDto _orderDetail;

        public ObservableCollection<Product> Products { get; set; }

        public EditOrder(IOrderRepository orderRepository, IProductRepository productRepository, IMemberRepository memberRepository, OrderDetailDto orderDetail)
        {
            InitializeComponent();
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _memberRepository = memberRepository;
            _orderDetail = orderDetail;

            Products = new ObservableCollection<Product>(_productRepository.GetAll());
            DataContext = this;

            // Load members into ComboBox
            MemberComboBox.ItemsSource = _memberRepository.GetAllMembers();

            // Load order details
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            OrderDatePicker.SelectedDate = _orderDetail.OrderDate;
            RequiredDatePicker.SelectedDate = _orderDetail.RequiredDate;
            ShippedDatePicker.SelectedDate = _orderDetail.ShippedDate;
            FreightTextBox.Text = _orderDetail.Freight.ToString();
            MemberComboBox.SelectedValue = _orderDetail.MemberId;
            OrderDetailsDataGrid.ItemsSource = _orderDetail.OrderDetails;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _orderDetail.OrderDate = OrderDatePicker.SelectedDate ?? DateTime.Now;
            _orderDetail.RequiredDate = RequiredDatePicker.SelectedDate;
            _orderDetail.ShippedDate = ShippedDatePicker.SelectedDate;
            _orderDetail.Freight = decimal.TryParse(FreightTextBox.Text, out var freight) ? freight : 0;
            _orderDetail.MemberId = (int)MemberComboBox.SelectedValue;
            _orderDetail.OrderDetails = OrderDetailsDataGrid.Items.OfType<OrderDetailItemDto>().Select(detail =>
            {
                var product = _productRepository.GetById(detail.ProductId);
                detail.UnitPrice = product.UnitPrice;
                return detail;
            }).ToList();

            _orderRepository.UpdateOrder(_orderDetail);
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
