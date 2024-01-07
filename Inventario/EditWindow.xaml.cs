using System;
using Inventario.Views;
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
                case "Usuarios":
                    EditContent.Content = new UsuarioView();
                    this.MinWidth = ((UsuarioView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((UsuarioView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
                case "Disciplinas":
                    EditContent.Content = new DisciplinaView();
                    this.MinWidth = ((DisciplinaView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((DisciplinaView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Proyectos":
                    EditContent.Content = new ProyectoView();
                    this.MinWidth = ((ProyectoView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((ProyectoView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Areas de negocio":
                    EditContent.Content = new ANegocioView();
                    this.MinWidth = ((ANegocioView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((ANegocioView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Locaciones":
                    EditContent.Content = new LocacionView();
                    this.MinWidth = ((LocacionView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((LocacionView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Areas de negocio - Locacion":
                    EditContent.Content = new ANLocacionView();
                    this.MinWidth = ((ANLocacionView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((ANLocacionView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Variantes":
                    EditContent.Content = new VarianteView();
                    this.MinWidth = ((VarianteView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((VarianteView)EditContent.Content).MinHeight + 43;
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
                case "Usuarios":
                    EditContent.Content = new UsuarioView(id, isAdmin, false);
                    this.MinWidth = ((UsuarioView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((UsuarioView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
                case "Disciplinas":
                    EditContent.Content = new DisciplinaView(id, isAdmin);
                    this.MinWidth = ((DisciplinaView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((DisciplinaView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Proyectos":
                    EditContent.Content = new ProyectoView(id, isAdmin);
                    this.MinWidth = ((ProyectoView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((ProyectoView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Areas de negocio":
                    EditContent.Content = new ANegocioView(id, isAdmin);
                    this.MinWidth = ((ANegocioView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((ANegocioView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Locaciones":
                    EditContent.Content = new LocacionView(id, isAdmin);
                    this.MinWidth = ((LocacionView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((LocacionView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Areas de negocio - Locacion":
                    EditContent.Content = new ANLocacionView(id, isAdmin);
                    this.MinWidth = ((ANLocacionView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((ANLocacionView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 10;
                    break;
                case "Variantes":
                    EditContent.Content = new VarianteView(id, isAdmin);
                    this.MinWidth = ((VarianteView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((VarianteView)EditContent.Content).MinHeight + 43;
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
                case "Proto en desarrollo":
                    EditContent.Content = new ProtoDesarrolloView(idUser);
                    this.MinWidth = ((ProtoDesarrolloView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((ProtoDesarrolloView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
                case "Proto en prueba":
                    EditContent.Content = new ProtoPruebaView(idUser);
                    this.MinWidth = ((ProtoPruebaView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((ProtoPruebaView)EditContent.Content).MinHeight + 43;
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
                case "Proto en desarrollo":
                    EditContent.Content = new ProtoDesarrolloView(idItem, idUser, isAdmin);
                    this.MinWidth = ((ProtoDesarrolloView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((ProtoDesarrolloView)EditContent.Content).MinHeight + 43;
                    this.Width = this.MinWidth + 66;
                    this.Height = this.MinHeight + 66;
                    break;
                case "Proto en prueba":
                    EditContent.Content = new ProtoPruebaView(idItem, idUser, isAdmin);
                    this.MinWidth = ((ProtoPruebaView)EditContent.Content).MinWidth + 17;
                    this.MinHeight = ((ProtoPruebaView)EditContent.Content).MinHeight + 43;
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
            EditContent.Content = new UsuarioView(id, isAdmin, isPerfilIcon);
            this.MinWidth = ((UsuarioView)EditContent.Content).MinWidth + 17;
            this.MinHeight = ((UsuarioView)EditContent.Content).MinHeight + 43;
            this.Width = this.MinWidth + 66;
            this.Height = this.MinHeight + 66;
        }
    }
}
