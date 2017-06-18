using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;
using System.Windows.Input;
using WpfApplication.Data;

namespace WpfApplication
{
    public partial class WindowSettings : Window
    {
        TextBox tb_Directory;   // textbox з controltemplate // краще було б прив'язати його до якогось string SavePath

        public WindowSettings()
        {
            InitializeComponent();
        }

        // Зміна кольору
        private void panel_Color_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WinForms.ColorDialog cd = new WinForms.ColorDialog();
            if (cd.ShowDialog() == WinForms.DialogResult.OK)
            {
                panel_Color.Background = cd.Color.ToMediaColor().ToSolidColorBrush();
            }
        }

        // Зміна дерикторії
        private void b_SetDirectory_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog dialog = new WinForms.FolderBrowserDialog();
            if (dialog.ShowDialog() == WinForms.DialogResult.OK)
            {
                tb_Directory.Text = dialog.SelectedPath;
            }
        }

        // Ініціалізація
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var template = panel_Directory.Template;
            tb_Directory = (TextBox)template.FindName("tb_Directory", panel_Directory);
        }

        // Клік "Стандартні налаштування"
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RegistryData.DefaultSettings();
            this.DialogResult = true;
            Close();
        }
    }
}
