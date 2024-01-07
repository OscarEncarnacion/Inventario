using Inventario.Clases;
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
    /// Lógica de interacción para MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public Button createButton = new Button()
        {
            Content = "Create",
            FontSize = 18,
            ToolTip = "Create new record"
        };
        private readonly DataGridsInformation dataGridsInformation = new();
        private decimal currentUserID;
        private readonly Dictionary<string, string> treeViewNames = new()
        {
            { "Samples", "SAMPLE" },
            { "VNs", "VN" },
            { "Disciplines", "DISCIPLINE" },
            { "Projects", "PROJECT" },
            { "Business Area-Location", "BA_LOCATION" },
            { "Locations", "LOCATION" },
            { "Business Areas", "BUSINESS_AREA" },
            { "Variants", "VARIANT" },
            { "Type Tests", "TYPE_TEST" },
            { "Users", "INVENTORY_USERS" }
        };

        private readonly string[] materialTables = { "Samples", "VNs" };
        private readonly string[] catalogTables = { "Disciplines", "Projects", "Business Areas", "Locations", "Business Area-Location", "Variants", "Type Tests" };
        private readonly string[] adminTables = { "Users" };

        public MenuView(List<string?> loginInfo)
        {
            InitializeComponent();
            CustomizeView(loginInfo);
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            int role = GetUserRole();
            bool isAdmin = false;
            if (role == 1)
                isAdmin = true;
            string? tableNameDisplayed = selectedMenu.Content.ToString();
            if (materialTables.Contains(tableNameDisplayed))
            {
                EditWindow editWindow = new(tableNameDisplayed, currentUserID);
                editWindow.ShowDialog();
                ResetDataGridMenuView();
                UploadDataGridInformation(selectedMenu.Content.ToString());
            }
            else if (catalogTables.Contains(tableNameDisplayed) || adminTables.Contains(tableNameDisplayed))
            {
                if (isAdmin)
                {
                    EditWindow editWindow = new(tableNameDisplayed);
                    editWindow.ShowDialog();
                    ResetDataGridMenuView();
                    UploadDataGridInformation(selectedMenu.Content.ToString());
                }
            }
            else
            {
                MessageBox.Show("There is a problem with this table", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TviDevices_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ResetDataGridMenuView();
            TreeViewItem treeViewItem = (TreeViewItem)sender;
            string? key = treeViewItem.Header.ToString();
            UploadDataGridInformation(key);
        }

        private void UploadDataGridInformation(string? key)
        {
            if (key != null)
            {
                bool addCreateButton = false;
                if (GetUserRole() == 1)
                {
                    addCreateButton = true;
                }
                else
                {
                    if (materialTables.Contains(key))
                    {
                        addCreateButton = true;
                    }
                }
                if (treeViewNames.TryGetValue(key, out string? tableName))
                {
                    selectedMenu.Content = key;
                    GetDataMenu(tableName, addCreateButton);
                }
                else
                {
                    ResetDataGridMenuView();
                    MessageBox.Show($"The table {key} doesn't exist", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                ResetDataGridMenuView();
                MessageBox.Show($"The table {key} doesn't exist", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CustomizeView(List<string?> loginInfo)
        {
            if (Decimal.TryParse(loginInfo[1], out decimal userID))
            {
                createButton.Click += BtnCreate_Click;
                lblWelcome.Content += $"\n{loginInfo[2]} {loginInfo[3]}";
                currentUserID = userID;
                if (loginInfo[4] == "1")
                {
                    TreeViewItem adminTreeViewItem = new()
                    {
                        Header = "Admin",
                        FontSize = 14
                    };
                    TreeViewItem usersTreeViewItem = new TreeViewItem()
                    {
                        Header = "Users",
                        FontSize = 12
                    };
                    usersTreeViewItem.MouseDoubleClick += new MouseButtonEventHandler(this.TviDevices_MouseLeftButtonUp);
                    usersTreeViewItem.KeyDown += new KeyEventHandler(this.TreeViewItem_EnterDown);
                    adminTreeViewItem.Items.Add(usersTreeViewItem);
                    treeView.Items.Add(adminTreeViewItem);
                }
            }
            else
            {
                currentUserID = 0;
                MessageBox.Show("there is an error with the user", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                var myWindow = Window.GetWindow(this);
                myWindow.Content = new Views.LoginView();

            }
        }

        private int GetUserRole()
        {
            using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(dataGridsInformation.SelectUserRole(), connection)
                {
                    CommandType = CommandType.Text
                };
                command.Parameters.AddWithValue("@ID", currentUserID);
                SqlDataReader reader = command.ExecuteReader();
                string? roleString = "";
                if (reader.Read())
                {
                    roleString = reader["ROLE"].ToString();
                }
                if (int.TryParse(roleString, out int role))
                {
                    return role;
                }
                else
                {
                    return -1;
                }
            }
        }

        private void GetDataMenu(string table, bool addCreateBtn)
        {
            using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
            {
                DataTable dataTable = new();
                connection.Open();
                SqlCommand command = new(dataGridsInformation.SelectTable(table), connection)
                {
                    CommandType = CommandType.Text
                };
                SqlDataAdapter adapter = new(command);
                adapter.Fill(dataTable);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    dataGrid.Columns.Add(new CustomDataGridTemplateColumn("view"));
                    foreach (var data in dataGridsInformation.GetDataTableInformation(table))
                    {
                        dataGrid.Columns.Add(new DataGridTextColumn()
                        {
                            Header = data.Key,
                            CanUserResize = false,
                            CanUserReorder = false,
                            IsReadOnly = true,
                            Binding = new Binding(data.Value)
                        });
                    }
                    dataGrid.ItemsSource = dataTable.DefaultView;
                    AddClickEventDataGrid();
                }
                AddCreateButton(addCreateBtn);
            }
        }

        private void ResetDataGridMenuView()
        {
            dataGrid.ItemsSource = null;
            dataGrid.Columns.Clear();
            RemoveClickEventDataGrid();
            RemoveCreateButton();
        }

        private void AddClickEventDataGrid()
        {
            var style = new Style { TargetType = typeof(DataGridCell) };
            var eventSetter = new EventSetter(MouseLeftButtonUpEvent, new MouseButtonEventHandler(DataGridCell_MouseLeftButtonUp));
            style.Setters.Add(eventSetter);
            dataGrid.Resources.Add(typeof(DataGridCell), style);
        }

        private void RemoveClickEventDataGrid()
        {
            if (dataGrid.Resources.Keys.Count != 0)
            {
                dataGrid.Resources.Remove(typeof(DataGridCell));
            }
        }

        private void AddCreateButton(bool addBtn)
        {
            if (addBtn)
            {
                if (!createButtonDockPanel.Children.Contains(createButton))
                {
                    createButtonDockPanel.Children.Add(createButton);
                }
            }
            else
            {
                createButtonDockPanel.Children.Clear();
            }
        }

        private void RemoveCreateButton()
        {
            if (createButtonDockPanel.Children.Contains(createButton))
            {
                createButtonDockPanel.Children.Remove(createButton);
            }
        }

        private void DataGridCell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGridCell dataGridCellSelected = (DataGridCell)sender;
                DataRowView? rowSelected = dataGrid.CurrentItem as DataRowView;
                if (dataGridCellSelected != null)
                {
                    if (rowSelected != null)
                    {
                        if (dataGrid.Items.Count > 0)
                        {
                            if (Decimal.TryParse(rowSelected[0].ToString(), out decimal idDecimal))
                            {
                                if (int.TryParse(dataGridCellSelected.Column.DisplayIndex.ToString(), out int indexColumn))
                                {
                                    string? tableNameDisplayed = selectedMenu.Content.ToString();
                                    if (tableNameDisplayed != null && tableNameDisplayed != "Void")
                                    {
                                        if (treeViewNames.TryGetValue(tableNameDisplayed, out string? tableName))
                                        {
                                            if (indexColumn == 0)
                                            {
                                                if (Decimal.TryParse(currentUserID.ToString(), out decimal idUserDecimal))
                                                {
                                                    int role = GetUserRole();
                                                    bool isAdmin = false;
                                                    if (role == 1)
                                                        isAdmin = true;
                                                    if (materialTables.Contains(tableNameDisplayed))
                                                    {
                                                        if (!isAdmin)
                                                        {
                                                            decimal ownerID = 0;
                                                            using (SqlConnection connection = new(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
                                                            {
                                                                connection.Open();
                                                                SqlCommand command = new(dataGridsInformation.SelectOwner(tableName), connection)
                                                                {
                                                                    CommandType = CommandType.Text
                                                                };
                                                                command.Parameters.AddWithValue("@ID", idDecimal);
                                                                SqlDataReader reader = command.ExecuteReader();
                                                                if (reader.Read())
                                                                {
                                                                    if (decimal.TryParse(reader["ID_USER"].ToString(), out decimal owner))
                                                                    {
                                                                        ownerID = owner;
                                                                    }
                                                                }
                                                            }
                                                            if (ownerID == currentUserID)
                                                            {
                                                                isAdmin = true;
                                                            }
                                                        }
                                                        EditWindow editWindow = new(tableNameDisplayed, idDecimal, idUserDecimal, isAdmin);
                                                        editWindow.ShowDialog();
                                                        ResetDataGridMenuView();
                                                        UploadDataGridInformation(selectedMenu.Content.ToString());

                                                    }
                                                    else if (catalogTables.Contains(tableNameDisplayed))
                                                    {
                                                        EditWindow editWindow = new(tableNameDisplayed, idDecimal, isAdmin);
                                                        editWindow.ShowDialog();
                                                        ResetDataGridMenuView();
                                                        UploadDataGridInformation(selectedMenu.Content.ToString());
                                                    }
                                                    else if (adminTables.Contains(tableNameDisplayed))
                                                    {
                                                        if (tableNameDisplayed == "Users")
                                                        {
                                                            if (idDecimal == idUserDecimal)
                                                            {
                                                                EditWindow editWindow = new(idDecimal, false, true);
                                                                editWindow.ShowDialog();
                                                                ResetDataGridMenuView();
                                                                UploadDataGridInformation(selectedMenu.Content.ToString());
                                                            }
                                                            else
                                                            {
                                                                EditWindow editWindow = new(tableNameDisplayed, idDecimal, isAdmin);
                                                                editWindow.ShowDialog();
                                                                ResetDataGridMenuView();
                                                                UploadDataGridInformation(selectedMenu.Content.ToString());
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("There is a problem with this table", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("There is a problem with the user", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Something is wrong", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void HomeIcon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectedMenu.Content = "Void";
            ResetDataGridMenuView();
            foreach (TreeViewItem tree in treeView.Items)
            {
                CollapseTreeViewItems(tree);
            }
            foreach (TreeViewItem tree in treeView.Items)
            {
                tree.IsSelected = false;
            }
        }

        private void PerfilIcon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Decimal.TryParse(currentUserID.ToString(), out decimal idDecimal))
            {
                EditWindow editWindow = new(idDecimal, false, true);
                editWindow.ShowDialog();
                if (selectedMenu.Content.ToString() == "Users")
                {
                    ResetDataGridMenuView();
                    UploadDataGridInformation(selectedMenu.Content.ToString());
                }
            }
            else
            {
                MessageBox.Show("There is a problem with the user", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExitIcon_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow actualWindow = (MainWindow)Window.GetWindow(this);
            actualWindow.MainContent.Content = new LoginView();
            actualWindow.MinWidth = ((LoginView)actualWindow.MainContent.Content).MinWidth + 17;
            actualWindow.MinHeight = ((LoginView)actualWindow.MainContent.Content).MinHeight + 43;
            actualWindow.Width = actualWindow.MinWidth + 66;
            actualWindow.Height = actualWindow.MinHeight + 66;
        }

        private void CollapseTreeViewItems(TreeViewItem tree)
        {
            foreach (TreeViewItem treeItem in tree.Items)
            {
                if (treeItem.HasItems)
                {
                    CollapseTreeViewItems(treeItem);
                    treeItem.IsExpanded = false;
                }
            }
            tree.IsExpanded = false;
        }

        private void TreeViewItem_EnterDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Return && e.Key != Key.Enter)
                return;
            e.Handled = true;
            ResetDataGridMenuView();
            TreeViewItem treeViewItem = (TreeViewItem)sender;
            string? key = treeViewItem.Header.ToString();
            UploadDataGridInformation(key);
        }
    }
}
