using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using WinForms = System.Windows.Forms; // тільки що дізнався, що так можна робити. круто
using System.Reflection;
using WpfApplication.Data;
using MyLibrary;
using System.Collections.Generic;

namespace WpfApplication
{
    public partial class MainWindow : Window
    {
        TextBox tb_Directory;   // textbox з controltemplate // краще було б прив'язати його до якогось string SavePath
        BitmapSource image;     // сам скріншот
        WinForms.NotifyIcon ni; // трей-іконка
        bool menuClose = false; // флажок для закриття програми виключно з меню трей-іконки

        public MainWindow()
        {
            InitializeComponent();
        }

        // Зміна властивості Stretch контролу Image при ресайзі
        private void Border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (img_Preview.Source != null)
            {
                Border border = sender as Border;
                img_Preview.Stretch = img_Preview.Source.Height > border.ActualHeight || img_Preview.Source.Width > border.ActualWidth ? Stretch.Uniform : Stretch.None;
            }
        }

        // Вибір дерикорії для збереження скріншоту
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
            NotifyIconInit();
            
            RegistryData.FavColorChanged += (s, ea) => lgb_Background.GradientStops[0].Color = RegistryData.FavColor;

            new ClipboardManager(this).ClipboardChanged += ClipboardChanged;
            ClipboardChanged(this, EventArgs.Empty);
        }

        // Обробник події зміни даних в буфері обміну
        private void ClipboardChanged(object sender, EventArgs e)
        {
            if (Clipboard.ContainsImage())
            {
                img_Preview.Source = image = Clipboard.GetImage();
                panel_Control.IsEnabled = true;
                tb_Directory.Text = string.IsNullOrEmpty(tb_Directory.Text)? RegistryData.SavePath : tb_Directory.Text;
                if (RegistryData.AutoGenerateName) tb_Name.Text = DateTime.Now.ToString("yyyy'-'MM'-'dd'_'HH'-'mm'-'ss");
                if (RegistryData.TrayNotification && (!IsVisible || WindowState == WindowState.Minimized))
                    ni.ShowBalloonTip(1000);
            }
        }

        // Збереження скріншоту
        private void b_Save_Click(object sender, RoutedEventArgs e)
        {
            if (!MyDataCorrection.CombineMethods(
                new MyValidationTextBoxParams(MyDataCorrection.IsValidFilename, new[] { tb_Name }, true),
                new MyValidationTextBoxParams(MyDataCorrection.IsBlank, new[] { tb_Directory }, false)))
            {
                this.Show();
                this.WindowState = WindowState.Normal;
                return;
            }

            if (RegistryData.ConfirmSave && (!IsVisible || WindowState == WindowState.Minimized))
            {
                MessageBoxResult mb = MessageBox.Show($"{tb_Name.Text}.png →→→ {tb_Directory.Text}", "Зберегти скріншот?", MessageBoxButton.YesNo);
                if (mb == MessageBoxResult.No)
                    return;
            }

            using (var fileStream = new FileStream(Path.Combine(tb_Directory.Text, tb_Name.Text + ".png"), FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(fileStream);
                if (RegistryData.LatestSavePathAsDefault) RegistryData.SavePath = tb_Directory.Text;
                if (RegistryData.ShowExplorer)
                {
                    string argument = "/select, \"" + Path.Combine(tb_Directory.Text, tb_Name.Text + ".png") + "\"";
                    System.Diagnostics.Process.Start("explorer.exe", argument);
                }
            }
        }

        // Зміна стану вікна (WindowState)
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (RegistryData.MinimizeToTray && WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
        }

        // Пункт меню "Налаштування"
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (new WindowSettings().ShowDialog() == true)
            {
                ni.ContextMenu.MenuItems[0].Checked = RegistryData.TrayNotification;
                ni.ContextMenu.MenuItems[1].Checked = RegistryData.ConfirmSave;
            }
        }

        // Дії при this.Close()
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (RegistryData.MinimizeToTray && !menuClose)
            {
                e.Cancel = true;
                WindowState = WindowState.Minimized;
            }
            else
            {
                ni.Visible = false;
                ni.Icon = null;
            }
        }

        // Ініціалізація 
        void NotifyIconInit()
        {
            ni = new WinForms.NotifyIcon();
            ni.Visible = true;
            ni.Text = Title;
            ni.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            ni.BalloonTipText = "Я бачу нове зображення! Зберегти?";
            ni.BalloonTipTitle = "Новий скріншот.";
            ni.BalloonTipClicked += (s, ea) => 
            b_Save_Click(this, null);

            ni.ContextMenu = new WinForms.ContextMenu();
            WinForms.MenuItem[] mItems = new WinForms.MenuItem[]
            {
                new WinForms.MenuItem { Text = "Сповіщення", Checked = RegistryData.TrayNotification },
                new WinForms.MenuItem { Text = "Підтвердження збереження", Checked = RegistryData.ConfirmSave },
                new WinForms.MenuItem { Text = "-" },
                new WinForms.MenuItem { Text = "Вихід" }
            };
            mItems[0].Click += (s, ea) =>
            {
                var obj = s as WinForms.MenuItem;
                obj.Checked = !obj.Checked;
                RegistryData.TrayNotification = obj.Checked;
            };
            mItems[1].Click += (s, ea) =>
            {
                var obj = s as WinForms.MenuItem;
                obj.Checked = !obj.Checked;
                RegistryData.ConfirmSave = obj.Checked;
            };
            mItems[3].Click += (s, ea) =>
            {
                menuClose = true;
                this.Close();
            };
            ni.ContextMenu.MenuItems.AddRange(mItems);

            ni.MouseClick += (sndr, args) =>
            {
                if (args.Button == WinForms.MouseButtons.Left)
                {
                    this.Show();
                    WindowState = WindowState.Normal;
                }
            };
        }
    }
}
