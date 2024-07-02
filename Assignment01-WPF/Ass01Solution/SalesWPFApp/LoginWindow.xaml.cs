using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SalesRepos;

namespace SalesWPFApp
{
    public partial class LoginWindow : Window
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public LoginWindow(IMemberRepository memberRepository, IProductRepository productRepository, IOrderRepository orderRepository)
        {
            InitializeComponent();
            _memberRepository = memberRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text;
            var password = PasswordBox.Password;

            var member = _memberRepository.GetMemberByEmail(email);

            if (member != null && member.Password == password)
            {
                var mainWindow = new MainWindow(_productRepository, _orderRepository, _memberRepository)
                {
                    Owner = this
                };

                if (member.Email == "admin@fstore.com")
                {
                    mainWindow.IsAdmin = true;
                }
                else
                {
                    mainWindow.IsAdmin = false;
                    mainWindow.LoggedMemberId = member.MemberId;
                }
                mainWindow.Closed += (s, args) => this.Close(); // Đăng ký sự kiện Closed để đóng ứng dụng khi MainWindow bị đóng
                mainWindow.Show();
                this.Hide();
                //chỗ này để Hide thay vì Close bởi vì đây là cửa sổ chính của ứng dụng (startup window)
                //khi dùng this.Close() thì toàn bộ cửa sổ của app đều bị đóng
            }
            else
            {
                MessageBox.Show("Invalid email or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "Email")
                {
                    textBox.Text = "";
                    textBox.Foreground = Brushes.Black;
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Foreground = Brushes.Gray;
                    textBox.Text = "Email";
                }
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (passwordBox.Password == "Password")
                {
                    passwordBox.Password = "";
                    passwordBox.Foreground = Brushes.Black;
                }
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (string.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    passwordBox.Foreground = Brushes.Gray;
                    passwordBox.Password = "Password";
                }
            }
        }
    }
}
