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

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Drawing.Image snap;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            //{
            //    MessageBox.Show(e.Key.ToString());
            //}

            //return;
            if (e.Key == Key.Snapshot && Clipboard.ContainsImage())
            {
                img_Preview.Source = Clipboard.GetImage();                
                panel_Control.IsEnabled = true;
                //System.Drawing.Image image = (System.Drawing.Image)Clipboard.GetDataObject().GetData(DataFormats.Bitmap);
                //MessageBox.Show("yes");
            }
        }
    }
}
