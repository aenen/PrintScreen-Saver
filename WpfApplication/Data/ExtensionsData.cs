using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication.Data
{
    static public class ExtensionsData
    {
        /// <summary>
        /// Створює System.Windows.Media.Color колір з System.Drawing.Color
        /// </summary>
        public static System.Windows.Media.Color ToMediaColor(this System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        /// <summary>
        /// Створює SolidColorBrush на основі System.Windows.Media.Color
        /// </summary>
        public static System.Windows.Media.SolidColorBrush ToSolidColorBrush(this System.Windows.Media.Color color)
        {
            return new System.Windows.Media.SolidColorBrush(color);
        }
    }
}
