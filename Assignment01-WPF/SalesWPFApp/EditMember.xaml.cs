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
    /// Interaction logic for EditMember.xaml
    /// </summary>
    public partial class EditMember : Window
    {
        private readonly IMemberRepository _memberRepository;
        private readonly Member _member;

        public EditMember(IMemberRepository memberRepository, Member member)
        {
            InitializeComponent();
            _memberRepository = memberRepository;
            _member = member;
            LoadMemberDetails();
        }

        private void LoadMemberDetails()
        {
            EmailTextBox.Text = _member.Email;
            CompanyNameTextBox.Text = _member.CompanyName;
            CityTextBox.Text = _member.City;
            CountryTextBox.Text = _member.Country;
            PasswordTextBox.Text = _member.Password;

            SetTextBoxPlaceholder(EmailTextBox, "Email");
            SetTextBoxPlaceholder(CompanyNameTextBox, "Company Name");
            SetTextBoxPlaceholder(CityTextBox, "City");
            SetTextBoxPlaceholder(CountryTextBox, "Country");
            SetTextBoxPlaceholder(PasswordTextBox, "Password");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            _member.Email = EmailTextBox.Text;
            _member.CompanyName = CompanyNameTextBox.Text;
            _member.City = CityTextBox.Text;
            _member.Country = CountryTextBox.Text;
            _member.Password = PasswordTextBox.Text;

            _memberRepository.Update(_member);
            this.DialogResult = true;
            this.Close();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Foreground == Brushes.Gray)
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
                SetTextBoxPlaceholder(textBox, GetPlaceholderText(textBox.Name));
            }
        }

        private void SetTextBoxPlaceholder(TextBox textBox, string placeholderText)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Foreground = Brushes.Gray;
                textBox.Text = placeholderText;
            }
        }

        private string GetPlaceholderText(string textBoxName)
        {
            return textBoxName switch
            {
                "EmailTextBox" => "Email",
                "CompanyNameTextBox" => "Company Name",
                "CityTextBox" => "City",
                "CountryTextBox" => "Country",
                "PasswordTextBox" => "Password",
                _ => ""
            };
        }
    }
}
