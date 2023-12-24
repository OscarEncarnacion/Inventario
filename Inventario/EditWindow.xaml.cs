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

namespace Inventario
{
    /// <summary>
    /// Lógica de interacción para EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        // Constructor to create admin tables
        public EditWindow(string? view)
        {
            InitializeComponent();
            LoadViewCreateAdminTables(view);
        }

        // Constructor to create user tables
        public EditWindow(string? view, decimal idUser)
        {
            InitializeComponent();
            LoadViewCreateUserTables(view, idUser);
        }

        // Constructor to update and view admin tables
        public EditWindow(string? view, decimal id, bool isAdmin)
        {
            InitializeComponent();
            LoadViewAdminTables(view, id, isAdmin);
        }

        // Constructor to update and view user tables
        public EditWindow(string? view, decimal idItem, decimal idUser, bool isAdmin)
        {
            InitializeComponent();
            LoadViewUserTables(view, idItem, idUser, isAdmin);
        }

        // Constructor to view and edit your own user
        public EditWindow(decimal id, bool isAdmin, bool isPerfilIcon)
        {
            InitializeComponent();
            LoadViewUserPerfilIcon(id, isAdmin, isPerfilIcon);
        }

        // Create admin tables
        public void LoadViewCreateAdminTables(string? view)
        {
            switch (view)
            {
                case "Users":
                    EditContent.Content = new UserCreateView();
                    this.MinWidth = ((UserCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((UserCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
                case "Disciplines":
                    EditContent.Content = new DisciplineCreateView();
                    this.MinWidth = ((DisciplineCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((DisciplineCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Projects":
                    EditContent.Content = new ProjectCreateView();
                    this.MinWidth = ((ProjectCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((ProjectCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Business Areas":
                    EditContent.Content = new BACreationView();
                    this.MinWidth = ((BACreationView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((BACreationView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Locations":
                    EditContent.Content = new LocationCreateView();
                    this.MinWidth = ((LocationCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((LocationCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Business Area-Location":
                    EditContent.Content = new BALocationCreationView();
                    this.MinWidth = ((BALocationCreationView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((BALocationCreationView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Variants":
                    EditContent.Content = new VariantCreateView();
                    this.MinWidth = ((VariantCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((VariantCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Type Tests":
                    EditContent.Content = new TypeTestCreateView();
                    this.MinWidth = ((TypeTestCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((TypeTestCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                default:
                    string message = "Something is wrong with the " + view + " view";
                    EditContent.Content = new DefaultCreateView(message);
                    this.MinWidth = ((DefaultCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((DefaultCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
            }
        }

        // View and edit admin tables
        private void LoadViewAdminTables(string? view, decimal id, bool isAdmin)
        {
            switch (view)
            {
                case "Users":
                    EditContent.Content = new UserCreateView(id, isAdmin, false);
                    this.MinWidth = ((UserCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((UserCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
                case "Disciplines":
                    EditContent.Content = new DisciplineCreateView(id, isAdmin);
                    this.MinWidth = ((DisciplineCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((DisciplineCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Projects":
                    EditContent.Content = new ProjectCreateView(id, isAdmin);
                    this.MinWidth = ((ProjectCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((ProjectCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Business Areas":
                    EditContent.Content = new BACreationView(id, isAdmin);
                    this.MinWidth = ((BACreationView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((BACreationView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Locations":
                    EditContent.Content = new LocationCreateView(id, isAdmin);
                    this.MinWidth = ((LocationCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((LocationCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Business Area-Location":
                    EditContent.Content = new BALocationCreationView(id, isAdmin);
                    this.MinWidth = ((BALocationCreationView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((BALocationCreationView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Variants":
                    EditContent.Content = new VariantCreateView(id, isAdmin);
                    this.MinWidth = ((VariantCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((VariantCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Type Tests":
                    EditContent.Content = new TypeTestCreateView(id, isAdmin);
                    this.MinWidth = ((TypeTestCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((TypeTestCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                default:
                    string message = "Something is wrong with the " + view + " view";
                    EditContent.Content = new DefaultCreateView(message);
                    this.MinWidth = ((DefaultCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((DefaultCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
            }
        }

        // Create user tables
        public void LoadViewCreateUserTables(string? view, decimal idUser)
        {
            switch (view)
            {
                case "Samples":
                    EditContent.Content = new SampleCreateView(idUser);
                    this.MinWidth = ((SampleCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((SampleCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
                case "VNs":
                    EditContent.Content = new VNCreateView(idUser);
                    this.MinWidth = ((VNCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((VNCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
                default:
                    string message = "Something is wrong with the " + view + " view";
                    EditContent.Content = new DefaultCreateView(message);
                    this.MinWidth = ((DefaultCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((DefaultCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
            }
        }

        // View and edit user tables
        private void LoadViewUserTables(string? view, decimal idItem, decimal idUser, bool isAdmin)
        {
            switch (view)
            {
                case "Samples":
                    EditContent.Content = new SampleCreateView(idItem, idUser, isAdmin);
                    this.MinWidth = ((SampleCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((SampleCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
                case "VNs":
                    EditContent.Content = new VNCreateView(idItem, idUser, isAdmin);
                    this.MinWidth = ((VNCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((VNCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
                default:
                    string message = "Something is wrong with the " + view + " view";
                    EditContent.Content = new DefaultCreateView(message);
                    this.MinWidth = ((DefaultCreateView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((DefaultCreateView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
            }
        }

        // View and edit your own user
        private void LoadViewUserPerfilIcon(decimal id, bool isAdmin, bool isPerfilIcon)
        {
            EditContent.Content = new UserCreateView(id, isAdmin, isPerfilIcon);
            this.MinWidth = ((UserCreateView)EditContent.Content).MinWidth + 17;
            this.MinHeight = ((UserCreateView)EditContent.Content).MinHeight + 43;
            this.Width = this.MinWidth + 66;
            this.Height = this.MinHeight + 66;
        }
    }
}
