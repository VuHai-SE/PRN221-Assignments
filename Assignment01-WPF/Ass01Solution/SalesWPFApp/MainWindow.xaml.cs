using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SalesBOs;
using SalesBOs.DTOs;
using SalesRepos;

namespace SalesWPFApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<OrderDetailDto> Orders { get; set; }
        public ObservableCollection<OrderDetailItemDto> OrderDetails { get; set; }
        public ObservableCollection<Member> Members { get; set; }
        public ObservableCollection<SalesReportDto> SalesReports { get; set; }

        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMemberRepository _memberRepository;

        public bool IsAdmin { get; set; }
        public int LoggedMemberId;

        public MainWindow(IProductRepository productRepository, IOrderRepository orderRepository, IMemberRepository memberRepository)
        {
            InitializeComponent();
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _memberRepository = memberRepository;

            Products = new ObservableCollection<Product>(_productRepository.GetAll());
            Orders = new ObservableCollection<OrderDetailDto>();
            OrderDetails = new ObservableCollection<OrderDetailItemDto>();
            Members = new ObservableCollection<Member>();
            SalesReports = new ObservableCollection<SalesReportDto>();
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrders();
            LoadMembers();
            if (!IsAdmin)
            {
                // Disable or hide UI elements for non-admin users
                AddProductButton.Visibility = Visibility.Collapsed;
                EditProductButton.Visibility = Visibility.Collapsed;
                DeleteProductButton.Visibility = Visibility.Collapsed;
                btnAddMember.Visibility = Visibility.Collapsed;
                btnDeleteMember.Visibility = Visibility.Collapsed;

                // Disable or hide UI elements for Orders for non-admin users
                AddOrderButton.Visibility = Visibility.Collapsed;
                EditOrderButton.Visibility = Visibility.Collapsed;
                DeleteOrderButton.Visibility = Visibility.Collapsed;

                // Hide search and show all for members
                memberSearchTextBox.Visibility = Visibility.Collapsed;
                btnSearchMember.Visibility = Visibility.Collapsed;
                btnShowAllMembers.Visibility = Visibility.Collapsed;

                // Hide the Dashboard tab for non-admin users
                DashboardTab.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Show the Dashboard tab for admin users
                DashboardTab.Visibility = Visibility.Visible;
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddProduct(_productRepository)
            {
                Owner = this
            };
            if (addProductWindow.ShowDialog() == true)
            {
                LoadProducts(); // Refresh the product list if a new product is added
            }
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is Product selectedProduct)
            {
                var editProductWindow = new EditProduct(_productRepository, selectedProduct)
                {
                    Owner = this
                };
                if (editProductWindow.ShowDialog() == true)
                {
                    LoadProducts(); // Refresh the product list if a product is edited
                }
            }
            else
            {
                MessageBox.Show("Please select a product to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductDataGrid.SelectedItem is Product selectedProduct)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the product '{selectedProduct.ProductName}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _productRepository.Delete(selectedProduct.ProductId);
                    LoadProducts(); // Refresh the product list after deletion
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts(); // Hiển thị tất cả sản phẩm
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string keyword = searchTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadProducts(); // Hiển thị tất cả sản phẩm nếu ô tìm kiếm trống
            }
            else
            {
                var searchResults = _productRepository.SearchProducts(keyword);
                Products.Clear();
                foreach (var product in searchResults)
                {
                    Products.Add(product);
                }
            }
        }


        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == "Search by Product Name")
            {
                searchTextBox.Text = "";
                searchTextBox.Foreground = Brushes.Black;
            }
        }

        private void AddPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                searchTextBox.Text = "Search by Product Name";
                searchTextBox.Foreground = Brushes.Gray;
            }
        }

        private void LoadProducts()
        {
            Products.Clear();
            foreach (var product in _productRepository.GetAll())
            {
                Products.Add(product);
            }
        }

        private void RefreshOrders_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            Orders.Clear();
            if (IsAdmin)
            {
                foreach (var order in _orderRepository.GetAllOrders())
                {
                    Orders.Add(order);
                }
            }
            else
            {
                var userOrders = _orderRepository.GetOrdersByMemberId(LoggedMemberId);
                foreach (var order in userOrders)
                {
                    Orders.Add(order);
                }
            }
        }


        private void SearchOrders_Click(object sender, RoutedEventArgs e)
        {
            string keyword = orderSearchTextBox.Text.Trim();
            Orders.Clear();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadOrders(); // Hiển thị tất cả đơn hàng nếu ô tìm kiếm trống
            }
            else
            {
                if (IsAdmin)
                {
                    var searchResults = _orderRepository.SearchOrders(keyword);
                    foreach (var order in searchResults)
                    {
                        Orders.Add(order);
                    }
                }
                else
                {
                    var searchResults = _orderRepository.SearchOrdersByMemberId(keyword, LoggedMemberId);
                    foreach (var order in searchResults)
                    {
                        Orders.Add(order);
                    }
                }
            }
        }


        private void OrderDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem is OrderDetailDto selectedOrder)
            {
                LoadOrderDetails(selectedOrder.OrderId);
            }
        }

        private void LoadOrderDetails(int orderId)
        {
            OrderDetails.Clear();
            foreach (var detail in _orderRepository.GetOrderDetails(orderId))
            {
                OrderDetails.Add(detail);
            }
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            var addOrderWindow = new AddOrder(_orderRepository, _productRepository, _memberRepository)
            {
                Owner = this
            };
            if (addOrderWindow.ShowDialog() == true)
            {
                LoadOrders(); // Refresh the order list if a new order is added
            }
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem is OrderDetailDto selectedOrder)
            {
                var editOrderWindow = new EditOrder(_orderRepository, _productRepository, _memberRepository, selectedOrder)
                {
                    Owner = this
                };
                if (editOrderWindow.ShowDialog() == true)
                {
                    LoadOrders(); // Refresh the order list if an order is edited
                }
            }
            else
            {
                MessageBox.Show("Please select an order to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrderDataGrid.SelectedItem is OrderDetailDto selectedOrder)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the order '{selectedOrder.OrderId}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _orderRepository.DeleteOrder(selectedOrder.OrderId);
                    LoadOrders(); // Refresh the order list after deletion
                }
            }
            else
            {
                MessageBox.Show("Please select an order to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void RemoveOrderPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (orderSearchTextBox.Text == "Search by Order ID or Company Name")
            {
                orderSearchTextBox.Text = "";
                orderSearchTextBox.Foreground = Brushes.Black;
            }
        }

        private void AddOrderPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(orderSearchTextBox.Text))
            {
                orderSearchTextBox.Text = "Search by Order ID or Company Name";
                orderSearchTextBox.Foreground = Brushes.Gray;
            }
        }

        private void LoadMembers()
        {
            if (IsAdmin)
            {
                Members.Clear();
                foreach (var member in _memberRepository.GetAllMembers())
                {
                    Members.Add(member);
                }
            }
            else
            {
                var member = _memberRepository.GetMemberById(LoggedMemberId);
                Members.Clear();
                Members.Add(member);
            }
        }

        private void SearchMembers_Click(object sender, RoutedEventArgs e)
        {
            string keyword = memberSearchTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                LoadMembers(); // Hiển thị tất cả thành viên nếu ô tìm kiếm trống
            }
            else
            {
                var searchResults = _memberRepository.SearchMembers(keyword);
                Members.Clear();
                foreach (var member in searchResults)
                {
                    Members.Add(member);
                }
            }
        }

        private void ShowAllMembers_Click(object sender, RoutedEventArgs e)
        {
            LoadMembers(); // Hiển thị tất cả thành viên
        }

        private void RemoveMemberPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (memberSearchTextBox.Text == "Search by Member ID, Email, Company Name, City, Country")
            {
                memberSearchTextBox.Text = "";
                memberSearchTextBox.Foreground = Brushes.Black;
            }
        }

        private void AddMemberPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(memberSearchTextBox.Text))
            {
                memberSearchTextBox.Text = "Search by Member ID, Email, Company Name, City, Country";
                memberSearchTextBox.Foreground = Brushes.Gray;
            }
        }

        private void AddMember_Click(object sender, RoutedEventArgs e)
        {
            if (IsAdmin)
            {
                var addMemberWindow = new AddMember(_memberRepository)
                {
                    Owner = this
                };
                if (addMemberWindow.ShowDialog() == true)
                {
                    LoadMembers(); // Refresh the member list if a new member is added
                }
            }
        }

        private void EditMember_Click(object sender, RoutedEventArgs e)
        {
            Member selectedMember;
            if (!IsAdmin)
            {
                selectedMember = Members.FirstOrDefault(m => m.MemberId == LoggedMemberId);
            }
            else
            {
                selectedMember = MemberDataGrid.SelectedItem as Member;
            }

            if (selectedMember != null)
            {
                var editMemberWindow = new EditMember(_memberRepository, selectedMember)
                {
                    Owner = this
                };
                if (editMemberWindow.ShowDialog() == true)
                {
                    LoadMembers(); // Refresh the member list if a member is edited
                }
            }
            else
            {
                MessageBox.Show("Please select a member to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void DeleteMember_Click(object sender, RoutedEventArgs e)
        {
            if (IsAdmin && MemberDataGrid.SelectedItem is Member selectedMember)
            {
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete the member '{selectedMember.CompanyName}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _memberRepository.Delete(selectedMember.MemberId);
                    LoadMembers(); // Refresh the member list after deletion
                }
            }
            else
            {
                MessageBox.Show("Please select a member to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ReloadAllSalesReports_Click(object sender, RoutedEventArgs e)
        {
            LoadAllSalesReports();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DashboardTab.IsSelected && !SalesReports.Any())
            {
                LoadAllSalesReports();
            }
        }

        private void LoadAllSalesReports()
        {
            var startDate = new DateTime(1900, 1, 1); // Giá trị hợp lệ cho SQL Server
            var endDate = new DateTime(9999, 12, 31); // Giá trị hợp lệ cho SQL Server
            var allReports = _orderRepository.GetSalesReportByPeriod(startDate, endDate);
            SalesReports.Clear();
            foreach (var report in allReports)
            {
                SalesReports.Add(report);
            }

            SalesReportDataGrid.ItemsSource = SalesReports;
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (!StartDatePicker.SelectedDate.HasValue || !EndDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select both Start Date and End Date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var startDate = StartDatePicker.SelectedDate ?? new DateTime(1900, 1, 1);
            var endDate = EndDatePicker.SelectedDate ?? new DateTime(9999, 12, 31);

            var filteredReports = _orderRepository.GetSalesReportByPeriod(startDate, endDate);
            SalesReports.Clear();
            foreach (var report in filteredReports)
            {
                SalesReports.Add(report);
            }

            SalesReportDataGrid.ItemsSource = SalesReports;
        }
    }
}
