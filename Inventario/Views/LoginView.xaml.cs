using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventario.Views
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            List<string?> loginInfo = LoginValidation();
            if (loginInfo[0] == "true")
            {
                MainWindow actualWindow = (MainWindow)Window.GetWindow(this);
                actualWindow.MainContent.Content = new MenuView(loginInfo);
                actualWindow.MinWidth = ((MenuView)actualWindow.MainContent.Content).MinWidth + 17;
                actualWindow.MinHeight = ((MenuView)actualWindow.MainContent.Content).MinHeight + 43;
                actualWindow.Width = actualWindow.MinWidth;
                actualWindow.Height = actualWindow.MinHeight + 66;
            }
            else
            {
                connectionInformation.Content = "Username or password is not correct";
            }
        }

        private List<string?> LoginValidation()
        {
            if (Decimal.TryParse(tbUser.Text, out decimal employeeNumber))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                {
                    DataGridsInformation dataGridsInformation = new();
                    connection.Open();
                    SqlCommand command = new SqlCommand(dataGridsInformation.SelectUser("login"), connection)
                    {
                        CommandType = CommandType.Text
                    };
                    command.Parameters.AddWithValue("@EN", employeeNumber);
                    SqlDataReader reader = command.ExecuteReader();
                    string? pass = String.Empty;
                    List<string?> userInfo = new()
                    {
                        "false"
                    };
                    if (reader.Read())
                    {
                        pass = reader["PASSWORD"].ToString();
                        userInfo.Add(reader["ID_USER"].ToString());
                        userInfo.Add(reader["NAME"].ToString());
                        userInfo.Add(reader["LAST_NAME"].ToString());
                        userInfo.Add(reader["ROLE"].ToString());
                    }
                    if (GetSHA256(pbPassword.Password) == pass)
                    {
                        userInfo[0] = "true";
                        return userInfo;

                    }
                    else
                    {
                        return userInfo;
                    }
                }
            }
            else
            {
                return new List<string?> { "false" };
            }
        }

        private string GetSHA256(string str)
        {
            try
            {
                SHA256 sha256 = SHA256.Create();
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[]? stream = null;
                StringBuilder stringBuilder = new();
                stream = sha256.ComputeHash(encoding.GetBytes(str));
                for (int i = 0; i < stream.Length; i++)
                {
                    stringBuilder.AppendFormat("{0:x2}", stream[i]);
                }
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Password encryption error", MessageBoxButton.OK, MessageBoxImage.Error);
                return "";
            }
        }

        private void Register_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EditWindow editWindow = new("Users");
            editWindow.ShowDialog();
        }

        private void PbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbUser.Text))
            {
                connectionInformation.Content = string.Empty;
                btnLogin.IsEnabled = !string.IsNullOrEmpty(pbPassword.Password);
            }
        }

        private void TbUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(pbPassword.Password))
            {
                connectionInformation.Content = string.Empty;
                btnLogin.IsEnabled = !string.IsNullOrEmpty(tbUser.Text);
            }
        }

        private void EnterKeyLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return && e.Key != Key.Enter)
                return;
            e.Handled = true;
            List<string?> loginInfo = LoginValidation();
            if (loginInfo[0] == "true")
            {
                MainWindow actualWindow = (MainWindow)Window.GetWindow(this);
                actualWindow.MainContent.Content = new MenuView(loginInfo);
                actualWindow.MinWidth = ((MenuView)actualWindow.MainContent.Content).MinWidth + 17;
                actualWindow.MinHeight = ((MenuView)actualWindow.MainContent.Content).MinHeight + 43;
                actualWindow.Width = actualWindow.MinWidth;
                actualWindow.Height = actualWindow.MinHeight + 66;
            }
            else
            {
                connectionInformation.Content = "Username or password is not correct";
            }
        }
    }
}
