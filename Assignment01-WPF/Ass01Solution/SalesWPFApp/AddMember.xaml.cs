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
    /// Interaction logic for AddMember.xaml
    /// </summary>
    public partial class AddMember : Window
    {
        private readonly IMemberRepository _memberRepository;

        public AddMember(IMemberRepository memberRepository)
        {
            InitializeComponent();
            _memberRepository = memberRepository;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var member = new Member
            {
                Email = EmailTextBox.Text,
                CompanyName = CompanyNameTextBox.Text,
                City = CityTextBox.Text,
                Country = CountryTextBox.Text,
                Password = PasswordTextBox.Text
            };

            _memberRepository.Add(member);
            this.DialogResult = true;
            this.Close();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "Email" || textBox.Text == "Company Name" || textBox.Text == "City" || textBox.Text == "Country" || textBox.Text == "Password")
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
                    if (textBox.Name == "EmailTextBox") textBox.Text = "Email";
                    if (textBox.Name == "CompanyNameTextBox") textBox.Text = "Company Name";
                    if (textBox.Name == "CityTextBox") textBox.Text = "City";
                    if (textBox.Name == "CountryTextBox") textBox.Text = "Country";
                    if (textBox.Name == "PasswordTextBox") textBox.Text = "Password";
                }
            }
        }
    }
}
