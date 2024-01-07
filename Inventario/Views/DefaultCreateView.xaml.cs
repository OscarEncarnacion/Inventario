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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventario.Views
{
    /// <summary>
    /// Lógica de interacción para DefaultCreateView.xaml
    /// </summary>
    public partial class DefaultCreateView : UserControl
    {
        public DefaultCreateView(string info)
        {
            InitializeComponent();
            ConfigMessage(info);
        }

        private void ConfigMessage(String info)
        {
            infoLabel.Content = info;
        }
    }
}
