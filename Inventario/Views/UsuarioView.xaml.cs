using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Lógica de interacción para UsuarioView.xaml
    /// </summary>
    public partial class UsuarioView : UserControl
    {
        private readonly DataGridsInformation dataGridsInformation = new();
        private readonly decimal idUser = 0;
        private readonly bool isHisPerfil = false;
        private TextBlock textBlockChangePassword = new()
        {
            Text = "Change Password",
            Foreground = Brushes.Blue,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = new Thickness(0, 30, 35, 0)
        };
        private CheckBox checkBoxIsAdmin = new()
        {
            Content = "Is admin",
            FontSize = 18,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 15, 0, 0)
        };
        private CheckBox checkBoxIsActive = new()
        {
            Content = "Is active",
            FontSize = 18,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 15, 0, 0)
        };
        private Label actualPasswordLabel = new()
        {
            Content = "Actual password",
            HorizontalAlignment = HorizontalAlignment.Left
        };
        private PasswordBox actualPasswordBox = new PasswordBox()
        {
            Name = "actualPasswordBox",
            MaxLength = 15
        };
        private Button btnEdit = new()
        {
            Content = "Edit user",
            Margin = new Thickness(15, 0, 15, 0)
        };


        // Contructor to create new user
        public UsuarioView()
        {
            InitializeComponent();
            BindBusinessArea();
            // Add methods in the future to create a user from an administrator user using isAdmin variable
            FieldsEventSwitch(true);
        }

        // Constructor to view and edit user
        public UsuarioView(decimal id, bool isAdmin, bool isPerfil)
        {
            InitializeComponent();
            BindBusinessArea();
            if (id > 0)
            {
                idUser = id;
                isHisPerfil = isPerfil;
                stackPanelFree.Children.Clear();
                ControlsConfigurator(id, isAdmin);
            }
            FieldsEventSwitch(true);
        }

        private void BindBusinessArea()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
            {
                DataGridsInformation dataGridsInformation = new DataGridsInformation();
                DataTable dtBusinessArea = new();
                connection.Open();
                SqlCommand command = new(dataGridsInformation.SelectBusinessAreaLocation(), connection)
                {
                    CommandType = CommandType.Text
                };
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dtBusinessArea);
                DataRow newRow = dtBusinessArea.NewRow();
                newRow["ID_BA_LOCATION"] = 0;
                newRow["BALOCATION"] = "--SELECT--";
                dtBusinessArea.Rows.InsertAt(newRow, 0);
                if (dtBusinessArea != null && dtBusinessArea.Rows.Count > 0)
                {
                    businessAreaComboBox.ItemsSource = dtBusinessArea.DefaultView;
                }
            }
            businessAreaComboBox.DisplayMemberPath = "BALOCATION";
            businessAreaComboBox.SelectedValuePath = "ID_BA_LOCATION";
            businessAreaComboBox.SelectedIndex = 0;
        }

        private void ControlsConfigurator(decimal id, bool isAdmin)
        {
            lblTitle.Content = "View User";
            btnCreate.Content = "Update";
            btnCancel.Content = "Exit";
            btnCreate.IsEnabled = false;
            btnClean.IsEnabled = false;
            gridButtons.Children.Remove(btnCreate);
            gridButtons.Children.Remove(btnClean);
            passwordStackPanel.Children.Clear();
            confirmPasswordStackPanel.Children.Clear();
            stackPanelFree.Children.Clear();
            FieldActivationSwitch(false);
            List<string?> userInfo = new();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
            {
                connection.Open();
                SqlCommand command = new(dataGridsInformation.SelectUser("edit"), connection)
                {
                    CommandType = CommandType.Text
                };
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    userInfo.Add(reader["NAME"].ToString());
                    userInfo.Add(reader["LAST_NAME"].ToString());
                    userInfo.Add(reader["EMPLOYEE_NUMBER"].ToString());
                    userInfo.Add(reader["EMAIL"].ToString());
                    userInfo.Add(reader["ID_BA_LOCATION"].ToString());
                    userInfo.Add(reader["ROLE"].ToString());
                    userInfo.Add(reader["STATUS"].ToString());
                }
            }
            if (userInfo.Count != 0)
            {
                nameTextBox.Text = userInfo[0];
                lastNameTextBox.Text = userInfo[1];
                employeeNumberTextBox.Text = userInfo[2];
                eMailTextBox.Text = userInfo[3];
                businessAreaComboBox.SelectedValue = userInfo[4];
                if (int.TryParse(userInfo[5], out int isAdminCheckBox))
                {
                    if (isAdminCheckBox == 1)
                    {
                        checkBoxIsAdmin.IsChecked = true;
                    }
                    else if (isAdminCheckBox == 2)
                    {
                        checkBoxIsAdmin.IsChecked = false;
                    }
                }
                if (bool.TryParse(userInfo[6], out bool isActiveCheckBox))
                {
                    if (isActiveCheckBox)
                    {
                        checkBoxIsActive.IsChecked = true;
                    }
                    else
                    {
                        checkBoxIsActive.IsChecked = false;
                    }
                }
                FieldsRoleConfigurator(isAdmin);
            }
        }

        private void FieldActivationSwitch(bool isActive)
        {
            nameTextBox.IsEnabled = isActive;
            lastNameTextBox.IsEnabled = isActive;
            employeeNumberTextBox.IsEnabled = isActive;
            eMailTextBox.IsEnabled = isActive;
            businessAreaComboBox.IsEnabled = isActive;
            checkBoxIsAdmin.IsEnabled = isActive;
            checkBoxIsActive.IsEnabled = isActive;
        }

        private void FieldsRoleConfigurator(bool isAdmin)
        {
            if (isAdmin)
            {
                btnEdit.Click += BtnEdit_Click;
                editButtonStackPanel.Children.Add(btnEdit);
                gridButtons.Children.Add(btnCreate);
                bool isUserAdmin = IsAdminUser(idUser);
                if (!isUserAdmin)
                {
                    textBlockChangePassword.MouseEnter += LblChangePassword_MouseEnter;
                    textBlockChangePassword.MouseLeave += LblChangePassword_MouseLeave;
                    textBlockChangePassword.MouseLeftButtonUp += TextBlockChangePassword_MouseLeftButtonUp;
                    passwordStackPanel.Children.Add(textBlockChangePassword);
                    stackPanelFree.Children.Add(checkBoxIsAdmin);
                    stackPanelFree.Children.Add(checkBoxIsActive);
                }
            }
            else
            {
                textBlockChangePassword.MouseEnter += LblChangePassword_MouseEnter;
                textBlockChangePassword.MouseLeave += LblChangePassword_MouseLeave;
                textBlockChangePassword.MouseLeftButtonUp += TextBlockChangePassword_MouseLeftButtonUp;
                passwordStackPanel.Children.Add(textBlockChangePassword);
            }
        }

        private bool IsAdminUser(decimal id)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
            {
                connection.Open();
                SqlCommand command = new(dataGridsInformation.SelectUserRole(), connection)
                {
                    CommandType = CommandType.Text
                };
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();
                string? role = "";
                if (reader.Read())
                {
                    role = reader["ROLE"].ToString();
                }
                if (role == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void ChangePasswordConfiguration()
        {
            lblTitle.Content = "Change Password";
            btnEdit.IsEnabled = false;
            editButtonStackPanel.Children.Clear();
            nameStackPanel.Children.Clear();
            lastNameStackPanel.Children.Clear();
            employeeNumberStackPanel.Children.Clear();
            eMailStackPanel.Children.Clear();
            businessAreaStackPanel.Children.Clear();
            passwordStackPanel.Children.Clear();
            stackPanelFree.Children.Clear();
            stackPanelFree.Children.Add(statusBarPassword);
            Grid.SetRow(stackPanelFree, 0);
            if (isHisPerfil)
            {
                nameStackPanel.Children.Add(actualPasswordLabel);
                nameStackPanel.Children.Add(actualPasswordBox);
                employeeNumberStackPanel.Children.Add(passwordLabel);
                employeeNumberStackPanel.Children.Add(passwordTextBox);
                businessAreaStackPanel.Children.Add(confirmPasswordLabel);
                businessAreaStackPanel.Children.Add(confirmPasswordTextBox);
            }
            else
            {
                nameStackPanel.Children.Add(passwordLabel);
                nameStackPanel.Children.Add(passwordTextBox);
                employeeNumberStackPanel.Children.Add(confirmPasswordLabel);
                employeeNumberStackPanel.Children.Add(confirmPasswordTextBox);
            }
            if (!gridButtons.Children.Contains(btnCreate))
            {
                gridButtons.Children.Add(btnCreate);
            }
            if (btnCreate.IsEnabled)
            {
                FieldsEventSwitch(true);
            }
        }

        private void LblChangePassword_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock lblTemp = (TextBlock)sender;
            lblTemp.TextDecorations = TextDecorations.Underline;
        }

        private void LblChangePassword_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock lblTemp = (TextBlock)sender;
            lblTemp.TextDecorations = null;
        }

        private void TextBlockChangePassword_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ChangePasswordConfiguration();
            btnCancel.Content = "Cancel";
        }

        #region "Validations"

        private bool IsNameValid()
        {
            if (nameTextBox.Text != "")
            {
                return true;
            }
            else
            {
                statusBar.Content = "The name field must not be empty";
                nameTextBox.BorderBrush = Brushes.Red;
                return false;
            }
        }

        private bool IsLastNameValid()
        {
            if (lastNameTextBox.Text != "")
            {
                return true;
            }
            else
            {
                statusBar.Content = "The last name field must not be empty";
                lastNameTextBox.BorderBrush = Brushes.Red;
                return false;
            }
        }

        private bool IsEmplooyeeNumberValid()
        {
            if (Decimal.TryParse(employeeNumberTextBox.Text, out decimal employeeNumber))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new(dataGridsInformation.SelectUser("employeeNumber"), connection)
                    {
                        CommandType = CommandType.Text
                    };
                    command.Parameters.AddWithValue("@EN", employeeNumber);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (btnCreate.Content.ToString() == "Update")
                        {
                            if (reader["ID_USER"].ToString() == idUser.ToString())
                            {
                                return true;
                            }
                        }
                        statusBar.Content = "The employee number is already registered";
                        employeeNumberTextBox.BorderBrush = Brushes.Red;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                statusBar.Content = "The employee number is not in a valid format";
                employeeNumberTextBox.BorderBrush = Brushes.Red;
                return false;
            }
        }

        private bool IsEmailValid()
        {
            if (IsEmailValidFormat(eMailTextBox.Text))
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new(dataGridsInformation.SelectUser("email"), connection)
                    {
                        CommandType = CommandType.Text
                    };
                    command.Parameters.AddWithValue("@EM", eMailTextBox.Text);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (btnCreate.Content.ToString() == "Update")
                        {
                            if (reader["ID_USER"].ToString() == idUser.ToString())
                            {
                                return true;
                            }
                        }
                        statusBar.Content = "The Email is already registered";
                        eMailTextBox.BorderBrush = Brushes.Red;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                statusBar.Content = "The Email is not in a valid format";
                eMailTextBox.BorderBrush = Brushes.Red;
                return false;
            }
        }

        private bool IsEmailValidFormat(string emailaddress)
        {
            string pattern = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
            Regex regex = new Regex(pattern);
            MatchCollection hits = regex.Matches(emailaddress);
            if (hits.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsBusinessAreaValid()
        {
            if (businessAreaComboBox.SelectedIndex > 0)
            {
                return true;
            }
            else
            {
                statusBar.Content = "A business area must be selected";
                borderComboBox.BorderBrush = Brushes.Red;
                return false;
            }
        }

        private bool IsPasswordValid()
        {
            if (lblTitle.Content.ToString() == "Change Password")
            {
                if (isHisPerfil)
                {
                    string ps = GetSHA256(actualPasswordBox.Password);
                    if (ps != null)
                    {
                        using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                        {
                            connection.Open();
                            SqlCommand command = new(dataGridsInformation.SelectUser("password"), connection)
                            {
                                CommandType = CommandType.Text
                            };
                            command.Parameters.AddWithValue("@ID", idUser);
                            SqlDataReader reader = command.ExecuteReader();
                            string? pass = String.Empty;
                            if (reader.Read())
                            {
                                pass = reader["PASSWORD"].ToString();
                            }
                            if (pass != ps)
                            {
                                statusBar.Content = "Incorrect password";
                                actualPasswordBox.BorderBrush = Brushes.Red;
                                return false;
                            }
                        }
                    }
                }
            }
            if (passwordTextBox.Password == confirmPasswordTextBox.Password)
            {
                if (IsPasswordFormatValid(passwordTextBox.Password))
                {
                    return true;
                }
                else
                {
                    statusBarPassword.Content = "Password specifications:\n8 characters minimum.\n15 characters" +
                        " maximum.\nAt least one capital letter.\nAt least one lowercase letter.\nAt least one di" +
                        "git.\nNo blank spaces.\nAt least one of the following special characters: $@$!%*?&";
                    passwordTextBox.BorderBrush = Brushes.Red;
                    confirmPasswordTextBox.BorderBrush = Brushes.Red;
                    return false;
                }
            }
            else
            {
                statusBar.Content = "Passwords don't match";
                passwordTextBox.BorderBrush = Brushes.Red;
                confirmPasswordTextBox.BorderBrush = Brushes.Red;
                return false;
            }
        }

        private bool IsPasswordFormatValid(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])([A-Za-z\d$@$!%*?&]|[^ ]){8,15}$";
            Regex regex = new Regex(pattern);
            MatchCollection hits = regex.Matches(password);
            if (hits.Count == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion  

        private bool IsInformationValid()
        {
            bool isValid = false;
            RemoveAlerts();
            if (lblTitle.Content.ToString() != "Change Password")
            {
                if (IsNameValid())
                {
                    if (IsLastNameValid())
                    {
                        if (IsEmplooyeeNumberValid())
                        {
                            if (IsEmailValid())
                            {
                                if (IsBusinessAreaValid())
                                {
                                    if (btnCreate.Content.ToString() == "Create")
                                    {
                                        if (IsPasswordValid())
                                        {
                                            isValid = true;
                                        }
                                    }
                                    else
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (IsPasswordValid())
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        private void RemoveAlerts()
        {
            statusBar.Content = "";
            statusBarPassword.Content = "";
            nameTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            lastNameTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            employeeNumberTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            eMailTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            borderComboBox.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            if (btnCreate.Content.ToString() == "Create")
            {
                passwordTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
                confirmPasswordTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            }
            if (lblTitle.Content.ToString() == "Change Password")
            {
                actualPasswordBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnCreate.IsEnabled = true;
            ((TextBox)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            FieldsEventSwitch(false);
        }

        private void BusinessAreaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (businessAreaComboBox.SelectedIndex != 0)
            {
                btnCreate.IsEnabled = true;
                borderComboBox.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                FieldsEventSwitch(false);
            }
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = (PasswordBox)sender;
            btnCreate.IsEnabled = true;
            if (passwordBox.Name != "actualPasswordBox")
            {
                passwordTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
                confirmPasswordTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            }
            else
            {
                passwordBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            }
            FieldsEventSwitch(false);
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            btnCreate.IsEnabled = true;
            FieldsEventSwitch(false);
        }

        private void FieldsEventSwitch(bool isActive)
        {
            if (isActive)
            {
                nameTextBox.TextChanged += TextBox_TextChanged;
                lastNameTextBox.TextChanged += TextBox_TextChanged;
                employeeNumberTextBox.TextChanged += TextBox_TextChanged;
                eMailTextBox.TextChanged += TextBox_TextChanged;
                businessAreaComboBox.SelectionChanged += BusinessAreaComboBox_SelectionChanged;
                checkBoxIsActive.Click += CheckBox_Click;
                checkBoxIsAdmin.Click += CheckBox_Click;
                actualPasswordBox.PasswordChanged += PasswordTextBox_PasswordChanged;
                passwordTextBox.PasswordChanged += PasswordTextBox_PasswordChanged;
                confirmPasswordTextBox.PasswordChanged += PasswordTextBox_PasswordChanged;
                btnCreate.IsEnabled = false;
            }
            else
            {
                nameTextBox.TextChanged -= TextBox_TextChanged;
                lastNameTextBox.TextChanged -= TextBox_TextChanged;
                employeeNumberTextBox.TextChanged -= TextBox_TextChanged;
                eMailTextBox.TextChanged -= TextBox_TextChanged;
                businessAreaComboBox.SelectionChanged -= BusinessAreaComboBox_SelectionChanged;
                checkBoxIsActive.Click -= CheckBox_Click;
                checkBoxIsAdmin.Click -= CheckBox_Click;
                actualPasswordBox.PasswordChanged -= PasswordTextBox_PasswordChanged;
                passwordTextBox.PasswordChanged -= PasswordTextBox_PasswordChanged;
                confirmPasswordTextBox.PasswordChanged -= PasswordTextBox_PasswordChanged;
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

        private void BtnClean_Click(object sender, RoutedEventArgs e)
        {
            CleanFields();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (btnCreate.Content.ToString() == "Create")
            {
                if (IsInformationValid())
                {
                    string ps = GetSHA256(passwordTextBox.Password);
                    if (ps.Length > 0)
                    {
                        if (Decimal.TryParse(employeeNumberTextBox.Text, out decimal employeeNumber))
                        {
                            if (Decimal.TryParse(businessAreaComboBox.SelectedValue.ToString(), out decimal idBusinessAreaLocation))
                            {
                                using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                                {
                                    connection.Open();
                                    SqlCommand command = new(dataGridsInformation.Inserts("user"), connection)
                                    {
                                        CommandType = CommandType.Text
                                    };
                                    command.Parameters.AddWithValue("@Name", nameTextBox.Text);
                                    command.Parameters.AddWithValue("@LastName", lastNameTextBox.Text);
                                    command.Parameters.AddWithValue("@EmployeeNumber", employeeNumber);
                                    command.Parameters.AddWithValue("@Email", eMailTextBox.Text);
                                    command.Parameters.AddWithValue("@ID_BA_LOCATION", idBusinessAreaLocation);
                                    command.Parameters.AddWithValue("@Password", ps);
                                    command.Parameters.AddWithValue("@Role", 2);
                                    command.Parameters.AddWithValue("@Status", true);
                                    if (command.ExecuteNonQuery() != 0)
                                    {
                                        MessageBox.Show("User created successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Sorry, user could not be created", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                    CleanFields();
                                    CloseWindow();
                                }
                            }
                        }
                    }
                }
            }
            else if (btnCreate.Content.ToString() == "Update")
            {
                if (idUser > 0)
                {
                    if (IsInformationValid())
                    {
                        if (lblTitle.Content.ToString() == "Edit User")
                        {
                            if (Decimal.TryParse(employeeNumberTextBox.Text, out decimal employeeNumber))
                            {
                                if (Decimal.TryParse(businessAreaComboBox.SelectedValue.ToString(), out decimal idBusinessAreaLocation))
                                {
                                    using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                                    {
                                        connection.Open();
                                        SqlCommand command = new(dataGridsInformation.UpdateUser("personalInformation"), connection)
                                        {
                                            CommandType = CommandType.Text
                                        };
                                        command.Parameters.AddWithValue("@ID", idUser);
                                        command.Parameters.AddWithValue("@Name", nameTextBox.Text);
                                        command.Parameters.AddWithValue("@LastName", lastNameTextBox.Text);
                                        command.Parameters.AddWithValue("@EmployeeNumber", employeeNumber);
                                        command.Parameters.AddWithValue("@Email", eMailTextBox.Text);
                                        command.Parameters.AddWithValue("@ID_BA_LOCATION", idBusinessAreaLocation);
                                        if (checkBoxIsAdmin.IsChecked == true)
                                        {
                                            command.Parameters.AddWithValue("@Role", 1);
                                        }
                                        else
                                        {
                                            command.Parameters.AddWithValue("@Role", 2);
                                        }
                                        if (checkBoxIsActive.IsChecked == true)
                                        {
                                            command.Parameters.AddWithValue("@Status", true);
                                        }
                                        else
                                        {
                                            command.Parameters.AddWithValue("@Status", false);
                                        }
                                        if (command.ExecuteNonQuery() != 0)
                                        {
                                            MessageBox.Show("User updated successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Sorry, the update could not be executed", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                                        }
                                        CleanFields();
                                        CloseWindow();
                                    }
                                }
                            }
                        }
                        else if (lblTitle.Content.ToString() == "Change Password")
                        {
                            string ps = GetSHA256(passwordTextBox.Password);
                            if (ps.Length > 0)
                            {
                                using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                                {
                                    connection.Open();
                                    SqlCommand command = new(dataGridsInformation.UpdateUser("password"), connection)
                                    {
                                        CommandType = CommandType.Text
                                    };
                                    command.Parameters.AddWithValue("@ID", idUser);
                                    command.Parameters.AddWithValue("@PASS", ps);
                                    if (command.ExecuteNonQuery() != 0)
                                    {
                                        MessageBox.Show("User password updated successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Sorry, the update could not be executed", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                    CleanFields();
                                    CloseWindow();
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Sorry, there is a problem whit the User ID", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            FieldsEventSwitch(true);
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            lblTitle.Content = "Edit User";
            btnCancel.Content = "Cancel";
            FieldActivationSwitch(true);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void CleanFields()
        {
            if (btnCreate.Content.ToString() == "Create")
            {
                nameTextBox.Text = string.Empty;
                lastNameTextBox.Text = string.Empty;
                employeeNumberTextBox.Text = string.Empty;
                eMailTextBox.Text = string.Empty;
                businessAreaComboBox.SelectedIndex = 0;
                passwordTextBox.Password = string.Empty;
                confirmPasswordTextBox.Password = string.Empty;
            }
            else if (btnCreate.Content.ToString() == "Update")
            {
                if (nameStackPanel.Children.Contains(nameTextBox))
                {
                    nameTextBox.Text = string.Empty;
                    lastNameTextBox.Text = string.Empty;
                    employeeNumberTextBox.Text = string.Empty;
                    eMailTextBox.Text = string.Empty;
                    businessAreaComboBox.SelectedIndex = 0;
                }
                else
                {
                    passwordTextBox.Password = string.Empty;
                    confirmPasswordTextBox.Password = string.Empty;
                }
            }
            RemoveAlerts();
        }

        private void CloseWindow()
        {
            EditWindow actualWindow = (EditWindow)Window.GetWindow(this);
            actualWindow.Close();
        }
    }
}
