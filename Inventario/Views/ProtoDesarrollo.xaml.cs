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
    /// Lógica de interacción para ProtoDesarrollo.xaml
    /// </summary>
    public partial class ProtoDesarrollo : UserControl
    {
        private decimal idUserG = 0;
        private decimal idItemG = 0;
        private readonly DataGridsInformation dataGridsInformation = new();
        private Button btnDelete = new()
        {
            Content = "Delete sample",
        };
        private Button btnEdit = new()
        {
            Content = "Edit sample",
            Margin = new Thickness(15, 0, 15, 0)
        };
        private Button btnUpdate = new()
        {
            Content = "Update"
        };

        // Constructor para crear un prototipo en desarrollo
        public ProtoDesarrollo(decimal idUser)
        {
            InitializeComponent();
            ConfigureView(0, idUser, "create");
        }

        // Constructor para ver o editar un prototipo en desarrollo
        public ProtoDesarrollo(decimal idItem, decimal idUser, bool isAdmin)
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
                    lblTitle.Content = "Sample view";
                }
                else if (configType == "edit")
                {
                    btnEdit.Click += BtnEdit_Click;
                    editButtonStackPanel.Children.Add(btnEdit);
                    idItemG = Convert.ToDecimal(idItem);
                    lblTitle.Content = "Sample edit";
                    btnDelete.Click += BtnDelete_Click;
                    btnCleanDeleteDockPanel.Children.Add(btnDelete);
                }
            }
            else
            {
                if (configType == "create")
                {
                    dateDeliveryDatePicker.SelectedDate = DateTime.Now;
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

                // Discipline ComboBox
                dataTable = new();
                command = new(dataGridsInformation.SelectDiscipline(), connection)
                {
                    CommandType = CommandType.Text
                };
                adapter = new(command);
                adapter.Fill(dataTable);
                newRow = dataTable.NewRow();
                newRow["ID_DISCIPLINE"] = 0;
                newRow["ACRONYM"] = "--SELECT--";
                dataTable.Rows.InsertAt(newRow, 0);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    disciplineComboBox.ItemsSource = dataTable.DefaultView;
                }
                disciplineComboBox.DisplayMemberPath = "ACRONYM";
                disciplineComboBox.SelectedValuePath = "ID_DISCIPLINE";
                disciplineComboBox.SelectedIndex = 0;

                // Variant ComboBox
                dataTable = new();
                command = new(dataGridsInformation.SelectVariant(), connection)
                {
                    CommandType = CommandType.Text
                };
                adapter = new(command);
                adapter.Fill(dataTable);
                newRow = dataTable.NewRow();
                newRow["ID_VARIANT"] = 0;
                newRow["DESCRIPTION"] = "--SELECT--";
                dataTable.Rows.InsertAt(newRow, 0);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    variantComboBox.ItemsSource = dataTable.DefaultView;
                }
                variantComboBox.DisplayMemberPath = "DESCRIPTION";
                variantComboBox.SelectedValuePath = "ID_VARIANT";
                variantComboBox.SelectedIndex = 0;

                // Type test ComboBox
                dataTable = new();
                command = new(dataGridsInformation.SelectTypeTest(), connection)
                {
                    CommandType = CommandType.Text
                };
                adapter = new(command);
                adapter.Fill(dataTable);
                newRow = dataTable.NewRow();
                newRow["ID_TYPE_TEST"] = 0;
                newRow["DESCRIPTION"] = "--SELECT--";
                dataTable.Rows.InsertAt(newRow, 0);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    typeTestComboBox.ItemsSource = dataTable.DefaultView;
                }
                typeTestComboBox.DisplayMemberPath = "DESCRIPTION";
                typeTestComboBox.SelectedValuePath = "ID_TYPE_TEST";
                typeTestComboBox.SelectedIndex = 0;
            }
        }

        private void FillFields(decimal idSample)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
            {
                connection.Open();
                SqlCommand command = new(dataGridsInformation.SelectSample("form"), connection)
                {
                    CommandType = CommandType.Text
                };
                command.Parameters.AddWithValue("@ID", idSample);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    serialNumberTextBox.Text = reader["SERIAL_NUMBER"].ToString();
                    serialCaseTextBox.Text = reader["SERIAL_CASE"].ToString();
                    hardwareLevelTextBox.Text = reader["HW_LEVEL"].ToString();
                    softwareLevelTextBox.Text = reader["SW_LEVEL"].ToString();
                    deliveredToTextBox.Text = reader["DELIVERED_TO"].ToString();
                    if (DateTime.TryParse(reader["DATE_DELIVERY"].ToString(), out DateTime date))
                    {
                        dateDeliveryDatePicker.SelectedDate = date;
                    }
                    projectComboBox.SelectedValue = reader["ID_PROJECT"].ToString();
                    disciplineComboBox.SelectedValue = reader["ID_DISCIPLINE"].ToString();
                    variantComboBox.SelectedValue = reader["ID_VARIANT"].ToString();
                    typeTestComboBox.SelectedValue = reader["ID_TYPE_TEST"].ToString();
                    jobTextBox.Text = reader["JOB"].ToString();
                    if (Boolean.TryParse(reader["REWORK_SAMPLE"].ToString(), out bool reworkSample))
                    {
                        reworkSampleCheckBox.IsChecked = reworkSample;
                    }
                    if (Boolean.TryParse(reader["REWORK_CONNECTOR"].ToString(), out bool reworkConnector))
                    {
                        reworkConnectorCheckBox.IsChecked = reworkConnector;
                    }
                    if (Boolean.TryParse(reader["FUNCTIONAL_TEST"].ToString(), out bool functionalTest))
                    {
                        functionalTestCheckBox.IsChecked = functionalTest;
                    }
                    if (Boolean.TryParse(reader["APTIV_CONNECTOR"].ToString(), out bool aptiv))
                    {
                        aptivConnectorCheckBox.IsChecked = aptiv;
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
                serialCaseTextBox.TextChanged += TextBox_TextChanged;
                hardwareLevelTextBox.TextChanged += TextBox_TextChanged;
                softwareLevelTextBox.TextChanged += TextBox_TextChanged;
                deliveredToTextBox.TextChanged += TextBox_TextChanged;
                dateDeliveryDatePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
                projectComboBox.SelectionChanged += ComboBox_SelectionChanged;
                disciplineComboBox.SelectionChanged += ComboBox_SelectionChanged;
                variantComboBox.SelectionChanged += ComboBox_SelectionChanged;
                typeTestComboBox.SelectionChanged += ComboBox_SelectionChanged;
                jobTextBox.TextChanged += TextBox_TextChanged;
                reworkSampleCheckBox.Click += CheckBox_Click;
                reworkConnectorCheckBox.Click += CheckBox_Click;
                functionalTestCheckBox.Click += CheckBox_Click;
                aptivConnectorCheckBox.Click += CheckBox_Click;
                commentsTextBox.TextChanged += TextBox_TextChanged;
                btnUpdate.IsEnabled = false;
            }
            else
            {
                serialNumberTextBox.TextChanged -= TextBox_TextChanged;
                serialCaseTextBox.TextChanged -= TextBox_TextChanged;
                hardwareLevelTextBox.TextChanged -= TextBox_TextChanged;
                softwareLevelTextBox.TextChanged -= TextBox_TextChanged;
                deliveredToTextBox.TextChanged -= TextBox_TextChanged;
                dateDeliveryDatePicker.SelectedDateChanged -= DatePicker_SelectedDateChanged;
                projectComboBox.SelectionChanged -= ComboBox_SelectionChanged;
                disciplineComboBox.SelectionChanged -= ComboBox_SelectionChanged;
                variantComboBox.SelectionChanged -= ComboBox_SelectionChanged;
                typeTestComboBox.SelectionChanged -= ComboBox_SelectionChanged;
                jobTextBox.TextChanged -= TextBox_TextChanged;
                reworkSampleCheckBox.Click -= CheckBox_Click;
                reworkConnectorCheckBox.Click -= CheckBox_Click;
                functionalTestCheckBox.Click -= CheckBox_Click;
                aptivConnectorCheckBox.Click -= CheckBox_Click;
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
            serialCaseTextBox.IsEnabled = isActive;
            hardwareLevelTextBox.IsEnabled = isActive;
            softwareLevelTextBox.IsEnabled = isActive;
            deliveredToTextBox.IsEnabled = isActive;
            dateDeliveryDatePicker.IsEnabled = isActive;
            projectComboBox.IsEnabled = isActive;
            disciplineComboBox.IsEnabled = isActive;
            variantComboBox.IsEnabled = isActive;
            typeTestComboBox.IsEnabled = isActive;
            jobTextBox.IsEnabled = isActive;
            reworkSampleCheckBox.IsEnabled = isActive;
            reworkConnectorCheckBox.IsEnabled = isActive;
            functionalTestCheckBox.IsEnabled = isActive;
            aptivConnectorCheckBox.IsEnabled = isActive;
            commentsTextBox.IsEnabled = isActive;
        }

        #region "Validations"

        private bool IsInformationValid()
        {
            bool isValid = false;
            RemoveAlerts();
            if (IsSerialNumberValid())
            {
                if (IsSerialCaseValid())
                {
                    if (IsHardwareLevelValid())
                    {
                        if (IsSoftwareLevelValid())
                        {
                            if (IsDeliveredToValid())
                            {
                                if (IsDateDeliveryValid())
                                {
                                    if (IsProjectValid())
                                    {
                                        if (IsDisciplineValid())
                                        {
                                            if (isVariantValid())
                                            {
                                                if (IsTypeTestValid())
                                                {
                                                    if (IsJobValid())
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

        private bool IsJobValid()
        {
            if (jobTextBox.Text.Length > 0 && jobTextBox.Text.Length < 11)
            {
                return true;
            }
            else
            {
                statusBar.Content = "The job field must not be empty";
                jobTextBox.BorderBrush = Brushes.Red;
                jobTextBox.Focus();
                return false;
            }
        }

        private bool IsTypeTestValid()
        {
            if (typeTestComboBox.SelectedIndex > 0)
            {
                return true;
            }
            else
            {
                statusBar.Content = "A type test must be selected";
                typeTestBorder.BorderBrush = Brushes.Red;
                typeTestComboBox.Focus();
                return false;
            }
        }

        private bool isVariantValid()
        {
            if (variantComboBox.SelectedIndex > 0)
            {
                return true;
            }
            else
            {
                statusBar.Content = "A variant must be selected";
                variantBorder.BorderBrush = Brushes.Red;
                variantComboBox.Focus();
                return false;
            }
        }

        private bool IsDisciplineValid()
        {
            if (disciplineComboBox.SelectedIndex > 0)
            {
                return true;
            }
            else
            {
                statusBar.Content = "A discipline must be selected";
                disciplineBorder.BorderBrush = Brushes.Red;
                disciplineComboBox.Focus();
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

        private bool IsDateDeliveryValid()
        {
            if (dateDeliveryDatePicker.SelectedDate != null)
            {
                return true;
            }
            else
            {
                statusBar.Content = "You must select a date";
                dateDeliveryDatePicker.BorderBrush = Brushes.Red;
                dateDeliveryDatePicker.Focus();
                return false;
            }
        }

        private bool IsDeliveredToValid()
        {
            if (deliveredToTextBox.Text.Length > 0 && deliveredToTextBox.Text.Length < 51)
            {
                return true;
            }
            else
            {
                statusBar.Content = "The delivered to field must not be empty";
                deliveredToTextBox.BorderBrush = Brushes.Red;
                deliveredToTextBox.Focus();
                return false;
            }
        }

        private bool IsSoftwareLevelValid()
        {
            if (softwareLevelTextBox.Text.Length > 0 && softwareLevelTextBox.Text.Length < 51)
            {
                return true;
            }
            else
            {
                statusBar.Content = "The software level field must not be empty";
                softwareLevelTextBox.BorderBrush = Brushes.Red;
                softwareLevelTextBox.Focus();
                return false;
            }
        }

        private bool IsHardwareLevelValid()
        {
            if (hardwareLevelTextBox.Text.Length > 0 && hardwareLevelTextBox.Text.Length < 51)
            {
                return true;
            }
            else
            {
                statusBar.Content = "The hardware level field must not be empty";
                hardwareLevelTextBox.BorderBrush = Brushes.Red;
                hardwareLevelTextBox.Focus();
                return false;
            }
        }

        private bool IsSerialCaseValid()
        {
            if (serialCaseTextBox.Text.Length > 0 && serialCaseTextBox.Text.Length < 51)
            {
                return true;
            }
            else
            {
                statusBar.Content = "The serial case field must not be empty";
                serialCaseTextBox.BorderBrush = Brushes.Red;
                serialCaseTextBox.Focus();
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
                    SqlCommand command = new(dataGridsInformation.SelectSample("serialNumber"), connection)
                    {
                        CommandType = CommandType.Text
                    };
                    command.Parameters.AddWithValue("@SN", serialNumberTextBox.Text);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        if (lblTitle.Content.ToString() == "Sample edit")
                        {
                            if (reader["ID_SAMPLE"].ToString() == idItemG.ToString())
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
            serialCaseTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            hardwareLevelTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            softwareLevelTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            deliveredToTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            dateDeliveryDatePicker.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            projectBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            disciplineBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            variantBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            typeTestBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            jobTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
            commentsTextBox.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 171, 173, 179));
        }

        private void CleanFields()
        {
            serialNumberTextBox.Text = string.Empty;
            serialCaseTextBox.Text = string.Empty;
            hardwareLevelTextBox.Text = string.Empty;
            softwareLevelTextBox.Text = string.Empty;
            deliveredToTextBox.Text = string.Empty;
            dateDeliveryDatePicker.SelectedDate = DateTime.Now;
            projectComboBox.SelectedIndex = 0;
            disciplineComboBox.SelectedIndex = 0;
            variantComboBox.SelectedIndex = 0;
            typeTestComboBox.SelectedIndex = 0;
            jobTextBox.Text = string.Empty;
            reworkSampleCheckBox.IsChecked = false;
            reworkConnectorCheckBox.IsChecked = false;
            functionalTestCheckBox.IsChecked = false;
            aptivConnectorCheckBox.IsChecked = false;
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
                    if (Decimal.TryParse(disciplineComboBox.SelectedValue.ToString(), out decimal idDiscipline))
                    {
                        if (Decimal.TryParse(variantComboBox.SelectedValue.ToString(), out decimal idVariant))
                        {
                            if (Decimal.TryParse(typeTestComboBox.SelectedValue.ToString(), out decimal idTypeTest))
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
                                        command = new(dataGridsInformation.Inserts("sample"), connection)
                                        {
                                            CommandType = CommandType.Text
                                        };
                                        command.Parameters.AddWithValue("@SN", serialNumberTextBox.Text);
                                        command.Parameters.AddWithValue("@HW", hardwareLevelTextBox.Text);
                                        command.Parameters.AddWithValue("@SW", softwareLevelTextBox.Text);
                                        command.Parameters.AddWithValue("@DT", deliveredToTextBox.Text);
                                        command.Parameters.AddWithValue("@DI", idDiscipline);
                                        command.Parameters.AddWithValue("@FT", functionalTestCheckBox.IsChecked);
                                        command.Parameters.AddWithValue("@RS", reworkSampleCheckBox.IsChecked);
                                        command.Parameters.AddWithValue("@RC", reworkConnectorCheckBox.IsChecked);
                                        command.Parameters.AddWithValue("@AC", aptivConnectorCheckBox.IsChecked);
                                        command.Parameters.AddWithValue("@CO", commentsTextBox.Text);
                                        command.Parameters.AddWithValue("@SC", serialCaseTextBox.Text);
                                        command.Parameters.AddWithValue("@DD", dateDeliveryDatePicker.SelectedDate);
                                        command.Parameters.AddWithValue("@IP", idProject);
                                        command.Parameters.AddWithValue("@IBL", idBAL);
                                        command.Parameters.AddWithValue("@IV", idVariant);
                                        command.Parameters.AddWithValue("@JO", jobTextBox.Text);
                                        command.Parameters.AddWithValue("@ITT", idTypeTest);
                                        command.Parameters.AddWithValue("@IU", idUserG);
                                        if (command.ExecuteNonQuery() != 0)
                                        {
                                            MessageBox.Show("Sample created successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Sorry, sample could not be created", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                                        }
                                        CleanFields();
                                        CloseWindow();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this sample?", "Information",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(dataGridsInformation.DeleteCommand("SAMPLE"), connection)
                    {
                        CommandType = CommandType.Text
                    };
                    command.Parameters.AddWithValue("@Id", idItemG);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Sample deleted successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    if (Decimal.TryParse(disciplineComboBox.SelectedValue.ToString(), out decimal idDiscipline))
                    {
                        if (Decimal.TryParse(variantComboBox.SelectedValue.ToString(), out decimal idVariant))
                        {
                            if (Decimal.TryParse(typeTestComboBox.SelectedValue.ToString(), out decimal idTypeTest))
                            {
                                using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                                {
                                    connection.Open();
                                    SqlCommand command = new(dataGridsInformation.UpdateTables("sample"), connection)
                                    {
                                        CommandType = CommandType.Text
                                    };
                                    command.Parameters.AddWithValue("@ID", idItemG);
                                    command.Parameters.AddWithValue("@SN", serialNumberTextBox.Text);
                                    command.Parameters.AddWithValue("@HW", hardwareLevelTextBox.Text);
                                    command.Parameters.AddWithValue("@SW", softwareLevelTextBox.Text);
                                    command.Parameters.AddWithValue("@DT", deliveredToTextBox.Text);
                                    command.Parameters.AddWithValue("@DI", idDiscipline);
                                    command.Parameters.AddWithValue("@FT", functionalTestCheckBox.IsChecked);
                                    command.Parameters.AddWithValue("@RS", reworkSampleCheckBox.IsChecked);
                                    command.Parameters.AddWithValue("@RC", reworkConnectorCheckBox.IsChecked);
                                    command.Parameters.AddWithValue("@AC", aptivConnectorCheckBox.IsChecked);
                                    command.Parameters.AddWithValue("@CO", commentsTextBox.Text);
                                    command.Parameters.AddWithValue("@SC", serialCaseTextBox.Text);
                                    command.Parameters.AddWithValue("@DD", dateDeliveryDatePicker.SelectedDate);
                                    command.Parameters.AddWithValue("@IP", idProject);
                                    command.Parameters.AddWithValue("@IV", idVariant);
                                    command.Parameters.AddWithValue("@JO", jobTextBox.Text);
                                    command.Parameters.AddWithValue("@ITT", idTypeTest);
                                    command.Parameters.AddWithValue("@IU", idUserG);
                                    if (command.ExecuteNonQuery() != 0)
                                    {
                                        MessageBox.Show("Sample updated successfully", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
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
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }
    }
}
