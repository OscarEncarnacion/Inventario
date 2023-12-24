using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventario
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MinWidth = ((LoginView)MainContent.Content).MinWidth + 17;
            this.MinHeight = ((LoginView)MainContent.Content).MinHeight + 43;
            this.Width = this.MinWidth + 66;
            this.Height = this.MinHeight + 66;
        }
    }
}