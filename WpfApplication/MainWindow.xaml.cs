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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using WinForms = System.Windows.Forms; // тільки що дізнався, що так можна робити. круто
using System.Reflection;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBox tb_Directory;
        BitmapSource image;
                     
        public MainWindow()
        {
            InitializeComponent();

            WinForms.NotifyIcon ni = new WinForms.NotifyIcon();
            ni.Text = Title;
            ni.Icon = new System.Drawing.Icon("Tray.ico");
            ni.Visible = true;
            ni.Click +=
                delegate (object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };
        }
        
        private void img_Preview_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                img_Preview.Source = image = Clipboard.GetImage();
                panel_Control.IsEnabled = true;
                tb_Name.Text = DateTime.Now.ToString("yyyy'-'MM'-'dd'_'HH'-'mm'-'ss");
            }
        }

        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (img_Preview.Source != null)
            {
                Border border = sender as Border;
                img_Preview.Stretch = img_Preview.Source.Height > border.ActualHeight || img_Preview.Source.Width > border.ActualWidth ? Stretch.Uniform : Stretch.None;
            }
        }

        private void b_SetDirectory_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog dialog = new WinForms.FolderBrowserDialog();
            if (dialog.ShowDialog() == WinForms.DialogResult.OK)
            {
                tb_Directory.Text = dialog.SelectedPath;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var template = panel_Directory.Template;
            tb_Directory = (TextBox)template.FindName("tb_Directory", panel_Directory);
        }

        private void b_Save_Click(object sender, RoutedEventArgs e)
        {
            using (var fileStream = new FileStream(System.IO.Path.Combine(tb_Directory.Text, tb_Name.Text + ".png"), FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized) this.Hide();
        }
    }
}
