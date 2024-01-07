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
    /// Lógica de interacción para LocacionView.xaml
    /// </summary>
    public partial class LocacionView : UserControl
    {
        private decimal idItemG = 0;
        private readonly DataGridsInformation dataGridsInformation = new();
        private Button btnDelete = new()
        {
            Content = "Delete Location",
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

        // Constructor para crear una nueva locacion
        public LocacionView()
        {
            InitializeComponent();
            ConfigureView(0, "create");
        }

        // Constructor para ver o editar una locacion
        public LocacionView(decimal idItem, bool isAdmin)
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
                    lblTitle.Content = "Location view";
                }
                else if (configType == "edit")
                {
                    btnEdit.Click += BtnEdit_Click;
                    editButtonStackPanel.Children.Add(btnEdit);
                    idItemG = Convert.ToDecimal(idItem);
                    lblTitle.Content = "Location edit";
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

        private void FillFields(decimal idLocation)
        {
            using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
            {
                connection.Open();
                SqlCommand command = new(dataGridsInformation.SelectLocation("form"), connection)
                {
                    CommandType = CommandType.Text
                };
                command.Parameters.AddWithValue("@ID", idLocation);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    acronymTextBox.Text = reader["ACRONYM"].ToString();
                    descriptionTextBox.Text = reader["DESCRIPTION"].ToString();
                }
            }
        }

        private void FieldsEventSwitch(bool isActive)
        {
            if (isActive)
            {
                acronymTextBox.TextChanged += TextBox_TextChanged;
                descriptionTextBox.TextChanged += TextBox_TextChanged;
                btnUpdate.IsEnabled = false;
            }
            else
            {
                acronymTextBox.TextChanged -= TextBox_TextChanged;
                descriptionTextBox.TextChanged -= TextBox_TextChanged;
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
            acronymTextBox.IsEnabled = isActive;
            descriptionTextBox.IsEnabled = isActive;
        }

        #region "Validations"

        private bool IsInformationValid()
        {
            bool isValid = false;
            RemoveAlerts();
            if (IsAcronymValid())
            {
                if (IsDescriptionValid())
                {
                    isValid = true;
                }
            }
            return isValid;
        }

        private bool IsDescriptionValid()
        {
            if (descriptionTextBox.Text.Length > 0 && descriptionTextBox.Text.Length < 101)
            {
                return true;
            }
            else
            {
                statusBar.Content = "The description field must not be empty";
                descriptionTextBox.BorderBrush = Brushes.Red;
                descriptionTextBox.Focus();
                return false;
            }
        }

        private bool IsAcronymValid()
        {
            if (acronymTextBox.Text.Length > 0 && acronymTextBox.Text.Length < 21)
            {
                using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new(dataGridsInformation.SelectLocation("acronym"), connection)
                    {
                        CommandType = CommandType.Text
                    };
                    command.Parameters.AddWithValue("@AC", acronymTextBox.Text);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (lblTitle.Content.ToString() == "Location edit")
                        {
                            if (reader["ID_LOCATION"].ToString() == idItemG.ToString())
                            {
                                return true;
                            }
                        }
                        statusBar.Content = "The acronym is already registered";
                        acronymTextBox.BorderBrush = Brushes.Red;
                        acronymTextBox.Focus();
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
                statusBar.Content = "The acronym field must not be empty";
                acronymTextBox.BorderBrush = Brushes.Red;
                acronymTextBox.Focus();
                return false;
            }
        }

        #endregion

        private void RemoveAlerts()
        {
            statusBar.Content = "";
            acronymTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            descriptionTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
        }

        private void CleanFields()
        {
            acronymTextBox.Text = string.Empty;
            descriptionTextBox.Text = string.Empty;
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
                using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new(dataGridsInformation.Inserts("location"), connection)
                    {
                        CommandType = CommandType.Text
                    };
                    command.Parameters.AddWithValue("@AC", acronymTextBox.Text);
                    command.Parameters.AddWithValue("@DE", descriptionTextBox.Text);
                    if (command.ExecuteNonQuery() != 0)
                    {
                        MessageBox.Show("Location created successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Sorry, location could not be created", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    CleanFields();
                    CloseWindow();
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this location?", "Information",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(dataGridsInformation.DeleteCommand("LOCATION"), connection)
                        {
                            CommandType = CommandType.Text
                        };
                        command.Parameters.AddWithValue("@Id", idItemG);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Location deleted successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
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
                using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new(dataGridsInformation.UpdateTables("location"), connection)
                    {
                        CommandType = CommandType.Text
                    };
                    command.Parameters.AddWithValue("@AC", acronymTextBox.Text);
                    command.Parameters.AddWithValue("@DE", descriptionTextBox.Text);
                    command.Parameters.AddWithValue("@ID", idItemG);
                    if (command.ExecuteNonQuery() != 0)
                    {
                        MessageBox.Show("Location updated successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
    }
}
