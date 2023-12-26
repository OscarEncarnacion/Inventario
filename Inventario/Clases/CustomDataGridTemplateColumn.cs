using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Inventario.Clases
{
    internal class CustomDataGridTemplateColumn : DataGridTemplateColumn
    {
        public CustomDataGridTemplateColumn(string file)
        {
            Width = 50;
            IsReadOnly = true;
            string toolTip;
            if (file == "edit")
            {
                Header = "Edit";
                toolTip = "Edit register";
            }
            else if (file == "view")
            {
                Header = "View";
                toolTip = "View register";
            }
            else
            {
                Header = "Delete";
                toolTip = "Delete register";
            }
            file = file switch
            {
                "edit" => @"D:\Documentos\GitLocal\Inventario\Inventario\Iconos\editRegister.png",
                "delete" => @"D:\Documentos\GitLocal\Inventario\Inventario\Iconos\delete.png",
                "view" => @"D:\Documentos\GitLocal\Inventario\Inventario\Iconos\eye5.png",
                _ => @"D:\Documentos\GitLocal\Inventario\Inventario\Iconos\rose.png",
            };
            FrameworkElementFactory imageFactory = new(typeof(Image));
            imageFactory.SetValue(Image.WidthProperty, 17.0);
            imageFactory.SetValue(Image.HeightProperty, 17.0);
            imageFactory.SetValue(Image.SourceProperty, new BitmapImage(new Uri(file, UriKind.Absolute)));
            imageFactory.SetValue(Image.ToolTipProperty, toolTip);

            DataTemplate dataTemplate = new()
            {
                VisualTree = imageFactory
            };

            CellTemplate = dataTemplate;
        }
    }
}
