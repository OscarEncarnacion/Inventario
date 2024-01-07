using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Lógica de interacción para ProtoPrueba.xaml
    /// </summary>
    public partial class ProtoPruebaView : UserControl
    {
        private decimal idUserG = 0;
        private decimal idItemG = 0;
        private readonly DataGridsInformation dataGridsInformation = new();
        private Button btnDelete = new()
        {
            Content = "Delete VN",
        };
        private Button btnEdit = new()
        {
            Content = "Edit VN",
            Margin = new Thickness(15, 0, 15, 0)
        };
        private Button btnUpdate = new()
        {
            Content = "Update"
        };

        // Constructor to create new VN
        public ProtoPruebaView(decimal idUser)
        {
            InitializeComponent();
            ConfigureView(0, idUser, "create");
        }

        // Constructor to view and edit VN
        public ProtoPruebaView(decimal idItem, decimal idUser, bool isAdmin)
        {
            InitializeComponent();
            if (isAdmin)
            {
                ConfigureView(idItem, idUser, "edit");
            }
            else
            {
                ConfigureView(idItem, idUser, "view");
            }
        }

        private void ConfigureView(decimal idItem, decimal idUser, string configType)
        {
            BindComboBoxInfo();
            idUserG = Convert.ToDecimal(idUser);
            if (idItem != 0)
            {
                FillFields(idItem);
                ActivateFields(false);
                btnCleanDeleteDockPanel.Children.Clear();
                btnCreateUpdateDockPanel.Children.Clear();
                btnCancel.Content = "Exit";
                if (configType == "view")
                {
                    lblTitle.Content = "VN view";
                }
                else if (configType == "edit")
                {
                    btnEdit.Click += BtnEdit_Click;
                    editButtonStackPanel.Children.Add(btnEdit);
                    idItemG = Convert.ToDecimal(idItem);
                    lblTitle.Content = "VN edit";
                    btnDelete.Click += BtnDelete_Click;
                    btnCleanDeleteDockPanel.Children.Add(btnDelete);
                }
            }
            else
            {
                if (configType == "create")
                {
                    dateReceptionDatePicker.SelectedDate = DateTime.Now;
                    dateLoadDatePicker.SelectedDate = DateTime.Now;
                }
            }
        }

        private void BindComboBoxInfo()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
            {
                // Project ComboBox
                DataTable dataTable = new();
                connection.Open();
                SqlCommand command = new(dataGridsInformation.SelectProject(), connection)
                {
                    CommandType = CommandType.Text
                };
                SqlDataAdapter adapter = new(command);
                adapter.Fill(dataTable);
                DataRow newRow = dataTable.NewRow();
                newRow["ID_PROJECT"] = 0;
                newRow["ACRONYM"] = "--SELECT--";
                dataTable.Rows.InsertAt(newRow, 0);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    projectComboBox.ItemsSource = dataTable.DefaultView;
                }
                projectComboBox.DisplayMemberPath = "ACRONYM";
                projectComboBox.SelectedValuePath = "ID_PROJECT";
                projectComboBox.SelectedIndex = 0;
            }
        }

        private void FillFields(decimal idVN)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
            {
                connection.Open();
                SqlCommand command = new(dataGridsInformation.SelectVN("form"), connection)
                {
                    CommandType = CommandType.Text
                };
                command.Parameters.AddWithValue("@ID", idVN);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    serialNumberTextBox.Text = reader["SERIAL_NUMBER"].ToString();
                    modelTextBox.Text = reader["MODEL"].ToString();
                    tagTextBox.Text = reader["TAG"].ToString();
                    typeLicenseTextBox.Text = reader["TYPE_LICENSE"].ToString();
                    linkTextBox.Text = reader["LINK"].ToString();
                    projectComboBox.SelectedValue = reader["ID_PROJECT"].ToString();
                    responsibleTextBox.Text = reader["RESPONSIBLE"].ToString();
                    currentAssignedTextBox.Text = reader["CURRENT_ASSIGNED"].ToString();
                    if (DateTime.TryParse(reader["DATE_RECEPTION"].ToString(), out DateTime dateReception))
                    {
                        dateReceptionDatePicker.SelectedDate = dateReception;
                    }
                    if (DateTime.TryParse(reader["DATE_LOAD"].ToString(), out DateTime dateLoad))
                    {
                        dateLoadDatePicker.SelectedDate = dateLoad;
                    }
                    commentsTextBox.Text = reader["COMMENTS"].ToString();
                }
            }
        }

        private void FieldsEventSwitch(bool isActive)
        {
            if (isActive)
            {
                serialNumberTextBox.TextChanged += TextBox_TextChanged;
                modelTextBox.TextChanged += TextBox_TextChanged;
                tagTextBox.TextChanged += TextBox_TextChanged;
                typeLicenseTextBox.TextChanged += TextBox_TextChanged;
                linkTextBox.TextChanged += TextBox_TextChanged;
                projectComboBox.SelectionChanged += ComboBox_SelectionChanged;
                responsibleTextBox.TextChanged += TextBox_TextChanged;
                currentAssignedTextBox.TextChanged += TextBox_TextChanged;
                dateReceptionDatePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
                dateLoadDatePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
                commentsTextBox.TextChanged += TextBox_TextChanged;
                btnUpdate.IsEnabled = false;
            }
            else
            {
                serialNumberTextBox.TextChanged -= TextBox_TextChanged;
                modelTextBox.TextChanged -= TextBox_TextChanged;
                tagTextBox.TextChanged -= TextBox_TextChanged;
                typeLicenseTextBox.TextChanged -= TextBox_TextChanged;
                linkTextBox.TextChanged -= TextBox_TextChanged;
                projectComboBox.SelectionChanged -= ComboBox_SelectionChanged;
                responsibleTextBox.TextChanged -= TextBox_TextChanged;
                currentAssignedTextBox.TextChanged -= TextBox_TextChanged;
                dateReceptionDatePicker.SelectedDateChanged -= DatePicker_SelectedDateChanged;
                dateLoadDatePicker.SelectedDateChanged -= DatePicker_SelectedDateChanged;
                commentsTextBox.TextChanged -= TextBox_TextChanged;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            btnUpdate.IsEnabled = true;
            FieldsEventSwitch(false);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex != 0)
            {
                btnUpdate.IsEnabled = true;
                ((Border)(((ComboBox)sender).Parent)).BorderBrush = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
                FieldsEventSwitch(false);
            }
        }

        private void DatePicker_SelectedDateChanged(object? sender, SelectionChangedEventArgs e)
        {
            btnUpdate.IsEnabled = true;
            if (sender != null)
            {
                ((DatePicker)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnUpdate.IsEnabled = true;
            ((TextBox)sender).BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            FieldsEventSwitch(false);
        }

        private void ActivateFields(bool isActive)
        {
            serialNumberTextBox.IsEnabled = isActive;
            modelTextBox.IsEnabled = isActive;
            tagTextBox.IsEnabled = isActive;
            typeLicenseTextBox.IsEnabled = isActive;
            linkTextBox.IsEnabled = isActive;
            projectComboBox.IsEnabled = isActive;
            responsibleTextBox.IsEnabled = isActive;
            currentAssignedTextBox.IsEnabled = isActive;
            dateReceptionDatePicker.IsEnabled = isActive;
            dateLoadDatePicker.IsEnabled = isActive;
            commentsTextBox.IsEnabled = isActive;
        }

        #region "Validations"

        private bool IsInformationValid()
        {
            bool isValid = false;
            RemoveAlerts();
            if (IsSerialNumberValid())
            {
                if (IsModelValid())
                {
                    if (IsTagValid())
                    {
                        if (IsTypeLicenseValid())
                        {
                            if (IsLinkValid())
                            {
                                if (IsProjectValid())
                                {
                                    if (IsResponsibleValid())
                                    {
                                        if (IsCurrentAssignedValid())
                                        {
                                            if (IsDateReceptionValid())
                                            {
                                                if (IsDateLoadValid())
                                                {
                                                    if (IsCommentsValid())
                                                    {
                                                        isValid = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return isValid;
        }

        private bool IsCommentsValid()
        {
            if (commentsTextBox.Text.Length > 0 && commentsTextBox.Text.Length < 401)
            {
                return true;
            }
            else
            {
                statusBar.Content = "The comments must not be empty";
                commentsTextBox.BorderBrush = Brushes.Red;
                commentsTextBox.Focus();
                return false;
            }
        }

        private bool IsDateLoadValid()
        {
            if (dateLoadDatePicker.SelectedDate != null)
            {
                return true;
            }
            else
            {
                statusBar.Content = "You must select an load date";
                dateLoadDatePicker.BorderBrush = Brushes.Red;
                dateLoadDatePicker.Focus();
                return false;
            }
        }

        private bool IsDateReceptionValid()
        {
            if (dateReceptionDatePicker.SelectedDate != null)
            {
                return true;
            }
            else
            {
                statusBar.Content = "You must select an reception date";
                dateReceptionDatePicker.BorderBrush = Brushes.Red;
                dateReceptionDatePicker.Focus();
                return false;
            }
        }

        private bool IsCurrentAssignedValid()
        {
            if (currentAssignedTextBox.Text.Length > 0 && currentAssignedTextBox.Text.Length < 51)
            {
                return true;
            }
            else
            {
                statusBar.Content = "The current assigned field must not be empty";
                currentAssignedTextBox.BorderBrush = Brushes.Red;
                currentAssignedTextBox.Focus();
                return false;
            }
        }

        private bool IsResponsibleValid()
        {
            if (responsibleTextBox.Text.Length > 0 && responsibleTextBox.Text.Length < 51)
            {
                return true;
            }
            else
            {
                statusBar.Content = "The responsible field must not be empty";
                responsibleTextBox.BorderBrush = Brushes.Red;
                responsibleTextBox.Focus();
                return false;
            }
        }

        private bool IsProjectValid()
        {
            if (projectComboBox.SelectedIndex > 0)
            {
                return true;
            }
            else
            {
                statusBar.Content = "A project must be selected";
                projectBorder.BorderBrush = Brushes.Red;
                projectComboBox.Focus();
                return false;
            }
        }

        private bool IsLinkValid()
        {
            if (linkTextBox.Text.Length > 0 && linkTextBox.Text.Length < 51)
            {
                return true;
            }
            else
            {
                statusBar.Content = "The link field must not be empty";
                linkTextBox.BorderBrush = Brushes.Red;
                linkTextBox.Focus();
                return false;
            }
        }

        private bool IsTypeLicenseValid()
        {
            if (typeLicenseTextBox.Text.Length > 0 && typeLicenseTextBox.Text.Length < 51)
            {
                return true;
            }
            else
            {
                statusBar.Content = "The type license field must not be empty";
                typeLicenseTextBox.BorderBrush = Brushes.Red;
                typeLicenseTextBox.Focus();
                return false;
            }
        }

        private bool IsTagValid()
        {
            if (tagTextBox.Text.Length > 0 && tagTextBox.Text.Length < 51)
            {
                return true;
            }
            else
            {
                statusBar.Content = "The tag field must not be empty";
                tagTextBox.BorderBrush = Brushes.Red;
                tagTextBox.Focus();
                return false;
            }
        }

        private bool IsModelValid()
        {
            if (modelTextBox.Text.Length > 0 && modelTextBox.Text.Length < 51)
            {
                return true;
            }
            else
            {
                statusBar.Content = "The model field must not be empty";
                modelTextBox.BorderBrush = Brushes.Red;
                modelTextBox.Focus();
                return false;
            }
        }

        private bool IsSerialNumberValid()
        {
            if (serialNumberTextBox.Text.Length > 0 && serialNumberTextBox.Text.Length < 51)
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new(dataGridsInformation.SelectVN("serialNumber"), connection)
                    {
                        CommandType = CommandType.Text
                    };
                    command.Parameters.AddWithValue("@SN", serialNumberTextBox.Text);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (lblTitle.Content.ToString() == "VN edit")
                        {
                            if (reader["ID_VN"].ToString() == idItemG.ToString())
                            {
                                return true;
                            }
                        }
                        statusBar.Content = "The serial number is already registered";
                        serialNumberTextBox.BorderBrush = Brushes.Red;
                        serialNumberTextBox.Focus();
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
                statusBar.Content = "The serial number field must not be empty";
                serialNumberTextBox.BorderBrush = Brushes.Red;
                serialNumberTextBox.Focus();
                return false;
            }
        }

        #endregion

        private void RemoveAlerts()
        {
            statusBar.Content = "";
            serialNumberTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            modelTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            tagTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            typeLicenseTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            linkTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            projectBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            responsibleTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            currentAssignedTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            dateReceptionDatePicker.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            dateLoadDatePicker.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            commentsTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
        }

        private void CleanFields()
        {
            serialNumberTextBox.Text = string.Empty;
            modelTextBox.Text = string.Empty;
            tagTextBox.Text = string.Empty;
            typeLicenseTextBox.Text = string.Empty;
            linkTextBox.Text = string.Empty;
            projectComboBox.SelectedIndex = 0;
            responsibleTextBox.Text = string.Empty;
            currentAssignedTextBox.Text = string.Empty;
            dateReceptionDatePicker.SelectedDate = DateTime.Now;
            dateLoadDatePicker.SelectedDate = DateTime.Now;
            commentsTextBox.Text = string.Empty;
        }

        private void CloseWindow()
        {
            EditWindow actualWindow = (EditWindow)Window.GetWindow(this);
            actualWindow.Close();
        }

        /// <summary>
        /// This is an example
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClean_Click(object sender, RoutedEventArgs e)
        {
            CleanFields();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (IsInformationValid())
            {
                if (Decimal.TryParse(projectComboBox.SelectedValue.ToString(), out decimal idProject))
                {
                    decimal idBAL = 0;
                    using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                    {
                        connection.Open();
                        SqlCommand command = new(dataGridsInformation.SelectUser("balocation"), connection)
                        {
                            CommandType = CommandType.Text
                        };
                        command.Parameters.AddWithValue("@ID", idUserG);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            if (Decimal.TryParse(reader["ID_BA_LOCATION"].ToString(), out decimal idBALtemp))
                            {
                                idBAL = idBALtemp;
                            }
                        }
                        reader.Close();
                        if (idBAL > 0)
                        {
                            command = new(dataGridsInformation.Inserts("vn"), connection)
                            {
                                CommandType = CommandType.Text
                            };
                            command.Parameters.AddWithValue("@SN", serialNumberTextBox.Text);
                            command.Parameters.AddWithValue("@TA", tagTextBox.Text);
                            command.Parameters.AddWithValue("@MO", modelTextBox.Text);
                            command.Parameters.AddWithValue("@TL", typeLicenseTextBox.Text);
                            command.Parameters.AddWithValue("@IBL", idBAL);
                            command.Parameters.AddWithValue("@IP", idProject);
                            command.Parameters.AddWithValue("@RE", responsibleTextBox.Text);
                            command.Parameters.AddWithValue("@CA", currentAssignedTextBox.Text);
                            command.Parameters.AddWithValue("@CO", commentsTextBox.Text);
                            command.Parameters.AddWithValue("@LI", linkTextBox.Text);
                            command.Parameters.AddWithValue("@IU", idUserG);
                            command.Parameters.AddWithValue("@DR", dateReceptionDatePicker.SelectedDate);
                            command.Parameters.AddWithValue("@DL", dateLoadDatePicker.SelectedDate);
                            if (command.ExecuteNonQuery() != 0)
                            {
                                MessageBox.Show("VN created successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Sorry, VN could not be created", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            CleanFields();
                            CloseWindow();
                        }
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this VN?", "Information",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(dataGridsInformation.DeleteCommand("VN"), connection)
                    {
                        CommandType = CommandType.Text
                    };
                    command.Parameters.AddWithValue("@Id", idItemG);
                    command.ExecuteNonQuery();
                    MessageBox.Show("VN deleted successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                    CleanFields();
                    CloseWindow();
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            ActivateFields(true);
            btnUpdate.Click += BtnUpdate_Click;
            btnCreateUpdateDockPanel.Children.Add(btnUpdate);
            FieldsEventSwitch(true);
            editButtonStackPanel.Children.Clear();
            btnCancel.Content = "Cancel";
            btnCleanDeleteDockPanel.Children.Clear();
        }



        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (IsInformationValid())
            {
                if (Decimal.TryParse(projectComboBox.SelectedValue.ToString(), out decimal idProject))
                {
                    using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                    {
                        connection.Open();
                        SqlCommand command = new(dataGridsInformation.UpdateTables("vn"), connection)
                        {
                            CommandType = CommandType.Text
                        };
                        command.Parameters.AddWithValue("@SN", serialNumberTextBox.Text);
                        command.Parameters.AddWithValue("@TA", tagTextBox.Text);
                        command.Parameters.AddWithValue("@MO", modelTextBox.Text);
                        command.Parameters.AddWithValue("@TL", typeLicenseTextBox.Text);
                        command.Parameters.AddWithValue("@IP", idProject);
                        command.Parameters.AddWithValue("@RE", responsibleTextBox.Text);
                        command.Parameters.AddWithValue("@CA", currentAssignedTextBox.Text);
                        command.Parameters.AddWithValue("@CO", commentsTextBox.Text);
                        command.Parameters.AddWithValue("@LI", linkTextBox.Text);
                        command.Parameters.AddWithValue("@IU", idUserG);
                        command.Parameters.AddWithValue("@DR", dateReceptionDatePicker.SelectedDate);
                        command.Parameters.AddWithValue("@DL", dateLoadDatePicker.SelectedDate);
                        command.Parameters.AddWithValue("@ID", idItemG);
                        if (command.ExecuteNonQuery() != 0)
                        {
                            MessageBox.Show("VN updated successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
    }
}
