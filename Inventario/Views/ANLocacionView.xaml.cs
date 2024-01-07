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
    /// Lógica de interacción para ANLocacionView.xaml
    /// </summary>
    public partial class ANLocacionView : UserControl
    {
        private decimal idItemG = 0;
        private readonly DataGridsInformation dataGridsInformation = new();
        private Button btnDelete = new()
        {
            Content = "Delete relation",
        };
        private Button btnEdit = new()
        {
            Content = "Edit",
            Margin = new Thickness(15, 0, 15, 0)
        };
        private Button btnUpdate = new()
        {
            Content = "Update"
        };

        // Constructor para crear nuevas relaciones de area de negocio y localizacion
        public ANLocacionView()
        {
            InitializeComponent();
            ConfigureView(0, "create");
        }

        // Constructor para ver y editar relaciones de area de negocio y localizacion
        public ANLocacionView(decimal idItem, bool isAdmin)
        {
            InitializeComponent();
            if (isAdmin)
            {
                ConfigureView(idItem, "edit");
            }
            else
            {
                ConfigureView(idItem, "view");
            }
        }

        private void ConfigureView(decimal idItem, string configType)
        {
            BindComboBoxInfo();
            // Edit and view
            if (idItem != 0)
            {
                FillFields(idItem);
                ActivateFields(false);
                btnCleanDeleteDockPanel.Children.Clear();
                btnCreateUpdateDockPanel.Children.Clear();
                btnCancel.Content = "Exit";
                if (configType == "view")
                {
                    lblTitle.Content = "Business area - location relation view";
                }
                else if (configType == "edit")
                {
                    btnEdit.Click += BtnEdit_Click;
                    editButtonStackPanel.Children.Add(btnEdit);
                    idItemG = Convert.ToDecimal(idItem);
                    lblTitle.Content = "Business area - location relation edit";
                    btnDelete.Click += BtnDelete_Click;
                    btnCleanDeleteDockPanel.Children.Add(btnDelete);
                }
            }
            // Create
            else
            {
                if (configType == "create")
                {
                    // Nothing for now
                }
            }
        }

        private void BindComboBoxInfo()
        {
            using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
            {
                // Business area ComboBox
                DataTable dataTable = new();
                connection.Open();
                SqlCommand command = new(dataGridsInformation.SelectBusinessArea(), connection)
                {
                    CommandType = CommandType.Text
                };
                SqlDataAdapter adapter = new(command);
                adapter.Fill(dataTable);
                DataRow newRow = dataTable.NewRow();
                newRow["ID_BA"] = 0;
                newRow["ACDE"] = "--SELECT--";
                dataTable.Rows.InsertAt(newRow, 0);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    businessAreaComboBox.ItemsSource = dataTable.DefaultView;
                }
                businessAreaComboBox.DisplayMemberPath = "ACDE";
                businessAreaComboBox.SelectedValuePath = "ID_BA";
                businessAreaComboBox.SelectedIndex = 0;

                // Location ComboBox
                dataTable = new();
                command = new(dataGridsInformation.SelectLocation(), connection)
                {
                    CommandType = CommandType.Text
                };
                adapter = new(command);
                adapter.Fill(dataTable);
                newRow = dataTable.NewRow();
                newRow["ID_LOCATION"] = 0;
                newRow["DEAC"] = "--SELECT--";
                dataTable.Rows.InsertAt(newRow, 0);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    locationComboBox.ItemsSource = dataTable.DefaultView;
                }
                locationComboBox.DisplayMemberPath = "DEAC";
                locationComboBox.SelectedValuePath = "ID_LOCATION";
                locationComboBox.SelectedIndex = 0;
            }
        }

        private void FillFields(decimal idBAL)
        {
            using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
            {
                connection.Open();
                SqlCommand command = new(dataGridsInformation.SelectBAL("form"), connection)
                {
                    CommandType = CommandType.Text
                };
                command.Parameters.AddWithValue("@ID", idBAL);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    businessAreaComboBox.SelectedValue = reader["ID_BA"].ToString();
                    locationComboBox.SelectedValue = reader["ID_LOCATION"].ToString();
                }
            }
        }

        private void FieldsEventSwitch(bool isActive)
        {
            if (isActive)
            {
                businessAreaComboBox.SelectionChanged += ComboBox_SelectionChanged;
                locationComboBox.SelectionChanged += ComboBox_SelectionChanged;
                btnUpdate.IsEnabled = false;
            }
            else
            {
                businessAreaComboBox.SelectionChanged -= ComboBox_SelectionChanged;
                locationComboBox.SelectionChanged -= ComboBox_SelectionChanged;
            }
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

        private void ActivateFields(bool isActive)
        {
            businessAreaComboBox.IsEnabled = isActive;
            locationComboBox.IsEnabled = isActive;
        }

        #region "Validations"

        private bool IsInformationValid()
        {
            bool isValid = false;
            RemoveAlerts();
            if (IsBusinessAreaValid())
            {
                if (IsLocationValid())
                {
                    if (IsRelationBALValid())
                    {
                        isValid = true;
                    }
                }
            }
            return isValid;
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
                businessAreaBorder.BorderBrush = Brushes.Red;
                businessAreaComboBox.Focus();
                return false;
            }
        }

        private bool IsLocationValid()
        {
            if (locationComboBox.SelectedIndex > 0)
            {
                return true;
            }
            else
            {
                statusBar.Content = "A location must be selected";
                locationBorder.BorderBrush = Brushes.Red;
                locationComboBox.Focus();
                return false;
            }
        }

        private bool IsRelationBALValid()
        {
            if (Decimal.TryParse(businessAreaComboBox.SelectedValue.ToString(), out decimal idBA))
            {
                if (Decimal.TryParse(locationComboBox.SelectedValue.ToString(), out decimal idLocation))
                {
                    using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                    {
                        connection.Open();
                        SqlCommand command = new(dataGridsInformation.SelectBAL("baLocation"), connection)
                        {
                            CommandType = CommandType.Text
                        };
                        command.Parameters.AddWithValue("@IBA", idBA);
                        command.Parameters.AddWithValue("@IL", idLocation);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            statusBar.Content = "The relation is already registered";
                            businessAreaBorder.BorderBrush = Brushes.Red;
                            locationBorder.BorderBrush = Brushes.Red;
                            businessAreaComboBox.Focus();
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        #endregion

        private void RemoveAlerts()
        {
            statusBar.Content = "";
            businessAreaBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            locationBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
        }

        private void CleanFields()
        {
            businessAreaComboBox.SelectedIndex = 0;
            locationComboBox.SelectedIndex = 0;
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
                if (Decimal.TryParse(businessAreaComboBox.SelectedValue.ToString(), out decimal idBA))
                {
                    if (Decimal.TryParse(locationComboBox.SelectedValue.ToString(), out decimal idLocation))
                    {
                        using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                        {
                            connection.Open();
                            SqlCommand command = new(dataGridsInformation.Inserts("baLocation"), connection)
                            {
                                CommandType = CommandType.Text
                            };
                            command.Parameters.AddWithValue("@IBA", idBA);
                            command.Parameters.AddWithValue("@IL", idLocation);
                            if (command.ExecuteNonQuery() != 0)
                            {
                                MessageBox.Show("Business area - location relation created successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Sorry, business area - location relation could not be created", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (MessageBox.Show("Are you sure you want to delete this relation?", "Information",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(dataGridsInformation.DeleteCommand("BA_LOCATION"), connection)
                        {
                            CommandType = CommandType.Text
                        };
                        command.Parameters.AddWithValue("@Id", idItemG);
                        if (command.ExecuteNonQuery() != 0)
                        {
                            MessageBox.Show("Business area - location relation deleted successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        CleanFields();
                        CloseWindow();
                    }
                }
                catch
                {
                    MessageBox.Show("This record cannot be deleted because there are records that depend on it.", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        //if (command.ExecuteNonQuery() != 0)
        //            {

        //            }

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
                if (Decimal.TryParse(businessAreaComboBox.SelectedValue.ToString(), out decimal idBA))
                {
                    if (Decimal.TryParse(locationComboBox.SelectedValue.ToString(), out decimal idLocation))
                    {
                        using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                        {
                            connection.Open();
                            SqlCommand command = new(dataGridsInformation.UpdateTables("baLocation"), connection)
                            {
                                CommandType = CommandType.Text
                            };
                            command.Parameters.AddWithValue("@IBA", idBA);
                            command.Parameters.AddWithValue("@IL", idLocation);
                            command.Parameters.AddWithValue("@IBAL", idItemG);
                            if (command.ExecuteNonQuery() != 0)
                            {
                                MessageBox.Show("Business area - location relation updated successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
    }
}
