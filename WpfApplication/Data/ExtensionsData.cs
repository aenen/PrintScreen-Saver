using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication.Data
{
    static public class ExtensionsData
    {
        public static System.Windows.Media.Color ToMediaColor(this System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        public static System.Windows.Media.SolidColorBrush ToSolidColorBrush(this System.Windows.Media.Color color)
        {
            return new System.Windows.Media.SolidColorBrush(color);
        }
    }
}
