using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;
using System.Windows.Input;
using WpfApplication.Data;

namespace WpfApplication
{
    /// <summary>
    /// Interaction logic for WindowSettings.xaml
    /// </summary>
    public partial class WindowSettings : Window
    {
        TextBox tb_Directory;

        public WindowSettings()
        {
            InitializeComponent();
        }

        private void panel_Color_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WinForms.ColorDialog cd = new WinForms.ColorDialog();
            if (cd.ShowDialog() == WinForms.DialogResult.OK)
            {
                panel_Color.Background = cd.Color.ToMediaColor().ToSolidColorBrush();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RegistryData.DefaultSettings();
            Close();
        }
    }
}
